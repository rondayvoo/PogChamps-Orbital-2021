using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gamePaused = false;
    [SerializeField] GameObject pauseUI;

    public void gPause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;    
        gamePaused = true;
    }

    public void gResume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gResume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused == false)
            {
                gPause();
            }

            else
            {
                gResume();
            }
        }
    }
}
