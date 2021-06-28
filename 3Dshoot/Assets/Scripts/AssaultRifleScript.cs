using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleScript : BaseWeaponScript
{
    public GameObject bulletImpPF;
    public float bulletSpread;
    public float range;
    //public List<WeaponInstance> modifiers;
    [SerializeField]
    private AudioSource shootSound;

    public int dmgCalc(float distance)
    {
        return (int) Mathf.Clamp(dmgFalloff * distance + baseDMG, 5f, Mathf.Infinity);
    }

    public override void primaryFire()
    {
        Transform cam = Camera.main.transform;
        float radius = Random.Range(0f, bulletSpread);
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 bSpread = Mathf.Cos(angle) * cam.right * radius + Mathf.Sin(angle) * cam.up * radius;
        muzzleFlash.Play();
        shootSound.Play();

        RaycastHit impact;

        if (Physics.Raycast(cam.position, cam.forward + bSpread, out impact, range, collisionLayer))
        {
            Instantiate(bulletImpPF, impact.point, Quaternion.LookRotation(impact.normal));
            IDamageable dmgObj = impact.transform.gameObject.GetComponent<IDamageable>();
            muzzleFlash.Play();

            if (dmgObj != null)
            {
                if (impact.collider.tag == "CritSpot")
                    dmgObj.takeDamage(dmgCalc(impact.distance) * 2, true);
                else
                    dmgObj.takeDamage(dmgCalc(impact.distance), false);
            }
        }
    }

    public override void secondaryFire()
    {

    }
}
