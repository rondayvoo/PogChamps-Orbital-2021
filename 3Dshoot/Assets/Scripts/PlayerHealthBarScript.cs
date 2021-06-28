using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBarScript : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] TMP_Text TMPHealth;

    public void Start()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
        GameEvents.instance.OnPlayerHit += healthUpdate;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnPlayerHit -= healthUpdate;
    }

    public void healthUpdate(object sender, GameEvents.OnPlayerHitEventArgs ev)
    {
        TMPHealth.text = ev.currHealth.ToString();
        slider.maxValue = ev.maxHealth;
        slider.value = ev.currHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
