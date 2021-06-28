using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageIndicator : MonoBehaviour
{
    TMP_Text stageText;

    // Start is called before the first frame update
    void Start()
    {
        stageText = GetComponent<TMP_Text>();
        stageText.text = "Stage " + GameEvents.instance.currStage + "-" + GameEvents.instance.currSubstage;
    }
}
