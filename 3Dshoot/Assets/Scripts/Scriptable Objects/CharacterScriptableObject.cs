using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterScriptableObject : ItemScriptableObject
{
    public int charHealth;
    public float charSpeed;

    public abstract void takeDamage(int currentHealth, int damage);
}

//public class Character<T>
//{
//    public CharacterScriptableObject<T> character;
//
//    public Character(CharacterScriptableObject<T> blueprint)
//    {
//        character = blueprint;
//    }
//};
