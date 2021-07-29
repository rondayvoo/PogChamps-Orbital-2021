using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using System;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject optionsUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject minimapObj;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] RenderPipelineAsset[] qualityLvls;
    [SerializeField] Slider volumeSlider;
    Resolution[] resolutions;

    public void gPause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gamePaused = true;
        GameEvents.instance.allowedToShoot = false;
        pauseUI.SetActive(true);
    }

    public void gResume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gamePaused = false;
        pauseUI.SetActive(false);
        GameEvents.instance.allowedToShoot = true;
    }

    public void gMainMenuWithSave()
    {
        gSave();
        Time.timeScale = 1f;
        GameEvents.instance.playerSpawn.SetActive(false);
        SceneLoader.Load(SceneLoader.Scene.menuScene);
    }

    public void gQuitGameWithSave()
    {
        gSave();
        Application.Quit();
    }

    public void gMainMenu()
    {
        Time.timeScale = 1f;
        GameEvents.instance.playerSpawn.SetActive(false);
        SceneLoader.Load(SceneLoader.Scene.menuScene);
    }

    public void gQuitGame()
    {
        Application.Quit();
    }

    public void gOpenOptions()
    {
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void gCloseOptions()
    {
        optionsUI.SetActive(false);
        pauseUI.SetActive(true);
    }

    public void gEnableMinimap()
    {
        minimapObj.SetActive(true);
    }

    public void gDisableMinimap()
    {
        minimapObj.SetActive(false);
    }

    public void gFullscreenMode(bool active)
    {
        Screen.fullScreen = active;
    }

    public void gSetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void gSetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        QualitySettings.renderPipeline = qualityLvls[index];
    }

    public void gSetVolume(float vol)
    {
        AudioListener.volume = vol / volumeSlider.maxValue;
    }

    public void gSave()
    {
        GameEvents.instance.Save();
        Debug.Log("Game saved.");
    }

    public void showGameOver(object sender, EventArgs ev)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gamePaused = true;
        gameOverUI.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resOptions = new List<string>();

        int currResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width.ToString() + " x " + resolutions[i].height.ToString();
            resOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currResIndex = i;
        }

        gResume();
        resolutionDropdown.AddOptions(resOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        volumeSlider.value = AudioListener.volume / 1.0f * volumeSlider.maxValue;

        GameEvents.instance.OnPlayerDie += showGameOver;
    }

    void OnDestroy()
    {
        GameEvents.instance.OnPlayerDie -= showGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused && pauseUI.activeSelf)
                gResume();
            else
                gPause();
        }
    }
}
