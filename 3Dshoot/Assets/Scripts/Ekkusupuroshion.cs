using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ekkusupuroshion : MonoBehaviour
{
    [HideInInspector] public ExplosionScriptableObject baseExp;
    [SerializeField] GameObject damageNumPF;
    Material mat;
    float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] blastzone = Physics.OverlapSphere(transform.position, baseExp.expRadius, baseExp.expCollision);
        mat = GetComponent<Renderer>().material;

        foreach (Collider blast in blastzone)
        {
            Rigidbody blastRB = blast.GetComponent<Rigidbody>();

            if (blastRB)
            {
                int dmgTaken = (int) (baseExp.dmgMultiplier + Random.Range(-baseExp.dmgScalar, baseExp.dmgScalar));
                GameObject numInst = Instantiate(damageNumPF, blastRB.transform.position + new Vector3(0f, 1f, 0f), transform.rotation);

                if (numInst)
                    numInst.GetComponent<BoxDamageText>().dmgUpdate(dmgTaken);

                if (blastRB.GetComponent<EnemyMovement>())
                {
                    blastRB.GetComponent<EnemyMovement>().takeDamage(dmgTaken);
                }

                if (blastRB.GetComponent<PlayerMovement>())
                {
                    blastRB.GetComponent<PlayerMovement>().takeDamage(dmgTaken);
                }

                blastRB.AddExplosionForce(baseExp.expForce, transform.position, baseExp.expRadius, 0f, ForceMode.Impulse);
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
