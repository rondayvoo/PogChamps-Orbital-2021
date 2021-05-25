using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Buff/Debuff", menuName = "Item/Abstract/Buffs and Debuffs/Character")]
public class BuffsDebuffsCharacterScriptableObject : ItemScriptableObject
{
    //public float playerJumpMod; Assign to child?
    public int charHealthMod;
    public float charSpeedMod;
    public float charJumpMod;
}
