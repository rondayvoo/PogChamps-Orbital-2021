using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public WeaponScriptableObject weaponInst;

    // Start is called before the first frame update
    void Start()
    {
        weaponInst = Instantiate(weaponInst);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
