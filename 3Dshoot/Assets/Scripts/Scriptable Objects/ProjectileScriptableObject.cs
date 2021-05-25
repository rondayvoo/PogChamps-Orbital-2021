using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Abstract/Projectile")]
public class ProjectileScriptableObject : ItemScriptableObject
{
    public float projSpeed;
    public float projMaxAirtime;
    public LayerMask projCollision;
}
