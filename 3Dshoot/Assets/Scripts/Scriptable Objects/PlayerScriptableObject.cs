using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Profile", menuName = "Item/Character/Player")]
public class PlayerScriptableObject : CharacterScriptableObject
{
    public float playerJumpForce;
    public float playerMaxAccel;
    public int playerHeldWeapon;
    public float playerDmgMultiplier;
    public float playerFireRateMultiplier;
    public float playerShotSpeedMultiplier;
    public PlayerInventoryScriptableObject playerInventory;

    public override void takeDamage(int currentHealth, int damage)
    {
        
    }
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