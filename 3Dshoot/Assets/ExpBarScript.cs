using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarScript : MonoBehaviour
{
    Slider slider;

    public void addExp(object sender, GameEvents.OnEnemyKillEventArgs ev)
    {
        slider.value += ev.enemy.GetComponent<BaseEnemyScript>().enemyMaxHealth / 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 1000;
        slider.value = 0;
        GameEvents.instance.OnEnemyKill += addExp;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnEnemyKill -= addExp;
    }
}
