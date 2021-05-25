using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomModuleScript : MonoBehaviour
{
    public Transform doorParent;
    [HideInInspector] public List<Transform> availableDoors;

    public void Awake()
    {
        availableDoors = new List<Transform>();

        foreach (Transform child in doorParent)
            availableDoors.Add(child);
    }
}
