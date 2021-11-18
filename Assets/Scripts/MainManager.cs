using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;       // gameObject
    public int LineCount = 6;       // matrix element count
    public Rigidbody Ball;          // gameObject
    public Text ScoreText;          //UI
    public Text CurrentLevelText;
    private string highScorePlayerName;
    private int savedHighScore;
    private int brickCount;
    public GameObject GameOverText; //UI
    public GameObject LevelText; //UI
    private bool m_Started;
    private int _currentScore;
    private int _currentLevel = 1;
    private string _currentPlayer;
    private bool m_GameOver;
    [SerializeField] private float yPos = 2.75f;
    void Start()
    {
        if ( LoadSaveManager.Instance.currentLevel > 1)
        {
            _currentScore = LoadSaveManager.Instance.currentScore; 
            _currentLevel = LoadSaveManager.Instance.currentLevel;
            yPos = SetYPos();
        }
        _currentPlayer = LoadSaveManager.Instance.currentPlayerName;
        UpdateScoreText();
        DrawBricks();
    }

    void UpdateHighScore()
    {
        if (_currentScore > savedHighScore)
        {
            LoadSaveManager.Instance.newHighScore = _currentScore;
            LoadSaveManager.Instance.SavePlayer();
        }
    }

    float SetYPos()
    {
        yPos = ((yPos -= 0.125f * _currentLevel) >= 1.75f) ? yPos : 1.75f;
        return yPos;
    }

    void DrawBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, yPos + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brickCount++;
            }
        }
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
                LoadSaveManager.Instance.currentLevel = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateHighScore();
            SceneManager.LoadScene(0);
        }
    }
    void AddPoint(int point)
    {
        _currentScore += point;
        brickCount--;
        UpdateScoreText();
        UpdateHighScore();
        if (brickCount > 0) return;
        LevelComplete();
    }
    void LevelComplete()
    {
        Ball.gameObject.SetActive(false);
        _currentLevel++;
        UpdateHighScore();
        LoadSaveManager.Instance.currentScore = _currentScore;
        LoadSaveManager.Instance.currentLevel = _currentLevel;
        LevelText.SetActive(true);
    }
    public void GameOver()
    {
        m_GameOver = true;
        UpdateHighScore();
        GameOverText.SetActive(true);
    }
    void UpdateScoreText()
    {
        CurrentLevelText.text = $"Level {_currentLevel}";
        ScoreText.text = $"{_currentPlayer} : {_currentScore}";
    }
}
