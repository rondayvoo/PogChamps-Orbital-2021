using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ekkusupuroshion : MonoBehaviour
{
    [SerializeField] float expForce;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] GameObject damageNumPF;
    Material mat;
    float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] blastzone = Physics.OverlapSphere(transform.position, 0.8f, collisionLayer);
        mat = GetComponent<Renderer>().material;

        foreach (Collider blast in blastzone)
        {
            Rigidbody blastRB = blast.GetComponent<Rigidbody>();

            if (blastRB)
            {
                GameObject numInst = Instantiate(damageNumPF, blastRB.transform.position + new Vector3(0f, 1f, 0f), transform.rotation);

                if (numInst)
                    numInst.GetComponent<BoxDamageText>().dmgUpdate((int)((1.6f - (transform.position - blastRB.transform.position).magnitude) * 96f));

                if (blastRB.GetComponent<EnemyMovement>())
                {
                    blastRB.GetComponent<EnemyMovement>().takeDamage((int)((1.6f - (transform.position - blastRB.transform.position).magnitude) * 96f));
                }

                blastRB.AddExplosionForce(expForce, transform.position, 0.8f, 0f, ForceMode.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mat.color.a < 0)
        {
            Destroy(this.gameObject);
        }

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.6f - timeElapsed * 4f);
        timeElapsed += Time.deltaTime;
    }
}
