using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BaseMelee : NetworkBehaviour
{
    private float cooldown = 1.0f;//GAME_CONST.MELEE_COOLDOWN;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ShotEffectsManager shotEffects;
    private float lastShot;

    public float Cooldown { get { return cooldown; } set { cooldown = value; } }
    public Transform FirePoint { get { return firePoint; } }

    public void Initialize(UnityEvent meleeEvent)
    {
        lastShot = Time.time;
        meleeEvent.AddListener(MeleeAttack);
    }

    public void MeleeAttack()
    {
        if (!isLocalPlayer || Time.time <= lastShot + cooldown)
            return;

        CmdMeleeAttack(firePoint.position, firePoint.forward);
        lastShot = Time.time;
    }

    [Command]
    public void CmdMeleeAttack(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        Ray ray = new Ray(origin, direction);

        bool result = Physics.Raycast(origin, direction, out hit, 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 10);

        if (result)
        {
            //Player enemy = hit.transform.GetComponent<Player>();

            //if (enemy != null)
            //{
            //    enemy.CmdMeleeHit(transform.position, hit.point);
            //    Debug.Log("Melee demandé");
            //}
        }

        RpcProcessShotEffects(result, hit.point);
    }

    [ClientRpc]
    void RpcProcessShotEffects(bool playImpact, Vector3 point)
    {
        shotEffects.PlayShotEffects();

        if (playImpact)
            shotEffects.PlayImpactEffect(point);
    }
}
