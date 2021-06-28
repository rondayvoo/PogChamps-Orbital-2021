using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : BaseWeaponScript
{
    public int numPellets;
    public GameObject bulletImpPF;
    public float bulletSpread;
    public float range;
    [SerializeField]
    private AudioSource shootSound;

    public override void primaryFire()
    {
        Transform cam = Camera.main.transform;
        int shotsHit = 0;
        muzzleFlash.Play();
        shootSound.Play();

        for (int i = 0; i < numPellets; i++)
        {
            float radius = Random.Range(0f, bulletSpread);
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 bSpread = Mathf.Cos(angle) * cam.right * radius + Mathf.Sin(angle) * cam.up * radius;

            RaycastHit impact;
            if (Physics.Raycast(cam.position, cam.forward + bSpread, out impact, range, collisionLayer))
                shotsHit++;

            Instantiate(bulletImpPF, impact.point, Quaternion.LookRotation(impact.normal));
            IDamageable dmgObj = impact.transform.gameObject.GetComponent<IDamageable>();

            if (dmgObj != null)
            {
                if (impact.collider.tag == "CritSpot")
                    dmgObj.takeDamage(dmgCalc(impact.distance, shotsHit) * 2, true);
                else
                    dmgObj.takeDamage(dmgCalc(impact.distance, shotsHit), false);
            }
        }
    }

    public override void secondaryFire()
    {

    }

    int dmgCalc(float distance, int shotsHit)
    {
        return (int) Mathf.Clamp(dmgFalloff * distance + baseDMG / numPellets * shotsHit, baseDMG / 10f, Mathf.Infinity);
    }
}
