using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu,
    }

    private static Scene targetscene;

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.Loading.ToString());

        targetscene = scene;
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(targetscene.ToString());
    }
}
