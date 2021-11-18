using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    public void StartView()
    {
        SceneManager.LoadScene(1); // load/re-load main scene
    }
    public void Exit()
    {
        LoadSaveManager.Instance.SavePlayer();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
