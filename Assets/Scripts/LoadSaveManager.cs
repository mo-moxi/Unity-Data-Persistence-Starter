using System.IO;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour
{
    public static LoadSaveManager Instance;
    public int newHighScore;
    public int savedHighScore;
    public string highscorePlayerName;
    public int currentLevel;
    public int currentScore;
    public string currentPlayerName;
    [System.Serializable]
    class SaveData
    {
        public int score;
        public string playerName;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            newHighScore = 0;
            currentPlayerName = "";
            SavePlayer();
        }
    }
    public void SavePlayer()
    {
        SaveData data = new SaveData();
        data.score = newHighScore;
        data.playerName = currentPlayerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile1.json", json);
    }

    public void LoadSavedPlayer()
    {
        string path = Application.persistentDataPath + "/savefile1.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            savedHighScore = data.score;
            highscorePlayerName = data.playerName;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSavedPlayer();
    }
}
