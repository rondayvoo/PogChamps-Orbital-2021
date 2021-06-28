using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    //Singleton functionality
    void Awake()
    {
        stringventory = null;
        ammoCountCO = null;

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool loadSavedGame = false;

    //Player stuff to carry over
    [HideInInspector] public List<string> stringventory;
    [HideInInspector] public List<int> ammoCountCO;
    //[HideInInspector] public int playerHealth;
    //[HideInInspector] public int playerLevel;
    //[HideInInspector] public int playerExp;

    //Metadata
    public List<GameObject> weaponPrefabs;
    [HideInInspector] public int currStage = 1;
    [HideInInspector] public int currSubstage = 1;

    public List<string> SaveInventory(Transform weaponTransform)
    {
        List<string> savedInv = new List<string>();
        List<int> savedAmmo = new List<int>();

        foreach (Transform weapon in weaponTransform)
        {
            foreach (GameObject prefab in weaponPrefabs)
            {
                if (weapon.gameObject.name.Replace("(Clone)", "") == prefab.name)
                {
                    savedInv.Add(prefab.name);
                    savedAmmo.Add(weapon.GetComponent<IWeapon>().currAmmo);
                    break;
                }
            }
        }

        stringventory = savedInv;
        ammoCountCO = savedAmmo;
        return savedInv;
    }

    public void LoadInventory(Transform weaponTransform, List<string> savedInv, List<int> savedAmmo)
    {
        foreach (Transform weapon in weaponTransform)
            Destroy(weapon.gameObject);

        int index = 0;

        foreach (string stringW in savedInv)
        {
            foreach (GameObject prefab in weaponPrefabs)
            {
                if (prefab.name == stringW)
                {
                    GameObject instance = Instantiate(prefab);
                    instance.GetComponent<Rigidbody>().isKinematic = true;
                    instance.GetComponent<Rigidbody>().detectCollisions = false;
                    instance.transform.position = weaponTransform.position;
                    instance.transform.rotation = weaponTransform.rotation;
                    instance.transform.parent = weaponTransform;
                    instance.GetComponent<IWeapon>().currAmmo = savedAmmo[index];
                    break;
                }
            }

            index++;
        }
    }

    public event EventHandler<OnPlayerHitEventArgs> OnPlayerHit;
    public class OnPlayerHitEventArgs : EventArgs
    {
        public int maxHealth;
        public int currHealth;
        public int dmgTaken;
    }
    public void PlayerHit(int maxHealth, int currHealth, int dmgTaken)
    {
        OnPlayerHit?.Invoke(this, new OnPlayerHitEventArgs { maxHealth = maxHealth, currHealth = currHealth, dmgTaken = dmgTaken });
    }

    public event EventHandler OnPlayerUpdate;
    public void PlayerUpdate()
    {
        OnPlayerUpdate?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<OnAmmoCountChangeEventArgs> OnAmmoCountChange;
    public class OnAmmoCountChangeEventArgs : EventArgs
    {
        public int maxAmmo;
        public int currAmmo;
    }
    public void AmmoCountChange(int maxAmmo, int currAmmo)
    {
        OnAmmoCountChange?.Invoke(this, new OnAmmoCountChangeEventArgs { maxAmmo = maxAmmo, currAmmo = currAmmo });
    }

    //public event EventHandler OnPlayerShoot;
    /*
     * public class OnPlayerShootEventArgs : EventArgs
     * {
     *      public weaponType type;
     *      public int weaponAmmoCount;
     * }
     */

    //public event EventHandler OnPlayerDie;
    //public void PlayerDie()
    //{
    //    OnPlayerDie?.Invoke(this, EventArgs.Empty);
    //}

    //public event EventHandler OnEnemyHit;
    /* 
     * public class OnEnemyHitEventArgs : EventArgs
     * {
     *      public enemyType type;
     *      public int dmgDealt;
     * }
     */

    public event EventHandler<OnEnemyKillEventArgs> OnEnemyKill;
    public class OnEnemyKillEventArgs : EventArgs
    {
         public GameObject enemy;
    }
    public void EnemyKill(GameObject enemy)
    {
        OnEnemyKill?.Invoke(this, new OnEnemyKillEventArgs { enemy = enemy });
    }
    

    public event EventHandler OnStageClear;
    public void StageClear()
    {
        OnStageClear?.Invoke(this, EventArgs.Empty);
        currSubstage++;
        SceneLoader.LoadLevel(currStage);
    }
    

    public event EventHandler OnSave;
    public void Save()
    {
        OnSave?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnLoad;
    public void Load()
    {
        OnLoad?.Invoke(this, EventArgs.Empty);
    }
}