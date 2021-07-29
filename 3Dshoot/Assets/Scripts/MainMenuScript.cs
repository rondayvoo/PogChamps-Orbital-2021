using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Rendering;

public class MainMenuScript : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject optionsUI;
    [SerializeField] Button continueButton;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] RenderPipelineAsset[] qualityLvls;
    [SerializeField] Slider volumeSlider;
    Resolution[] resolutions;

    bool saveFileExists;

    void Start()
    {
        if (File.Exists($"{Application.persistentDataPath}/saveL.md"))
            saveFileExists = true;
        else
            saveFileExists = false;

        if (!saveFileExists)
        {
            continueButton.interactable = false;
            TMP_Text text = continueButton.GetComponentInChildren<TMP_Text>();
            text.color = Color.grey;
        }

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

        resolutionDropdown.AddOptions(resOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        volumeSlider.value = AudioListener.volume / 1.0f * volumeSlider.maxValue;
    }

    public void gPlay()
    {
        GameEvents.instance.loadSavedGame = false;
        SceneLoader.Load(SceneLoader.Scene.gameScene);
        GameObject player = GameEvents.instance.playerReset();
        GameEvents.instance.currStage = 1;
        GameEvents.instance.currSubstage = 1;
        player.SetActive(true);
    }

    public void gDontOverwriteSave()
    {
        
    }

    public void gContinue()
    {
        GameEvents.instance.loadSavedGame = true;
        SceneLoader.Load(SceneLoader.Scene.gameScene);
        GameEvents.instance.playerSpawn.SetActive(true);
    }

    public void gOpenOptions()
    {
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void gCloseOptions()
    {
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
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

    public void gQuit()
    {
        Application.Quit();
    }
}
