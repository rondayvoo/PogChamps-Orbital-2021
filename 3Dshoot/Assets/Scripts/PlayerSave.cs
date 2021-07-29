using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    PlayerMovement pm;
    WeaponManager wm;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        wm = GetComponentInChildren<WeaponManager>();

        GameEvents.instance.OnSave += Save;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnSave -= Save;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Saving the game
    private string savePath => $"{Application.persistentDataPath}/saveP.md";

    private void Save(object sender, EventArgs ev)
    {
        saveFile(captureState());
    }

    public void Load()
    {
        PlayerSaveData state = loadFile();
        restoreState(state);
    }

    public PlayerSaveData captureState()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        PlayerSaveData psd = new PlayerSaveData();

        psd.position = new float[3];
        psd.position[0] = rb.position.x;
        psd.position[1] = rb.position.y;
        psd.position[2] = rb.position.z;

        psd.rotation = new float[4];
        psd.rotation[0] = rb.rotation.x;
        psd.rotation[1] = rb.rotation.y;
        psd.rotation[2] = rb.rotation.z;
        psd.rotation[3] = rb.rotation.w;

        psd.velocity = new float[3];
        psd.velocity[0] = rb.velocity.x;
        psd.velocity[1] = rb.velocity.y;
        psd.velocity[2] = rb.velocity.z;

        psd.health = pm.playerCurrHealth;
        psd.exp = pm.playerExperience;

        psd.invState = GameEvents.instance.SaveInventory(wm.transform);
        psd.currHeldWeapon = wm.playerHeldWeapon;
        psd.ammoCount = new List<int>();

        foreach (Transform weapon in wm.transform)
            psd.ammoCount.Add(weapon.GetComponent<BaseWeaponScript>().currAmmo);

        return psd;
    }

    public void restoreState(object state)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        PlayerSaveData psd = (PlayerSaveData)state;

        rb.position = new Vector3(psd.position[0], psd.position[1], psd.position[2]);
        rb.rotation = new Quaternion(psd.rotation[0], psd.rotation[1], psd.rotation[2], psd.rotation[3]);
        rb.velocity = new Vector3(psd.velocity[0], psd.velocity[1], psd.velocity[2]);

        pm.playerCurrHealth = psd.health;
        pm.playerExperience = psd.exp;
        pm.playerLevel = psd.exp / pm.expToLevel + 1;

        GameEvents.instance.LoadInventory(wm.transform, psd.invState, psd.ammoCount);
        wm.playerHeldWeapon = psd.currHeldWeapon;
        wm.wSelect();
    }

    private void saveFile(PlayerSaveData state)
    {
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private PlayerSaveData loadFile()
    {
        if (!File.Exists(savePath))
            return new PlayerSaveData();

        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (PlayerSaveData)formatter.Deserialize(stream);
        }
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public float[] position;
    public float[] rotation;
    public float[] velocity;
    public int health;
    public int exp;

    public int currHeldWeapon;
    public List<string> invState;
    public List<int> ammoCount;
}