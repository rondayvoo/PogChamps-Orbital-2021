using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour, IDamageable
{
    public GameObject damageNumPF;

    public void takeDamage(int dmg, bool crit)
    {
        damageNumPF.GetComponent<BoxDamageText>().takeDamage(dmg, crit);
    }
}
