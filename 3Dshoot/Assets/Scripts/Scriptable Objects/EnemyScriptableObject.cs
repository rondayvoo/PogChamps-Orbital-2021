using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Profile", menuName = "Item/Character/Enemy")]
public class EnemyScriptableObject : CharacterScriptableObject
{
    public WeaponScriptableObject heldWeapon;
    public float stunTime;

    public override void takeDamage(int currentHealth, int damage)
    {
        currentHealth -= damage;
        //healthbar.healthUpdate(health);
        //stunTimer = 1f;
        //agent.enabled = false;
        //rb.isKinematic = false;
        //
        //if (health <= 0)
        //{
        //    Destroy(this.gameObject);
        //}
    }
}

[System.Serializable]
public class Enemy
{
    public EnemyScriptableObject enemy;

    public Enemy(EnemyScriptableObject blueprint)
    {
        enemy = blueprint;
    }
}
