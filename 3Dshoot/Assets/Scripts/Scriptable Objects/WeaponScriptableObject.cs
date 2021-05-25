using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum wElement
{
    none,
    fire,
    lightning,
    acid
}

public abstract class WeaponScriptableObject : ItemScriptableObject
{
    public int weaponLevel;
    public float fireDelay;
    public float reloadTime;
    public int clipSize;
    public int ammoCap;
    public wElement element;
    public LayerMask collisionLayer;
    public bool drawBulletTrail;
    public ParticleSystem bulletTrail;

    public abstract void primaryFire(Transform cam);

    public void drawTrail(Transform firepoint, Transform hit)
    {
        Instantiate(bulletTrail, firepoint.position, firepoint.rotation);
    }
}
