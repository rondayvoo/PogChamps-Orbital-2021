using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelGenScript : MonoBehaviour
{
    public List<GameObject> roomModules;
    public int iter;
    public int cycles;

    public int seed;
    public bool seededRuns;

    NavMeshSurface nmSurface;
    public LayerMask envLayerMask;

    public GameObject portal;
    
    System.Random rng = new System.Random();
    List<GameObject> createdModules = new List<GameObject>();
    List<Transform> disconnectedDoors = new List<Transform>();
    List<GameObject> enemyPositions = new List<GameObject>();

    public void buildRooms()
    {
        //Build initial room
        GameObject initialModule = Instantiate(roomModules[rng.Next(roomModules.Count)], transform.position, Quaternion.identity);
        disconnectedDoors.AddRange(initialModule.GetComponent<RoomModuleScript>().availableDoors);
        createdModules.Add(initialModule);
        initialModule.transform.SetParent(transform);

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
                            candidate.transform.SetParent(transform);

                            Collider[] cDoors = Physics.OverlapSphere(door.position, 0.5f);

                            foreach (Collider collider in cDoors)
                                collider.gameObject.SetActive(false);

                            break;
                        }
                    }

                    //This room can't work, so we destroy it
                    if (!doorFound)
                    {
                        DestroyImmediate(candidate);
                    }

                    if (roomFound)
                        break;
                }

                //No more available doors, stop the process
                if (roomFound || disconnectedDoors.Count == 0)
                    break;
            }
        }

        foreach (GameObject item in createdModules)
        {
            //Disable overlap test colliders
            item.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void addCycles()
    {
        while (disconnectedDoors.Count > 0)
        {
            Transform targetDoor = disconnectedDoors[0];
            disconnectedDoors.RemoveAt(0);

            foreach (Transform door in disconnectedDoors)
            {
                if (Vector3.Distance(door.position, targetDoor.position) <= 0.01f)
                {
                    disconnectedDoors.Remove(door);
                    Collider[] cDoors = Physics.OverlapSphere(door.position, 0.5f);

                    foreach (Collider collider in cDoors)
                        collider.gameObject.SetActive(false);

                    break;
                }
            }
        }
    }

    public void nmSetup()
    {
        //Build Navmesh for enemies to traverse
        nmSurface = gameObject.GetComponent<NavMeshSurface>();
        nmSurface.BuildNavMesh();
    }

    public void spawnEnemies()
    {
        foreach (GameObject module in createdModules)
            module.GetComponent<RoomModuleScript>().spawnEnemies();
    }

    public void addEnemiesToList()
    {
        foreach (GameObject module in createdModules)
        {
            foreach (Transform enemy in module.GetComponent<RoomModuleScript>().enemyParent)
            {
                if (enemy.gameObject.activeSelf == true)
                    enemyPositions.Add(enemy.gameObject);
            }
        }
            
    }

    public void removeEnemyFromList(object sender, GameEvents.OnEnemyKillEventArgs ev)
    {
        if (enemyPositions.Count == 1)
        {
            portal.transform.position = ev.enemy.transform.position;
            portal.gameObject.SetActive(true);
        }

        enemyPositions.Remove(ev.enemy);
        Instantiate(GameEvents.instance.weaponPrefabs[UnityEngine.Random.Range(0, GameEvents.instance.weaponPrefabs.Count)], 
                    ev.enemy.transform.position, UnityEngine.Random.rotation, transform);
        Debug.Log(enemyPositions.Count.ToString());
    }

    public void destroyRooms()
    {
        foreach (GameObject room in createdModules)
        {
            DestroyImmediate(room);
        }

        createdModules = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.instance.OnSave += Save;
        GameEvents.instance.OnEnemyKill += removeEnemyFromList;

        if (GameEvents.instance.loadSavedGame)
        {
            Load();
            GameEvents.instance.loadSavedGame = false;
        }

        else
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
            addCycles();
            nmSetup();
            spawnEnemies();
        }

        addEnemiesToList();
    }

    void OnDestroy()
    {
        GameEvents.instance.OnSave -= Save;
        GameEvents.instance.OnEnemyKill -= removeEnemyFromList;
    }

    //Saving the game
    private string savePath => $"{Application.persistentDataPath}/save.md";

    private void Save(object sender, EventArgs ev)
    {
        saveFile(captureState());
    }

    private void Load()
    {
        LevelSaveData state = loadFile();
        restoreState(state);
    }

    public LevelSaveData captureState()
    {
        LevelSaveData lsd = new LevelSaveData();
        lsd.seed = seed;
        lsd.objSaveData = new List<object>();
        lsd.wsdList = new List<WeaponSaveData>();

        foreach (GameObject weapon in GameObject.FindGameObjectsWithTag("SaveWeapon"))
        {
            if (!weapon.GetComponent<BaseWeaponScript>().inInventory)
            {
                Rigidbody rb = weapon.GetComponent<Rigidbody>();
                WeaponSaveData wsd = new WeaponSaveData();
                wsd.currAmmo = weapon.GetComponent<IWeapon>().currAmmo;

                wsd.name = weapon.name.Replace("(Clone)", "");

                wsd.position = new float[3];
                wsd.position[0] = weapon.transform.position.x;
                wsd.position[1] = weapon.transform.position.y;
                wsd.position[2] = weapon.transform.position.z;

                wsd.rotation = new float[4];
                wsd.rotation[0] = weapon.transform.rotation.x;
                wsd.rotation[1] = weapon.transform.rotation.y;
                wsd.rotation[2] = weapon.transform.rotation.z;
                wsd.rotation[3] = weapon.transform.rotation.w;

                if (rb)
                {
                    wsd.velocity = new float[3];
                    wsd.velocity[0] = rb.velocity.x;
                    wsd.velocity[1] = rb.velocity.y;
                    wsd.velocity[2] = rb.velocity.z;
                }

                lsd.wsdList.Add(wsd);
            }
        }

        foreach (ISaveable saveable in GetComponentsInChildren<ISaveable>(true))
            lsd.objSaveData.Add(saveable.captureState());

        return lsd;
    }

    public void restoreState(LevelSaveData lsd)
    {
        seed = lsd.seed;
        destroyRooms();
        rng = new System.Random(lsd.seed);
        buildRooms();
        addCycles();
        nmSetup();
        spawnEnemies();

        int index = 0;

        foreach (GameObject weapon in GameObject.FindGameObjectsWithTag("SaveWeapon"))
            DestroyImmediate(weapon);

        foreach (ISaveable saveable in GetComponentsInChildren<ISaveable>(true))
        {
            saveable.restoreState(lsd.objSaveData[index]);
            index++;
        }

        //List<WeaponSaveData> wsd = (List<WeaponSaveData>) lsd.wsdList;

        foreach (WeaponSaveData wsd in lsd.wsdList)
        {
            foreach (GameObject weaponPF in GameEvents.instance.weaponPrefabs)
            {
                if (weaponPF.name == wsd.name)
                {
                    GameObject weaponObj = Instantiate(weaponPF, transform);
                    Rigidbody rb = weaponObj.GetComponent<Rigidbody>();
                    weaponObj.GetComponent<IWeapon>().currAmmo = wsd.currAmmo;

                    weaponObj.transform.position = new Vector3(wsd.position[0], wsd.position[1], wsd.position[2]);
                    weaponObj.transform.rotation = new Quaternion(wsd.rotation[0], wsd.rotation[1], wsd.rotation[2], wsd.rotation[3]);

                    if (rb)
                        rb.velocity = new Vector3(wsd.velocity[0], wsd.velocity[1], wsd.velocity[2]);
                }
            }

            
        }
    }

    private void saveFile(LevelSaveData state)
    {
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private LevelSaveData loadFile()
    {
        if (!File.Exists(savePath))
            return new LevelSaveData();

        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (LevelSaveData)formatter.Deserialize(stream);
        }
    }
}

[System.Serializable]
public class LevelSaveData
{
    public int seed;
    public List<object> objSaveData;
    public List<WeaponSaveData> wsdList;
}

[System.Serializable]
public class WeaponSaveData
{
    public string name;
    public int currAmmo;
    public float[] position;
    public float[] rotation;
    public float[] velocity;
}