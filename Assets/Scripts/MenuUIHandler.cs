#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    private string _inputName;
    public Text highScoreText;
    private InputField _inputField;  
    void Start()
    {
        _inputField = GetComponentInChildren<InputField>();
        _inputField.ActivateInputField();
        
        DisplayHighScore();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) StartView();
        if(Input.GetKeyDown(KeyCode.Escape)) Exit();
    }

    public void StartView()
    {
        LoadSaveManager.Instance.currentPlayerName = (string.IsNullOrEmpty(_inputName)) ? "No Name" : _inputName;
        LoadSaveManager.Instance.currentLevel = 0;
        SceneManager.LoadScene(1);
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
    }
    
    private void DisplayHighScore()
    {
        var savedHighScore = LoadSaveManager.Instance.savedHighScore;
        if (savedHighScore < 1) highScoreText.gameObject.SetActive(false);
        var highScorePlayerName = LoadSaveManager.Instance.highscorePlayerName;
        highScoreText.text = $"Highscore: {savedHighScore} {highScorePlayerName}";
    }
}