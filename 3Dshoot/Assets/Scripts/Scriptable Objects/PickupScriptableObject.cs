using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    ordinary,
    peculiar,
    tremendous,
    awesome,
    mythical
}

[CreateAssetMenu(fileName = "New Pickup", menuName = "Item/Pickup")]
public class PickupScriptableObject : ItemScriptableObject
{
    //public List<BuffsDebuffsCharacterScriptableObject> playerAttributes;
    //public List<BuffsDebuffsWeaponScriptableObject> weaponAttributes;

    public Rarity rarity;
    public int charHealthMod;
    public float charSpeedMod;
    public float charJumpMod;
}
