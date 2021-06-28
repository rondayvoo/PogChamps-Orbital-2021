using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour, IDamageable, ISaveable
{
    protected Rigidbody rb;
    public int enemyMaxHealth;
    protected int enemyCurrHealth;
    public float enemySpeed;
    public float stunTime;
    public float lookRadius;
    [SerializeField] EnemyHealthBarScript healthbar;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject damageNumPF;
    protected NavMeshAgent agent;
    protected float stunTimer = 0f;

    public void takeDamage(int dmg, bool crit)
    {
        enemyCurrHealth -= dmg;
        healthbar.healthUpdate(enemyCurrHealth);
        stunTimer = stunTime;
        agent.enabled = false;
        rb.isKinematic = false;
        //damageNumPF.SetActive(true);
        damageNumPF.GetComponent<BoxDamageText>().takeDamage(dmg, crit);

        if (enemyCurrHealth <= 0)
        {
            GameEvents.instance.EnemyKill(gameObject);
            gameObject.SetActive(false);
        }
    }

    public bool grounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, 0.4f, groundLayer);

        if (colliders.Length > 0)
        {
            return true;
        }

        return false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyCurrHealth = enemyMaxHealth;
        healthbar.setMax(enemyMaxHealth);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public object captureState()
    {
        rb = GetComponent<Rigidbody>();
        EnemySaveData esd = new EnemySaveData();

        esd.position = new float[3];
        esd.position[0] = rb.position.x;
        esd.position[1] = rb.position.y;
        esd.position[2] = rb.position.z;

        esd.rotation = new float[4];
        esd.rotation[0] = rb.rotation.x;
        esd.rotation[1] = rb.rotation.y;
        esd.rotation[2] = rb.rotation.z;
        esd.rotation[3] = rb.rotation.w;

        esd.velocity = new float[3];
        esd.velocity[0] = rb.velocity.x;
        esd.velocity[1] = rb.velocity.y;
        esd.velocity[2] = rb.velocity.z;

        esd.currHealth = enemyCurrHealth;

        return esd;
    }

    public void restoreState(object state)
    {
        rb = GetComponent<Rigidbody>();
        EnemySaveData esd = (EnemySaveData) state;

        rb.position = new Vector3(esd.position[0], esd.position[1], esd.position[2]);
        rb.rotation = new Quaternion(esd.rotation[0], esd.rotation[1], esd.rotation[2], esd.rotation[3]);
        rb.velocity = new Vector3(esd.velocity[0], esd.velocity[1], esd.velocity[2]);
        enemyCurrHealth = esd.currHealth;

        if (enemyCurrHealth <= 0)
            gameObject.SetActive(false);
    }
}

[System.Serializable]
public class EnemySaveData
{
    public float[] position;
    public float[] rotation;
    public float[] velocity;
    public int currHealth;
}
