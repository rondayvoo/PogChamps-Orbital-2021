using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int playerHeldWeapon;
    [SerializeField] int maxInventorySize;
    public PlayerInventoryScriptableObject playerInventory;
    int qWeapon = 0;
    float currFireDelay;
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
            if (playerInventory.inventoryWeaponList.Count >= maxInventorySize)
            {
                playerInventory.inventoryWeaponList[playerHeldWeapon] = facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst;
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
                playerInventory.inventoryWeaponList.Add(facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst);

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
            if (playerInventory.inventoryWeaponList.Count > 0 && iter == playerHeldWeapon)
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

    void wSway()
    {
        float xRotate = Input.GetAxis("Mouse X");
        float yRotate = Input.GetAxis("Mouse Y");
        float sensitivity = 5f;
        Quaternion xAdjust = Quaternion.AngleAxis(sensitivity * xRotate, -Vector3.up);
        Quaternion yAdjust = Quaternion.AngleAxis(sensitivity * yRotate, Vector3.right);
        Quaternion targetRotate = transform.localRotation * xAdjust * yAdjust;

        foreach (Transform weapon in transform)
        {
            weapon.localRotation = Quaternion.Lerp(weapon.localRotation, targetRotate, Time.deltaTime);
        }
    }

    void primaryFireFunction()
    {
        if (playerInventory.inventoryWeaponList.Count > 0)
        {
            if (Input.GetButton("Fire1") && currFireDelay <= 0f)
            {
                Transform cam = Camera.main.transform;
                RaycastHit ray;

                Physics.Raycast(cam.position, cam.forward, out ray);

                if (playerInventory.inventoryWeaponList[playerHeldWeapon].drawBulletTrail)
                    playerInventory.inventoryWeaponList[playerHeldWeapon].drawTrail(transform.GetChild(playerHeldWeapon).GetComponent<WeaponObject>().firepoint, ray.transform);

                playerInventory.inventoryWeaponList[playerHeldWeapon].primaryFire(cam);
                currFireDelay += playerInventory.inventoryWeaponList[playerHeldWeapon].fireDelay;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Remove this when trying to implement saving
        playerInventory.inventoryWeaponList = new List<WeaponScriptableObject>();

        foreach (Transform weaponT in transform)
        {
            playerInventory.inventoryWeaponList.Add(weaponT.GetComponent<WeaponObject>().weaponInst);
            weaponT.GetComponent<Rigidbody>().isKinematic = true;
            weaponT.GetComponent<Rigidbody>().detectCollisions = false;
            weaponT.transform.parent = transform;
        }

        wSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            qWeapon = playerHeldWeapon;

            if (playerHeldWeapon >= playerInventory.inventoryWeaponList.Count - 1)
                playerHeldWeapon = 0;
            else
                playerHeldWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            qWeapon = playerHeldWeapon;

            if (playerHeldWeapon <= 0)
                playerHeldWeapon = playerInventory.inventoryWeaponList.Count - 1;
            else
                playerHeldWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && playerInventory.inventoryWeaponList.Count >= 1)
        {
            qWeapon = playerHeldWeapon;
            playerHeldWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && playerInventory.inventoryWeaponList.Count >= 2)
        {
            qWeapon = playerHeldWeapon;
            playerHeldWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && playerInventory.inventoryWeaponList.Count >= 3)
        {
            qWeapon = playerHeldWeapon;
            playerHeldWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            int temp = playerHeldWeapon;
            playerHeldWeapon = qWeapon;
            qWeapon = temp;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            wSwap();
        }

        wSelect();
        wSway();
        
        primaryFireFunction();
        currFireDelay = currFireDelay <= 0f ? 0f : currFireDelay - Time.deltaTime;
    }
}
