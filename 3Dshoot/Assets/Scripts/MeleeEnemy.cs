using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemyScript
{
    public int meleeDamage;
    public float attackRange;
    public float attackSpeed;
    private float attackTimer;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Rest");
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] pCheck = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (pCheck.Length > 0 && attackTimer <= 0f)
        {
            animator.SetTrigger("Attack_1");
            agent.speed = 0f;
            pCheck[0].GetComponent<PlayerMovement>().takeDamage(meleeDamage, false);
            attackTimer = attackSpeed;
        }

        else if (stunTimer <= 0f && grounded() && Vector3.Distance(Camera.main.transform.position, transform.position) <= lookRadius && pCheck.Length <= 0)
        {
            agent.speed = enemySpeed;
            agent.enabled = true;
            rb.isKinematic = true;
            agent.SetDestination(Camera.main.transform.position);
            animator.SetTrigger("Walk_Cycle_1");
        }

        else
        {
            agent.enabled = false;
            rb.isKinematic = false;
            //animator.SetTrigger("Rest");
            //rb.velocity = (rb.velocity - rb.velocity.y * Vector3.up) * 0.97f + rb.velocity.y * Vector3.up;
        }

        stunTimer = stunTimer <= 0f ? 0f : stunTimer - Time.deltaTime;
        attackTimer = attackTimer <= 0f ? 0f : attackTimer - Time.deltaTime;
    }


}
