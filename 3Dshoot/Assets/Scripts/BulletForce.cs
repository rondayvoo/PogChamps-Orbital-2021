using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    [SerializeField] float expForce;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] GameObject damageNumPF;
    Material mat;
    float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] blastzone = Physics.OverlapSphere(transform.position, 0.05f, collisionLayer);
        mat = GetComponent<Renderer>().material;

        foreach (Collider blast in blastzone)
        {
            Rigidbody blastRB = blast.GetComponent<Rigidbody>();

            if (blastRB)
            {
                blastRB.AddExplosionForce(expForce, transform.position, 0.05f, 0.1f, ForceMode.Impulse);
                GameObject numInst = Instantiate(damageNumPF, blastRB.transform.position + new Vector3(0f, 1f, 0f), transform.rotation);

                if (numInst)
                    numInst.GetComponent<BoxDamageText>().dmgUpdate((int)(1 / (Camera.main.transform.position - blastRB.transform.position).magnitude * 50));

                if (blastRB.GetComponent<EnemyMovement>())
                {
                    blastRB.GetComponent<EnemyMovement>().takeDamage((int)(1 / (Camera.main.transform.position - blastRB.transform.position).magnitude * 50));
                }
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
