using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarScript : MonoBehaviour
{
    Slider slider;
    int playerLevelInt;
    TMP_Text playerLevel;
    PlayerMovement playerInfo;

    public void Init(object sender, EventArgs ev)
    {
        slider.maxValue = playerInfo.expToLevel;
        slider.value = playerInfo.playerExperience % playerInfo.expToLevel;
        playerLevelInt = playerInfo.playerLevel;
        playerLevel.text = "Level " + playerLevelInt.ToString();
    }

    public void addExp(object sender, GameEvents.OnEnemyKillEventArgs ev)
    {
        slider.maxValue = playerInfo.expToLevel;
        slider.value = playerInfo.playerExperience % playerInfo.expToLevel;
        playerLevelInt = playerInfo.playerLevel;
        playerLevel.text = "Level " + playerLevelInt.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        playerLevel = GetComponentInChildren<TMP_Text>();
        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        slider.maxValue = playerInfo.expToLevel;
        slider.value = playerInfo.playerExperience % playerInfo.expToLevel;
        playerLevelInt = playerInfo.playerLevel;
        playerLevel.text = "Level " + playerLevelInt.ToString();

        GameEvents.instance.OnPlayerStart += Init;
        GameEvents.instance.OnEnemyKill += addExp;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnPlayerStart -= Init;
        GameEvents.instance.OnEnemyKill -= addExp;
    }
}
