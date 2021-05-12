using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScience : MonoBehaviour
{
    Rigidbody rb;
    float airtime = 0.0f;
    [SerializeField] float speed;
    [SerializeField] GameObject explosionPF;
    [SerializeField] LayerMask collisionLayer;

    bool shouldExplode()
    {
        Collider[] colliders = Physics.OverlapSphere(rb.position, 0.2f, collisionLayer);

        if (colliders.Length > 0 || airtime > 5.0f)
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldExplode())
        {
            Instantiate(explosionPF, rb.position, rb.rotation);
            Destroy(this.gameObject);
        }

        airtime += Time.deltaTime;
    }
}
