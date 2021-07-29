using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemyScript
{
    public GameObject projectilePF;
    public Transform firepoint;
    public float attackRange;
    public float attackSpeed;
    private float attackTimer;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //animator.SetTrigger("Rest");
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] pCheck = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (pCheck.Length > 0 && attackTimer <= 0f)
        {
            animator.SetTrigger("Atack_0");

            agent.speed = 0f;
            Instantiate(projectilePF, firepoint.position, Quaternion.LookRotation(pCheck[0].transform.position - firepoint.position));
            attackTimer = attackSpeed;
        }

        else if (stunTimer <= 0f && grounded() && Camera.main && Vector3.Distance(Camera.main.transform.position, transform.position) <= lookRadius && pCheck.Length <= 0)
        {
            if (animator)
                animator.SetTrigger("Run");

            agent.speed = enemySpeed;
            agent.enabled = true;
            rb.isKinematic = true;
            agent.SetDestination(Camera.main.transform.position);
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
