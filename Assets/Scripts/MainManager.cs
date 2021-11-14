using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;       // gameObject
    public int LineCount = 6;       // matrix element count
    public Rigidbody Ball;          // gameObject

    public Text ScoreText;          //UI
    public Text highScoreText;
    public string highScorePlayerName;
    // private Text playerName;
    private int savedHighScore;
    
    public GameObject GameOverText; //UI
    
    private bool m_Started = false; // game status
    private int _currentScore;           // current player points
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        SetHighScore();
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        highScoreText.text = $"Best Score: {savedHighScore}: Name: {highScorePlayerName}";
    }

    private void SetHighScore()
    {
        savedHighScore = LoadSaveManager.Instance.savedHighScore;
        highScorePlayerName = LoadSaveManager.Instance.highScorePlayerName;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        _currentScore += point;
        ScoreText.text = $"Score : {_currentScore}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        if (_currentScore > savedHighScore)
        {
            LoadSaveManager.Instance.newHighScore = _currentScore;
            LoadSaveManager.Instance.SavePlayer();
        }
        GameOverText.SetActive(true);
    }
}
