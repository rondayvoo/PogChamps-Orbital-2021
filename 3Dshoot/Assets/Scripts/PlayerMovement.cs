using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public PlayerScriptableObject playerProfile;
    public PlayerInventoryScriptableObject inventory;
    private float playerJumpForce;
    private float playerSpeed;
    private int playerCurrHealth;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    RaycastHit surface;
    Vector3 totalForce;
    bool spaceKey = false;
    bool dashKey = false;
    HealthBarScript healthbar;

    bool grounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, 0.1f, groundLayer);
    
        if (colliders.Length > 0)
        {
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

    public void takeDamage(int dmg)
    {
        playerCurrHealth -= dmg;
        healthbar.healthUpdate(playerCurrHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Remove this when trying to implement saving
        inventory.inventoryPickupList = new List<PickupScriptableObject>();

        playerCurrHealth = playerProfile.charHealth;
        playerSpeed = playerProfile.charSpeed;
        playerJumpForce = playerProfile.playerJumpForce;

        rb = GetComponent<Rigidbody>();
        healthbar = GetComponentInChildren<HealthBarScript>();
        healthbar.setMax(playerProfile.charHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horiInput = Input.GetAxisRaw("Horizontal") * transform.right;
        Vector3 vertInput = Input.GetAxisRaw("Vertical") * transform.forward;
        spaceKey = Input.GetKey(KeyCode.Space);
        dashKey = Input.GetKeyDown(KeyCode.LeftShift);
        totalForce = (horiInput + vertInput).normalized;

        float speedMod = 0f;
        float jumpMod = 0f;

        foreach (PickupScriptableObject pickup in inventory.inventoryPickupList)
        {
            if (pickup)
            {
                speedMod += pickup.charSpeedMod;
                jumpMod += pickup.charJumpMod;
            }
        }

        playerSpeed = playerProfile.charSpeed + speedMod;
        playerJumpForce = playerProfile.playerJumpForce + jumpMod;
    }

    void FixedUpdate()
    {
        //Jumping
        if (spaceKey && grounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, playerJumpForce, rb.velocity.z);
        }

        //Walking
        else if (grounded() && rb.velocity.y <= playerJumpForce - 2f)
        {
            Physics.Raycast(groundChecker.position, -Vector3.up, out surface, groundLayer);
            Vector3 adjustedMv;
            float normalAngle = Vector3.Angle(surface.normal, Vector3.up);

            if (Vector3.ProjectOnPlane(totalForce, surface.normal).y > 0)
                adjustedMv = totalForce * (1 / Mathf.Cos(normalAngle));
            else
                adjustedMv = Vector3.ProjectOnPlane(totalForce, surface.normal);

            //if (totalForce == Vector3.zero && normalAngle != 0f)
            //    rb.velocity = Vector3.zero;
            //if (normalAngle == 0f)
            //    rb.velocity = playerSpeed * totalForce;
            //else
            //    rb.velocity = playerSpeed * adjustedMv + jumpVelocity;

            playerForce(adjustedMv * playerSpeed, playerProfile.playerMaxAccel);
            //rb.velocity = playerSpeed * adjustedMv;// + rb.velocity.y * Vector3.up;
            //rb.velocity = (playerSpeed * adjustedMv + rb.velocity.y * Vector3.up) * Time.deltaTime * 100f;
        }

        //Midair Strafing
        else
        {
            if (!grounded() && Vector3.Dot((rb.velocity - rb.velocity.y * Vector3.up), totalForce) <= 0f)
            {
                rb.AddForce(totalForce * playerProfile.playerMaxAccel, ForceMode.Acceleration);
                //rb.velocity = rb.velocity + totalForce * playerSpeed * 0.2f;
            }
        }
    }
}
