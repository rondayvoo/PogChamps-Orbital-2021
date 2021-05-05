using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    [SerializeField] float expForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject damageNumPF;
    Material mat;
    float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] blastzone = Physics.OverlapSphere(transform.position, 0.05f, groundLayer);
        Collider[] blowback = Physics.OverlapSphere(transform.position, 0.05f, playerLayer);
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
            }
        }

        foreach (Collider plr in blowback)
        {
            Rigidbody blowbackRB = plr.GetComponent<Rigidbody>();

            if (blowbackRB)
                blowbackRB.AddExplosionForce(expForce, transform.position, 0.6f, 0.1f, ForceMode.Impulse);
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
