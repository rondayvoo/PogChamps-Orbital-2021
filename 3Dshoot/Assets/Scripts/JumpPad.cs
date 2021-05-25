using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float velocity;
    public float angle;
    public LayerMask affected;

    Vector3 angleTransform()
    {
        Vector3 newV = transform.up * Mathf.Sin(Mathf.Deg2Rad * angle) - transform.forward * Mathf.Cos(Mathf.Deg2Rad * angle);
        return newV * velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().velocity = angleTransform();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
