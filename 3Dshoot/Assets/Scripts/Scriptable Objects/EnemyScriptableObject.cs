using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Profile", menuName = "Character/Enemy")]
public class EnemyScriptableObject : CharacterScriptableObject
{
    public WeaponScriptableObject heldWeapon;
    public float stunTime;

    private float currStunTime;

    public override void takeDamage(int damage)
    {
        charHealth -= damage;
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
