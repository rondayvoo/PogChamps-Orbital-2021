using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScience : MonoBehaviour
{
    Rigidbody rb;
    float airtime = 0.0f;

    public float projRadius;
    public float projSpeed;
    public float projMaxAirtime;
    public LayerMask projCollision;
    public GameObject explosionPF;

    bool shouldExplode()
    {
        Collider[] colliders = Physics.OverlapSphere(rb.position, projRadius, projCollision);

        if (colliders.Length > 0 || airtime > projMaxAirtime)
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * projSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(rb.position, projRadius, projCollision);

        if (colliders.Length > 0 || airtime > projMaxAirtime)
        {
            Instantiate(explosionPF, rb.position, rb.rotation);
            Destroy(gameObject);
        }

        airtime += Time.deltaTime;
    }
}
