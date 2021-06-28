using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRifleScript : BaseWeaponScript
{
    public GameObject bulletImpPF;
    public float bulletSpread;
    public float range;
    public float chainRadius;
    [SerializeField]
    private AudioSource shootSound;
    //public List<WeaponInstance> modifiers;
    LineRenderer lr;

    public void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        lr.startColor = lr.startColor.a > 0f ? lr.startColor -= new Color(0f, 0f, 0f, 0.1f) : lr.startColor;
        lr.endColor = lr.startColor;
    }

    public int dmgCalc(float distance)
    {
        return (int)Mathf.Clamp(dmgFalloff * distance + baseDMG, baseDMG / 10f, Mathf.Infinity);
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
            int lrIndex = 2;
            lr.positionCount = lrIndex;

            lr.startColor = lr.startColor + new Color(0f, 0f, 0f, 1f);
            lr.endColor = lr.startColor;

            lr.enabled = true;
            lr.SetPosition(0, firepoint.position);
            lr.SetPosition(1, impact.point);

            Instantiate(bulletImpPF, impact.point, Quaternion.LookRotation(impact.normal));
            IDamageable dmgObj = impact.transform.gameObject.GetComponent<IDamageable>();

            if (dmgObj != null)
            {
                if (impact.collider.tag == "CritSpot")
                    dmgObj.takeDamage(dmgCalc(impact.distance) * 2, true);
                else
                    dmgObj.takeDamage(dmgCalc(impact.distance), false);

                Collider[] colliders = Physics.OverlapSphere(impact.point, chainRadius, collisionLayer);

                foreach (Collider thing in colliders)
                {
                    IDamageable cDmgObj = thing.GetComponent<IDamageable>();

                    if (cDmgObj != null && cDmgObj != dmgObj)
                    {
                        lr.positionCount++;
                        cDmgObj.takeDamage(dmgCalc((thing.transform.position - transform.position).magnitude), false);
                        lr.SetPosition(lrIndex, thing.transform.position);
                        lrIndex++;
                    }
                }
            }
        }
    }

    public override void secondaryFire()
    {
        shootSound.Play();
    }
}
