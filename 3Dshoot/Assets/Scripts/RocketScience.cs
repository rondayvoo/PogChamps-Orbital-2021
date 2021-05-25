using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScience : MonoBehaviour
{
    Rigidbody rb;
    float airtime = 0.0f;
    [HideInInspector] public ProjectileScriptableObject baseProj;
    [HideInInspector] public ExplosionScriptableObject baseExp;
    [SerializeField] GameObject explosionPF;

    bool shouldExplode()
    {
        Collider[] colliders = Physics.OverlapSphere(rb.position, 0.2f, baseProj.projCollision);

        if (colliders.Length > 0 || airtime > baseProj.projMaxAirtime)
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * baseProj.projSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldExplode())
        {
            GameObject exp = Instantiate(explosionPF, rb.position, rb.rotation);
            exp.GetComponent<Ekkusupuroshion>().baseExp = baseExp;
            Destroy(this.gameObject);
        }

        airtime += Time.deltaTime;
    }
}
