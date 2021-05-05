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

    public void dmgUpdate(int dmg)
    {
        damageTaken = dmg;
    }

    // Start is called before the first frame update
    void Start()
    {
        tMesh = GetComponent<TextMeshPro>();
        pTransform = Camera.main.transform;
        tColor = tMesh.color;
    }

    // Update is called once per frame
    void Update()
    {
        tMesh.text = damageTaken.ToString();
        transform.LookAt(2 * transform.position - pTransform.position);
        transform.position += new Vector3(0f, Time.deltaTime, 0f);
        vanishTime += 0.01f * Time.deltaTime;
        tColor.a -= vanishTime;
        tMesh.color = tColor;

        if (vanishTime > 1f)
        {
            Destroy(gameObject);
        }
    }
}
