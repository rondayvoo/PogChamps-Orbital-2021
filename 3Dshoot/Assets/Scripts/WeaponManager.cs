using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int playerHeldWeapon;
    [SerializeField] int maxInventorySize;
    List<WeaponScriptableObject> playerInventory = new List<WeaponScriptableObject>();
    int qWeapon = 0;
    public float reachDist;
    public LayerMask gunLayer;

    void wSelect()
    {
        int iter = 0;
        foreach (Transform weapon in transform)
        {
            if (iter == playerHeldWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
        
            iter++;
        }
    }

    void wSwap()
    {
        RaycastHit facing;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out facing, reachDist, gunLayer))
        {
            if (playerInventory.Count >= maxInventorySize)
            {
                playerInventory[playerHeldWeapon] = facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst;
                wDrop();
                facing.transform.SetParent(transform);
                facing.transform.SetSiblingIndex(playerHeldWeapon);
                facing.rigidbody.detectCollisions = false;
                facing.rigidbody.isKinematic = true;
                facing.transform.localPosition = Vector3.zero;
                facing.transform.localRotation = Quaternion.identity;
            }
            
            else
            {
                playerInventory.Add(facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst);

                facing.transform.SetParent(transform);
                facing.rigidbody.detectCollisions = false;
                facing.rigidbody.isKinematic = true;
                facing.transform.localPosition = Vector3.zero;
                facing.transform.localRotation = Quaternion.identity;
            }
        }
    }

    //void wPickup(RaycastHit facing)
    //{
    //    playerInventory[playerInventory.Count] = facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst;
    //
    //    facing.transform.SetParent(transform);
    //    facing.transform.SetSiblingIndex(playerInventory.Count);
    //    facing.rigidbody.detectCollisions = false;
    //    facing.rigidbody.isKinematic = true;
    //    facing.transform.localPosition = Vector3.zero;
    //    facing.transform.localRotation = Quaternion.identity;
    //}

    void wDrop()
    {
        int iter = 0;

        foreach (Transform weapon in transform)
        {
            if (playerInventory.Count > 0 && iter == playerHeldWeapon)
            {
                weapon.GetComponent<Rigidbody>().position = weapon.GetComponent<Rigidbody>().position + weapon.GetComponent<Rigidbody>().transform.right * 10f;
                weapon.parent = null;
                weapon.GetComponent<Rigidbody>().detectCollisions = true;
                weapon.GetComponent<Rigidbody>().isKinematic = false;
                weapon.GetComponent<Rigidbody>().velocity = Camera.main.transform.up * 5f;
                return;
            }

            iter++;
        }
    }

    void primaryFireFunction()
    {
        if (playerInventory.Count > 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                CancelInvoke("Shoot");
            }

            if (Input.GetButton("Fire1") && !IsInvoking("Shoot"))
            {
                Invoke("Shoot", playerInventory[playerHeldWeapon].fireDelay);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    void Shoot()
    {
        Transform cam = Camera.main.transform;
        playerInventory[playerHeldWeapon].primaryFire(cam);
        playerInventory[playerHeldWeapon].fireDelay -= 0.01f;
    }

    // Start is called before the first frame update
    void Start()
    {
        wSelect();

        foreach (Transform weaponT in transform)
        {
            playerInventory.Add(weaponT.GetComponent<WeaponObject>().weaponInst);
            weaponT.GetComponent<Rigidbody>().isKinematic = true;
            weaponT.GetComponent<Rigidbody>().detectCollisions = false;
            weaponT.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (playerHeldWeapon >= playerInventory.Count - 1)
                playerHeldWeapon = 0;
            else
                playerHeldWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (playerHeldWeapon <= 0)
                playerHeldWeapon = playerInventory.Count - 1;
            else
                playerHeldWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerHeldWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerHeldWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerHeldWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerHeldWeapon = qWeapon;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            wSwap();
        }

        qWeapon = playerHeldWeapon;
        wSelect();
        
        primaryFireFunction();
    }
}
