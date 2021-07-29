using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    Rigidbody rb;
    //public PlayerScriptableObject playerProfile;
    //public PlayerInventoryScriptableObject inventory;
    public float playerJumpForce;
    public float playerSpeed;
    public float playerMaxAccel;
    public int playerMaxHealth;
    [HideInInspector] public int playerCurrHealth;
    [HideInInspector] public int playerLevel;
    [HideInInspector] public int playerExperience;
    public int expToLevel;
    //private const int MAX_JUMP = 2;
    //private int currentJump = 0;
    //private int jumpcounter = 0;
    //private bool canDoubleJump = false;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    RaycastHit surface;
    Vector3 totalForce;
    bool spaceKey = false;
    bool spacePush = false;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource jumpSound2;
    [SerializeField] private AudioSource footstepsSound;

    bool grounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, 0.2f, groundLayer);

        if (colliders.Length > 0)
        {
            //currentJump = 0;
            return true;
        }

        return false;
    }

    void playerForce(Vector3 wishVel, float maxAccel)
    {
        Vector3 deltaVel = wishVel - rb.velocity;
        Vector3 accel = deltaVel / Time.deltaTime;

        if (accel.sqrMagnitude > maxAccel * maxAccel)
            accel = accel.normalized * maxAccel;

        rb.AddForce(accel, ForceMode.Acceleration);
    }

    public void takeDamage(int dmg, bool crit)
    {
        playerCurrHealth -= dmg;
        GameEvents.instance.PlayerHit(playerMaxHealth, playerCurrHealth, dmg);

        if (playerCurrHealth <= 0)
            GameEvents.instance.PlayerDie();
    }

    public void addExp(object sender, GameEvents.OnEnemyKillEventArgs ev)
    {
        playerExperience += ev.enemy.GetComponent<BaseEnemyScript>().enemyExperience;
        playerLevel = playerExperience / expToLevel + 1;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCurrHealth = playerMaxHealth;
        playerLevel = 1;
        playerExperience = 0;

        rb = GetComponent<Rigidbody>();
        GameEvents.instance.PlayerStart(gameObject);
        GameEvents.instance.OnEnemyKill += addExp;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnEnemyKill -= addExp;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horiInput = Input.GetAxisRaw("Horizontal") * transform.right;
        Vector3 vertInput = Input.GetAxisRaw("Vertical") * transform.forward;
        spaceKey = Input.GetKey(KeyCode.Space);
        spacePush = Input.GetKeyDown(KeyCode.Space);

        totalForce = (horiInput + vertInput).normalized;

        //float speedMod = 0f;
        //float jumpMod = 0f;

        //foreach (PickupScriptableObject pickup in inventory.inventoryPickupList)
        //{
        //    if (pickup)
        //    {
        //        speedMod += pickup.charSpeedMod;
        //        jumpMod += pickup.charJumpMod;
        //    }
        //}
        //
        //playerSpeed = playerProfile.charSpeed + speedMod;
        //playerJumpForce = playerProfile.playerJumpForce + jumpMod;
    }

    void FixedUpdate()
    {
        //Jumping
        if (spaceKey && grounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, playerJumpForce, rb.velocity.z);
            //var myRandomIndex = 0;
            //myRandomIndex = Random.Range(0, 1);
            //if (myRandomIndex == 0)
            //{
            jumpSound2.Play();
            //}
        }

        //Walking
        else if (grounded() && rb.velocity.y <= playerJumpForce - 2f)
        {
            if (totalForce != Vector3.zero && !footstepsSound.isPlaying)
                footstepsSound.Play();

            Physics.Raycast(groundChecker.position, -Vector3.up, out surface, groundLayer);
            Vector3 adjustedMv;
            float normalAngle = Vector3.Angle(surface.normal, Vector3.up);

            if (Vector3.ProjectOnPlane(totalForce, surface.normal).y > 0)
                adjustedMv = totalForce;
            else
                adjustedMv = Vector3.ProjectOnPlane(totalForce, surface.normal);

            playerForce(adjustedMv * playerSpeed, playerMaxAccel);
        }

        //Midair Strafing
        else
        {
            if (!grounded() && Vector3.Dot((rb.velocity - rb.velocity.y * Vector3.up), totalForce) <= 0f)
            {
                rb.AddForce(totalForce * playerMaxAccel, ForceMode.Acceleration);
                //rb.velocity = rb.velocity + totalForce * playerSpeed * 0.2f;
            }

            //if (spacePush && currentJump < MAX_JUMP - 1)
            //{
            //    rb.velocity = new Vector3(rb.velocity.x, playerJumpForce, rb.velocity.z);
            //    currentJump++;
            //}
        }
    }
}
