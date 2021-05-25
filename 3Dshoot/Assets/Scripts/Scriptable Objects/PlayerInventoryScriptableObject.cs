using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Item/Abstract/Player Inventory")]
public class PlayerInventoryScriptableObject : ItemScriptableObject
{
    public List<WeaponScriptableObject> inventoryWeaponList;
    public List<PickupScriptableObject> inventoryPickupList;

    void Reset()
    {
        inventoryWeaponList = new List<WeaponScriptableObject>();
        inventoryPickupList = new List<PickupScriptableObject>();
    }
}
