using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }

    public event Action<PickupScriptableObject> onPlayerPickup;

    public void PlayerPickup(PickupScriptableObject pickup)
    {
        if (onPlayerPickup != null)
        {
            onPlayerPickup(pickup);
        }
    }

    public event Action onPlayerUpdate;

    public void PlayerUpdate()
    {
        if (onPlayerUpdate != null)
        {
            onPlayerUpdate();
        }
    }

    public event Action onPlayerShoot;

    public void PlayerShoot()
    {
        if (onPlayerShoot != null)
        {
            onPlayerShoot();
        }
    }

    public event Action onPlayerHit;

    public void PlayerHit()
    {
        if (onPlayerHit != null)
        {
            onPlayerHit();
        }
    }

    public event Action onEnemyHit;

    public void EnemyHit()
    {
        if (onEnemyHit != null)
        {
            onEnemyHit();
        }
    }
}
