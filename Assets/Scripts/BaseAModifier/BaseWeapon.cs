using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BaseWeapon : NetworkBehaviour//, IWeapon
{
    //private float cooldown = GAME_CONST.WEAPON_COOLDOWN;
    //private float balDamage = -GAME_CONST.WEAPON_BAL_DAMAGE;
    //private float fireHeating = GAME_CONST.WEAPON_FIRE_HEATING;
    //private float coolingTime = GAME_CONST.WEAPON_COOLING_TIME;
    //[SerializeField] private Transform firePoint;
    //[SerializeField] private ShotEffectsManager shotEffects;
    //private float lastShot;

    //public float Cooldown { get { return cooldown; } set { cooldown = value; } }
    //public float BalDamage { get { return balDamage; } set { balDamage = value; } }
    //public float FireHeating { get { return fireHeating; } set { fireHeating = value; } }
    //public float CoolingTime { get { return coolingTime; } set { coolingTime = value; } }
    //public Transform FirePoint { get { return firePoint; } }

    //public void Initialize(UnityEvent fireEvent)
    //{
    //    lastShot = Time.time;
    //    fireEvent.AddListener(Fire);
    //}

    ///// <summary>
    ///// Appelé par l'évènement FireEvent lorsque le joueur clique
    ///// </summary>
    //public void Fire()
    //{
    //    if (!isLocalPlayer || Time.time <= lastShot + cooldown)
    //        return;

    //    RaycastHit hit;
    //    Ray ray = new Ray(firePoint.position, firePoint.forward);
    //    bool result = Physics.Raycast(ray, out hit, 500f);
    //    //if (result)
    //    //    Debug.Log("en tant que client, j'ai hit");

    //    CmdFire(firePoint.position, firePoint.forward, result, this.netId);
    //    lastShot = Time.time;
        
    //}

    //[Command]
    //public void CmdFire(Vector3 origin, Vector3 direction, bool clientHit, NetworkInstanceId id)
    //{
    //    Debug.Log(NetworkManager.singleton.client.GetRTT());
    //    //NetworkIdentity client = NetworkServer.FindLocalObject(id).GetComponent<NetworkIdentity>().clientAuthorityOwner.;
    //    //Debug.Log(client.GetRTT());

    //    RaycastHit hit;
    //    Ray ray = new Ray(origin, direction);
    //    bool result = Physics.Raycast(ray, out hit, 500f);

    //    if (result)
    //    {
    //        PlayerWeight enemy = hit.transform.GetComponent<PlayerWeight>();

    //        if (enemy != null)
    //        {
    //            enemy.TakeDamage(balDamage);
    //            Debug.Log("Damage demandé");
    //        }
    //    }

    //    RpcProcessShotEffects(result, hit.point);
    //}

    //[ClientRpc]
    //void RpcProcessShotEffects(bool playImpact, Vector3 point)
    //{
    //    shotEffects.PlayShotEffects();
    //    if (playImpact)
    //        shotEffects.PlayImpactEffect(point);
    //}
}
