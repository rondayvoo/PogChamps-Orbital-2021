using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Profile", menuName = "Item/Weapon/Assault Rifle")]
public class AssaultRifleScriptableObject : WeaponScriptableObject
{
    public GameObject bulletImpPF;
    public ExplosionScriptableObject baseExp;
    public float bulletSpread;
    public float range;

    public override void primaryFire(Transform cam)
    {
        float radius = Random.Range(0f, bulletSpread);
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 bSpread = Mathf.Cos(angle) * cam.right * radius + Mathf.Sin(angle) * cam.up * radius;

        RaycastHit impact;

        if (Physics.Raycast(cam.position, cam.forward + bSpread, out impact, range, collisionLayer))
        {
            GameObject exp = Instantiate(bulletImpPF, impact.point, Quaternion.LookRotation(impact.normal));
            exp.GetComponent<Ekkusupuroshion>().baseExp = baseExp;
        }
    }
}
