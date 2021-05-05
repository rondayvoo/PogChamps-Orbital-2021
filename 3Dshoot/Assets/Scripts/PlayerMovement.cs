using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] float jumpForce;
    Vector3 totalForce = Vector3.zero;
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
        Vector3 horiInput = Input.GetAxis("Horizontal") * transform.right;
        Vector3 vertInput = Input.GetAxis("Vertical") * transform.forward;
        spaceKey = Input.GetKeyDown(KeyCode.Space);
        totalForce = (horiInput + vertInput).normalized;

        //Jumping
        if (spaceKey && grounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //Walking
        if (grounded())
        {
            if (totalForce != Vector3.zero)
                rb.velocity = totalForce * speed + rb.velocity.y * Vector3.up;

            else
                rb.velocity = rb.velocity - (rb.velocity - rb.velocity.y * Vector3.up) * Time.deltaTime * 50.0f;
        }

        //Midair Strafing + Air Resistance
        else
        {
            if (totalForce != Vector3.zero && !grounded() && Vector3.Dot((rb.velocity - rb.velocity.y * Vector3.up), totalForce) <= 1f)
            {
                rb.velocity = rb.velocity + totalForce * speed * 0.05f;
            }

            rb.velocity = rb.velocity - (rb.velocity - rb.velocity.y * Vector3.up) * Time.deltaTime * 0.01f;
        }
    }
}
