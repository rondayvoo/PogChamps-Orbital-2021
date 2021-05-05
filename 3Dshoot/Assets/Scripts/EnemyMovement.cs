using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = ((Camera.main.transform.position.x * Vector3.right + Camera.main.transform.position.z * Vector3.forward) - 
                        (transform.position.x * Vector3.right + transform.position.z * Vector3.forward)).normalized * speed
                        + rb.velocity.y * Vector3.up;
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}
