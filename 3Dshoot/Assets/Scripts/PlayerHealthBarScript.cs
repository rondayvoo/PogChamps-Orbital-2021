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
        GameEvents.instance.OnPlayerStart += healthInit;
        GameEvents.instance.OnPlayerHit += healthUpdate;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnPlayerStart -= healthInit;
        GameEvents.instance.OnPlayerHit -= healthUpdate;
    }

    public void healthInit(object sender, GameEvents.OnPlayerStartEventArgs ev)
    {
        PlayerMovement pm = ev.player.GetComponent<PlayerMovement>();
        TMPHealth.text = pm.playerCurrHealth.ToString();
        slider.maxValue = pm.playerMaxHealth;
        slider.value = pm.playerCurrHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void healthUpdate(object sender, GameEvents.OnPlayerHitEventArgs ev)
    {
        TMPHealth.text = ev.currHealth.ToString();
        slider.maxValue = ev.maxHealth;
        slider.value = ev.currHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
