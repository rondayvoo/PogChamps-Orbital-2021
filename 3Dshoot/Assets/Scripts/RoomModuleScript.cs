using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RoomModuleScript : MonoBehaviour
{
    public Transform doorParent;
    public Transform enemyParent;
    [HideInInspector] public List<Transform> availableDoors;

    public List<GameObject> spawnEnemies()
    {
        List<GameObject> spawned = new List<GameObject>();

        foreach (Transform enemy in enemyParent)
        {
            enemy.gameObject.SetActive(true);
            spawned.Add(enemy.gameObject);
            enemy.GetComponent<NavMeshAgent>().enabled = true;
        }

        return spawned;
    }

    public void Awake()
    {
        availableDoors = new List<Transform>();

        foreach (Transform child in doorParent)
        {
            availableDoors.Add(child);
        }
    }
}
