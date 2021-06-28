using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCountScript : MonoBehaviour
{
    public TMP_Text ammoC;

    public void ammoUpdate(object sender, GameEvents.OnAmmoCountChangeEventArgs ev)
    {
        ammoC.text = ev.currAmmo.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        ammoC = GetComponent<TMP_Text>();
        GameEvents.instance.OnAmmoCountChange += ammoUpdate;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnAmmoCountChange -= ammoUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
