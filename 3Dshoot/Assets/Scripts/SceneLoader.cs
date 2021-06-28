using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private class LoadingMonobehaviour : MonoBehaviour { };
    public static AsyncOperation asO;

    public enum Scene
    {
        gameScene,
        loadScene,
        menuScene
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonobehaviour>().StartCoroutine(LoadSceneAsync(scene));
            //SceneManager.LoadSceneAsync(scene);
        };

        SceneManager.LoadScene(Scene.loadScene.ToString());
    }

    public static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        asO = SceneManager.LoadSceneAsync(scene.ToString());
        
        while (!asO.isDone)
            yield return null;
    }

    public static float loadProgress()
    {
        if (asO != null)
            return asO.progress;
        else
            return 1f;
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    public static void LoadLevel(int level)
    {
        Load(Scene.gameScene);
    }
}
