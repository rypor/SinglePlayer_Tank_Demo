using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static int targetSceneIndex;
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    public static void Load(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
