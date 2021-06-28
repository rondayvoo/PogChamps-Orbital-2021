using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public float suckRadius;
    public float suckForce;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<PlayerMovement>())
            GameEvents.instance.StageClear();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(50f * Time.deltaTime, 100f * Time.deltaTime, 0f);

        Collider[] succ = Physics.OverlapSphere(transform.position, suckRadius);

        foreach (Collider collider in succ)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(-suckForce, transform.position, suckRadius);
            }
        }
    }
}
