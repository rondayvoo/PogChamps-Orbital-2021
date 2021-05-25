using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Explosion", menuName = "Item/Abstract/Explosion")]
public class ExplosionScriptableObject : ItemScriptableObject
{
    public LayerMask expCollision;
    public float expRadius;
    public float expForce;
    public float dmgScalar;
    public float dmgMultiplier;

    public float dmgCalculation(Transform origin, Transform impact)
    {
        return dmgScalar - (origin.position - impact.position).magnitude * dmgMultiplier;
    }

    public float dmgCalculationBullet(Transform playerPos, Transform impact)
    {
        return 1 / (playerPos.position - impact.position).magnitude * dmgMultiplier;
    }
}
