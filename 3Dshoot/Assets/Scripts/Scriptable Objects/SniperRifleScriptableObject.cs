using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon/Sniper Rifle")]
public class SniperRifleScriptableObject : WeaponScriptableObject
{
    public GameObject bulletImpPF;
    public ExplosionScriptableObject baseExp;

    public override void primaryFire(Transform cam)
    {
        RaycastHit impact;

        if (Physics.Raycast(cam.position, cam.forward, out impact, collisionLayer))
        {
            GameObject exp = Instantiate(bulletImpPF, impact.point, Quaternion.LookRotation(impact.normal));
            exp.GetComponent<Ekkusupuroshion>().baseExp = baseExp;
        }
    }
}
