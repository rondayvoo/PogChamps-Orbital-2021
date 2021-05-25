using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody rb;
    public EnemyScriptableObject enemyProfile;
    private int enemyCurrHealth;
    [SerializeField] HealthBarScript healthbar;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    NavMeshAgent agent;
    float stunTimer = 0f;

    public void takeDamage(int dmg)
    {
        enemyCurrHealth -= dmg;
        healthbar.healthUpdate(enemyCurrHealth);
        stunTimer = enemyProfile.stunTime;
        agent.enabled = false;
        rb.isKinematic = false;

        if (enemyCurrHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    bool grounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, 0.5f, groundLayer);

        if (colliders.Length > 0)
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyCurrHealth = enemyProfile.charHealth;
        healthbar.setMax(enemyProfile.charHealth);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyProfile.charSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTimer <= 0f && grounded())
        {
            agent.enabled = true;
            rb.isKinematic = true;
            agent.SetDestination(Camera.main.transform.position);
        }

        else
        {
            agent.enabled = false;
            rb.isKinematic = false;
            //rb.velocity = (rb.velocity - rb.velocity.y * Vector3.up) * 0.97f + rb.velocity.y * Vector3.up;
            stunTimer -= Time.deltaTime;
        }

        //transform.LookAt(Camera.main.transform.position - Camera.main.transform.position.y * Vector3.up + transform.position.y * Vector3.up);
    }
}
