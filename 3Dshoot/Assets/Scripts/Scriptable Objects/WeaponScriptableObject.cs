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

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public abstract class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;
    public int weaponLevel;
    public string weaponDescription;
    public float fireDelay;
    public float reloadTime;
    public wElement element;
    //public Transform firepoint;
    public LayerMask collisionLayer;

    public abstract void primaryFire(Transform cam);
}
