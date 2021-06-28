using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScreen : MonoBehaviour
{
    public Image img;
    public float fadeTime;
    private float fTimer = 0f;
    private float maxAlpha;
    private Color imgCol;

    public void showScreen(object sender, GameEvents.OnPlayerHitEventArgs ev)
    {
        fTimer = fadeTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.instance.OnPlayerHit += showScreen;
        maxAlpha = img.color.a;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnPlayerHit -= showScreen;
    }

    // Update is called once per frame
    void Update()
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, fTimer / fadeTime * maxAlpha);
        fTimer = fTimer <= 0f ? 0f : fTimer - Time.deltaTime;
    }
}
