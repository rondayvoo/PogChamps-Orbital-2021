using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxDamageText : MonoBehaviour
{
    private TextMeshPro tMesh;
    private Color tColor;
    private Transform pTransform;
    private float vanishTime = 0f;
    int damageTaken = 0;
    bool critTaken = false;

    public void takeDamage(int dmg, bool crit)
    {
        damageTaken += dmg;
        critTaken = crit;
        vanishTime = 0f;
        tColor.a = 1.0f;
        transform.localPosition = Vector3.up * 1.6f;
    }

    // Start is called before the first frame update
    void Start()
    {
        tMesh = GetComponent<TextMeshPro>();
        pTransform = Camera.main.transform;
        tColor = tMesh.color;
        tColor.a = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (critTaken)
        {
            tMesh.text = damageTaken.ToString() + "!";
            tMesh.color = Color.red;
        }
        else
            tMesh.text = damageTaken.ToString();

        transform.LookAt(2 * transform.position - pTransform.position);
        transform.position += Time.deltaTime * Vector3.up;
        vanishTime += 0.1f * Time.deltaTime;
        tColor.a -= vanishTime;

        if (damageTaken == 0)
        {
            tColor.a = 0f;
        }

        if (vanishTime >= 0.1f)
        {
            vanishTime = 0.1f;
            damageTaken = 0;
            transform.localPosition = Vector3.zero;
        }

        tMesh.color = tColor;
    }
}
