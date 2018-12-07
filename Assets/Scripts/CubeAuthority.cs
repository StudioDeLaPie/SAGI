using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeAuthority : NetworkBehaviour {


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Player" && isServer)
        {
            CmdAssignNetworkAuthority(GetComponent<NetworkIdentity>(), collision.transform.GetComponent<NetworkIdentity>());
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
}
