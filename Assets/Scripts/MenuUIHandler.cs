#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    private string _inputName;

    public void StartView()
    {
        LoadSaveManager.Instance.currentPlayerName = (string.IsNullOrEmpty(_inputName)) ? "No Name" : _inputName;
        SceneManager.LoadScene(1); // load main scene
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

    public void ReadStringInput(string s)
    {
        _inputName = s;
        Debug.Log($"Player name {_inputName}");
    }
}