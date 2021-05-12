using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Profile", menuName = "Character/Player")]
public class PlayerScriptableObject : CharacterScriptableObject
{
    public float playerJumpForce;
    public int playerHeldWeapon;
    public List<WeaponScriptableObject> playerInventory;
    public int maxInventorySize;

    public override void takeDamage(int damage)
    {
        
    }

    //public override Character<PlayerScriptableObject> createCharacter()
    //{
    //    Character<PlayerScriptableObject> newChar = new Character<PlayerScriptableObject>(this);
    //    return newChar;
    //}
    //
    //public override CharacterScriptableObject Clone()
    //{
    //    return new PlayerScriptableObject(this);
    //}
}

//[System.Serializable]
//public class Player
//{
//    public PlayerScriptableObject player;
//
//    public Player(PlayerScriptableObject blueprint)
//    {
//        player = blueprint;
//    }
//}