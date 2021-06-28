using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamageable, ISaveable
{
    Rigidbody rb;
    //public PlayerScriptableObject playerProfile;
    //public PlayerInventoryScriptableObject inventory;
    public float playerJumpForce;
    public float playerSpeed;
    public float playerMaxAccel;
    public int playerMaxHealth;
    private int playerCurrHealth;
    private const int MAX_JUMP = 2;
    private int currentJump = 0;
    //private int jumpcounter = 0;
    //private bool canDoubleJump = false;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    RaycastHit surface;
    Vector3 totalForce;
    bool spaceKey = false;
    bool spacePush = false;
    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private AudioSource jumpSound2;
    [SerializeField]
    private AudioSource footstepsSound;

    bool grounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, 0.2f, groundLayer);
    
        if (colliders.Length > 0)
        {
            currentJump = 0;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //Remove this when trying to implement saving
        //inventory.inventoryPickupList = new List<PickupScriptableObject>();

        playerCurrHealth = playerMaxHealth;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horiInput = Input.GetAxisRaw("Horizontal") * transform.right;
        Vector3 vertInput = Input.GetAxisRaw("Vertical") * transform.forward;
        spaceKey = Input.GetKey(KeyCode.Space);
        spacePush = Input.GetKeyDown(KeyCode.Space);

        totalForce = (horiInput + vertInput).normalized;

        float speedMod = 0f;
        float jumpMod = 0f;

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

    public object captureState()
    {
        rb = GetComponent<Rigidbody>();
        PlayerSaveData psd = new PlayerSaveData();

        psd.position = new float[3];
        psd.position[0] = rb.position.x;
        psd.position[1] = rb.position.y;
        psd.position[2] = rb.position.z;

        psd.rotation = new float[4];
        psd.rotation[0] = rb.rotation.x;
        psd.rotation[1] = rb.rotation.y;
        psd.rotation[2] = rb.rotation.z;
        psd.rotation[3] = rb.rotation.w;

        psd.velocity = new float[3];
        psd.velocity[0] = rb.velocity.x;
        psd.velocity[1] = rb.velocity.y;
        psd.velocity[2] = rb.velocity.z;

        return psd;
    }

    public void restoreState(object state)
    {
        rb = GetComponent<Rigidbody>();
        PlayerSaveData psd = (PlayerSaveData)state;

        rb.position = new Vector3(psd.position[0], psd.position[1], psd.position[2]);
        rb.rotation = new Quaternion(psd.rotation[0], psd.rotation[1], psd.rotation[2], psd.rotation[3]);
        rb.velocity = new Vector3(psd.velocity[0], psd.velocity[1], psd.velocity[2]);
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public float[] position;
    public float[] rotation;
    public float[] velocity;
    //public List<object> inventory;
}
