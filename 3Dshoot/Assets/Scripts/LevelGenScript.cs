using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenScript : MonoBehaviour
{
    public List<GameObject> roomModules;
    public int iter;
    public int seed;
    public bool seededRuns;
    
    System.Random rng = new System.Random();
    List<GameObject> createdModules = new List<GameObject>();//, availableModules;
    List<Transform> disconnectedDoors = new List<Transform>();

    public void buildRooms()
    {
        //Build initial room
        GameObject initialModule = Instantiate(roomModules[rng.Next(roomModules.Count)], transform.position, Quaternion.identity);
        disconnectedDoors.AddRange(initialModule.GetComponent<RoomModuleScript>().availableDoors);
        createdModules.Add(initialModule);

        //Recursively generate other rooms
        for (int i = 0; i < iter - 1; i++)
        {
            bool roomFound = false;
            Transform[] targetPoints = disconnectedDoors.OrderBy(d => rng.Next()).ToArray();

            //Try to find a good door on the map
            foreach (Transform tp in targetPoints)
            {
                //Shuffle and try every room to fit 
                GameObject[] shuffledBlocks = roomModules.OrderBy(d => rng.Next()).ToArray();
                foreach (GameObject sBlock in shuffledBlocks)
                {
                    GameObject candidate = Instantiate(sBlock);
                    Transform[] shuffledDoors = candidate.GetComponent<RoomModuleScript>().availableDoors.OrderBy(d => rng.Next()).ToArray();
                    bool doorFound = true;

                    //Look for a door on the new room to match
                    foreach (Transform door in shuffledDoors)
                    {
                        candidate.transform.Translate(tp.position - door.position);
                        candidate.transform.RotateAround(door.position, Vector3.up, Vector3.SignedAngle(door.forward, -tp.forward, Vector3.up));

                        //Check if there is an any overlapping
                        Bounds bound = candidate.GetComponent<BoxCollider>().bounds;
                        foreach (GameObject item in createdModules)
                        {
                            if (bound.Intersects(item.GetComponent<BoxCollider>().bounds))
                            {
                                //Try another module
                                doorFound = false;
                                break;
                            }
                        }

                        if (doorFound)
                        {
                            //Module connected safely
                            roomFound = true;
                            disconnectedDoors.AddRange(candidate.GetComponent<RoomModuleScript>().availableDoors);
                            disconnectedDoors.Remove(door);
                            disconnectedDoors.Remove(tp);
                            createdModules.Add(candidate);
                            break;
                        }
                    }

                    //This room can't work, so we destroy it
                    if (!doorFound)
                    {
                        Destroy(candidate);
                    }

                    if (roomFound)
                        break;
                }

                if (roomFound)
                    break;

                if (disconnectedDoors.Count == 0)
                {
                    //No available output on any modules. Stop the process
                    break;
                }
            }
        }

        foreach (GameObject item in createdModules)
        {
            //Disable overlap test colliders
            item.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (seededRuns)
            rng = new System.Random(seed);
        else
        {
            int rngSeed = rng.Next();
            seed = rngSeed;
            rng = new System.Random(rngSeed);
        }

        buildRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
