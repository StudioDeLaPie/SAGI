using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeAuthority : NetworkBehaviour {

    private float lastHit;
    private float cooldown = 0.5f;
    private Coroutine coroutineReset;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Player" && isServer)
        {
            CmdAssignNetworkAuthority(GetComponent<NetworkIdentity>(), collision.transform.GetComponent<NetworkIdentity>());
            if (coroutineReset != null)
            {
                Debug.Log("Arrêt coroutine");
                StopCoroutine(coroutineReset);
            }
        }
    }

    [Command]
    public void CmdAssignNetworkAuthority(NetworkIdentity cubeId, NetworkIdentity clientId)
    {
        //Si -> cube a un proprio && proprio n'est pas celui qui demande autorité
        if (cubeId.clientAuthorityOwner != null && cubeId.clientAuthorityOwner != clientId.connectionToClient)
        {
            // Suppression de l'autorité
            cubeId.RemoveClientAuthority(cubeId.clientAuthorityOwner);
        }

        //Si -> cube n'a pas de proprio
        if (cubeId.clientAuthorityOwner == null)
        {
            // Ajout du demandeur comme proprio
            cubeId.AssignClientAuthority(clientId.connectionToClient);
        }
    }

    [Command]
    public void CmdRemoveAuthority(NetworkIdentity cubeId)
    {
        cubeId.RemoveClientAuthority(cubeId.clientAuthorityOwner);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player" && isServer)
        {
            lastHit = Time.time;
            coroutineReset = StartCoroutine(ResetAuthority());
        }
    }

    private IEnumerator ResetAuthority()
    {
        while (lastHit + cooldown > Time.time)
        {
            Debug.Log("Attente avant de supprimer l'authorité");
            yield return null;
        }
        Debug.Log("Suppression de l'authorité");
        CmdRemoveAuthority(GetComponent<NetworkIdentity>());
    }
}
