using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int playerHeldWeapon;
    int qWeapon = 0;
    IWeapon currWeapon;
    float currFireDelay;
    public float reachDist;
    public LayerMask gunLayer;
    public Transform levelGenerator;

    public void wSelect()
    {
        int iter = 0;
        foreach (Transform weapon in transform)
        {
            if (iter == playerHeldWeapon)
            {
                weapon.gameObject.SetActive(true);
                currWeapon = weapon.GetComponent<IWeapon>();
                GameEvents.instance.AmmoCountChange(weapon.GetComponent<BaseWeaponScript>().maxAmmo, currWeapon.currAmmo);
            }
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
            facing.transform.GetComponent<BaseWeaponScript>().pointedAt = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (transform.childCount >= 3)
                {
                    //playerInventory[playerHeldWeapon] = facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst;
                    wDrop();
                    facing.transform.SetParent(transform);
                    facing.transform.SetSiblingIndex(playerHeldWeapon);
                    facing.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    facing.rigidbody.detectCollisions = false;
                    facing.rigidbody.isKinematic = true;
                    facing.transform.GetComponent<BaseWeaponScript>().inInventory = true;
                    facing.transform.localPosition = Vector3.zero;
                    facing.transform.localRotation = Quaternion.identity;
                }

                else
                {
                    //playerInventory.Add(facing.transform.gameObject.GetComponent<WeaponObject>().weaponInst);

                    facing.transform.SetParent(transform);
                    facing.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    facing.rigidbody.detectCollisions = false;
                    facing.rigidbody.isKinematic = true;
                    facing.transform.GetComponent<BaseWeaponScript>().inInventory = true;
                    facing.transform.localPosition = Vector3.zero;
                    facing.transform.localRotation = Quaternion.identity;
                }
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
            if (transform.childCount > 0 && iter == playerHeldWeapon)
            {
                weapon.position = weapon.position + transform.forward * 2f;
                weapon.parent = levelGenerator;
                weapon.GetComponent<Rigidbody>().detectCollisions = true;
                weapon.GetComponent<Rigidbody>().isKinematic = false;
                weapon.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                weapon.GetComponent<BaseWeaponScript>().inInventory = false;
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
        if (Input.GetButton("Fire1") && currFireDelay <= 0f && currWeapon != null && currWeapon.currAmmo > 0)
        {
            Transform cam = Camera.main.transform;
            currWeapon.primaryFire();
            currWeapon.currAmmo--;
            currFireDelay += currWeapon.fireDelay;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //levelGenerator = GameObject.FindGameObjectWithTag("Respawn").transform;

        foreach (Transform weaponT in transform)
        {
            weaponT.GetComponent<BaseWeaponScript>().inInventory = true;
            weaponT.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            weaponT.GetComponent<Rigidbody>().isKinematic = true;
            weaponT.GetComponent<Rigidbody>().detectCollisions = false;
            weaponT.transform.parent = transform;
        }

        wSelect();
    }

    void OnDestroy()
    {
        //GameEvents.instance.OnStageClear -= weaponTransfer;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEvents.instance.allowedToShoot)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                qWeapon = playerHeldWeapon;

                if (playerHeldWeapon >= transform.childCount - 1)
                    playerHeldWeapon = 0;
                else
                    playerHeldWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                qWeapon = playerHeldWeapon;

                if (playerHeldWeapon <= 0)
                    playerHeldWeapon = transform.childCount - 1;
                else
                    playerHeldWeapon--;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 1)
            {
                qWeapon = playerHeldWeapon;
                playerHeldWeapon = 0;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            {
                qWeapon = playerHeldWeapon;
                playerHeldWeapon = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
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

            wSwap();
            primaryFireFunction();
        }

        wSelect();
        wSway();

        currFireDelay = currFireDelay <= 0f ? 0f : currFireDelay - Time.deltaTime;
    }
}
