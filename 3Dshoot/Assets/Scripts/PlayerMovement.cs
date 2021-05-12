using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float playerJumpForce;
    [SerializeField] float playerSpeed;
    [SerializeField] int playerHeldWeapon;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    Vector3 totalForce;
    bool spaceKey = false;

    bool grounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, 0.2f, groundLayer);

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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horiInput = Input.GetAxisRaw("Horizontal") * transform.right;
        Vector3 vertInput = Input.GetAxisRaw("Vertical") * transform.forward;
        spaceKey = Input.GetKeyDown(KeyCode.Space);
        totalForce = (horiInput + vertInput).normalized;
    }

    void FixedUpdate()
    {
        //Jumping
        if (spaceKey && grounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, playerJumpForce, rb.velocity.z);
        }

        //Walking
        if (grounded())
        {
            if (totalForce != Vector3.zero)
                rb.velocity = totalForce * playerSpeed + rb.velocity.y * Vector3.up;

            else
                rb.velocity = rb.velocity.y * Vector3.up;
        }

        //Midair Strafing + Air Resistance
        else
        {
            if (totalForce != Vector3.zero && !grounded() && Vector3.Dot((rb.velocity - rb.velocity.y * Vector3.up), totalForce) <= 1f)
            {
                rb.velocity = rb.velocity + totalForce * playerSpeed * 0.15f;
            }

            rb.velocity = rb.velocity - (rb.velocity - rb.velocity.y * Vector3.up) * Time.deltaTime * 0.01f;
        }
    }
}
