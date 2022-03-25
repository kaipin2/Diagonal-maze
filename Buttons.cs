using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private static string GameScene = "main";
    private static string TitleScene = "title";

    void Start()
    {
        //this.gameObject.AddComponent<>
    }
    public void TitleSceneMove()
    {
        OnClick();
        SceneManager.LoadScene(TitleScene);
    }
    public void GameSceneMove()
    {
        OnClick();
        SceneManager.LoadScene(GameScene);
    }
    public void Exit()
    {
        OnClick();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        UnityEngine.Application.Quit();
        #endif
    }

    public void OnClick()
    {

    }
}
