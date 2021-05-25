using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponInstance : ScriptableObject
{
    public int weaponLevel;
    public float fireDelay;
    public float reloadTime;
    public wElement element;
    //public Transform firepoint;
    public LayerMask collisionLayer;
}

public class AssaultRifleInstance : WeaponInstance
{
    public GameObject bulletImpPF;
    public float bulletSpread;
    public float range;

    public AssaultRifleInstance(AssaultRifleScriptableObject blueprint)
    {
        weaponLevel = blueprint.weaponLevel;
        fireDelay = blueprint.fireDelay;
        reloadTime = blueprint.reloadTime;
        element = blueprint.element;
        //firepoint = blueprint.firepoint;
        collisionLayer = blueprint.collisionLayer;
        bulletImpPF = blueprint.bulletImpPF;
        bulletSpread = blueprint.bulletSpread;
        range = blueprint.range;
    }
}

public class RocketLauncherInstance : WeaponInstance
{
    public GameObject rocketPF;

    public RocketLauncherInstance(RocketLauncherScriptableObject blueprint)
    {
        weaponLevel = blueprint.weaponLevel;
        fireDelay = blueprint.fireDelay;
        reloadTime = blueprint.reloadTime;
        element = blueprint.element;
        //firepoint = blueprint.firepoint;
        collisionLayer = blueprint.collisionLayer;
        rocketPF = blueprint.rocketPF;
    }
}

public class SniperRifleInstance : WeaponInstance
{
    public GameObject bulletImpPF;
    public Transform firepoint;

    public SniperRifleInstance(SniperRifleScriptableObject blueprint)
    {
        weaponLevel = blueprint.weaponLevel;
        fireDelay = blueprint.fireDelay;
        reloadTime = blueprint.reloadTime;
        element = blueprint.element;
        //firepoint = blueprint.firepoint;
        collisionLayer = blueprint.collisionLayer;
        bulletImpPF = blueprint.bulletImpPF;
    }
}
