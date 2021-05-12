using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Profile", menuName = "Character Profile")]
public abstract class CharacterScriptableObject : ScriptableObject
{
    public string charName;
    public string charDesc;
    public int charHealth;
    public float charSpeed;

    public abstract void takeDamage(int damage);
    //public abstract Character<T> createCharacter();
    //public abstract CharacterScriptableObject Clone();
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
