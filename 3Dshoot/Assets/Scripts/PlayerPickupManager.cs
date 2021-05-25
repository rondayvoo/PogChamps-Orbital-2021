using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupManager : MonoBehaviour
{
    Transform cam;
    [SerializeField] float reachDist = 10f;
    [SerializeField] LayerMask pickupLayer;
    public PlayerInventoryScriptableObject inventory;

    void pUse()
    {
        cam = Camera.main.transform;
        RaycastHit facing;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out facing, reachDist, pickupLayer))
        {
            PickupObject pObject = facing.transform.gameObject.GetComponent<PickupObject>();
            inventory.inventoryPickupList.Add(pObject.pickup);
            Destroy(facing.transform.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pUse();
        }
    }
}
