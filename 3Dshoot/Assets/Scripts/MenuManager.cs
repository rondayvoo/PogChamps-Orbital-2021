using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject optionsUI;
    [SerializeField] GameObject minimapObj;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] RenderPipelineAsset[] qualityLvls;
    Resolution[] resolutions;

    public void gPause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gamePaused = true;
        pauseUI.SetActive(true);
    }

    public void gResume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gamePaused = false;
        pauseUI.SetActive(false);
    }

    public void gMainMenu()
    {
        gSave();
        Time.timeScale = 1f;
        SceneLoader.Load(SceneLoader.Scene.menuScene);
    }

    public void gQuitGame()
    {
        gSave();
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

    public void gSave()
    {
        GameEvents.instance.Save();
        Debug.Log("Game saved.");
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                gResume();
            else
                gPause();
        }
    }
}
