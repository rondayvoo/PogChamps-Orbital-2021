using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ekkusupuroshion : MonoBehaviour
{
    [HideInInspector] public ExplosionScriptableObject baseExp;
    [SerializeField] GameObject damageNumPF;
    float timeElapsed = 0f;

    public LayerMask expCollision;
    public float splashDmg;
    public bool hasSplashDmg;
    public float expRadius;
    public float expForce;

    int dmgCalc(Vector3 closestPoint)
    {
        return (int) ((expRadius - Vector3.Distance(closestPoint, transform.position)) / expRadius * splashDmg);
    }

    // Start is called before the first frame update
    void Start()
    {
        Collider[] blastzone = Physics.OverlapSphere(transform.position, expRadius, expCollision);

        foreach (Collider blast in blastzone)
        {
            Rigidbody blastRB = blast.GetComponent<Rigidbody>();

            if (blastRB)
            {
                if (hasSplashDmg)
                {
                    IDamageable dmgObj = blastRB.GetComponent<IDamageable>();

                    if (dmgObj != null)
                        dmgObj.takeDamage(dmgCalc(blast.ClosestPoint(transform.position)), false);
                }

                blastRB.AddExplosionForce(expForce, transform.position, expRadius, 0f, ForceMode.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.6f - timeElapsed * 4f);
        timeElapsed += Time.deltaTime;
    }
}
