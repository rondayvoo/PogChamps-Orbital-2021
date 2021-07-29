using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponScript : MonoBehaviour, IWeapon
{
    public Transform firepoint;
    public int weaponLevel;
    public int maxAmmo;
    public int currAmmo { get; set; }
    public float fireDelay { get { return m_fireDelay; } set { m_fireDelay = fireDelay; } }
    public float m_fireDelay;
    public float baseDMG;
    public float dmgFalloff;
    public bool inInventory;
    public wElement element;
    public LayerMask collisionLayer;
    public ParticleSystem muzzleFlash;
    [HideInInspector] public bool pointedAt = false;
    private List<cakeslice.Outline> outlineList = new List<cakeslice.Outline>();
    //public List<WeaponInstance> modifiers;

    void Awake()
    {
        currAmmo = maxAmmo;

        foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
        {
            cakeslice.Outline currOutline = rend.gameObject.AddComponent<cakeslice.Outline>();
            currOutline.color = 0;
            currOutline.enabled = false;
            outlineList.Add(currOutline);
        }
    }

    void Update()
    {
        foreach (cakeslice.Outline currOutline in outlineList)
        {
            if (pointedAt)
                currOutline.enabled = true;
            else
                currOutline.enabled = false;
        }
        
        pointedAt = false;
    }

    public virtual void primaryFire()
    {
        throw new System.NotImplementedException();
    }

    public virtual void secondaryFire()
    {
        throw new System.NotImplementedException();
    }
}