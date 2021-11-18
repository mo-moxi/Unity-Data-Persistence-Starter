using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab; 
    public int lineCount = 6;
    public Rigidbody ball;
    public Text scoreText; 
    public Text currentLevelText;
    private string _highScorePlayerName;
    private int _savedHighScore;
    private int _brickCount;
    public GameObject gameOverText;
    public GameObject levelText;
    private bool _started;
    private int _currentScore;
    private int _currentLevel = 1;
    private string _currentPlayer;
    private bool _gameOver;
    [SerializeField] private float yPos = 2.75f;
    public AudioSource audioSource;
    public AudioClip vanishSfx;
    public AudioClip gameOver;
    public AudioClip levelUp;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
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
        if (_currentScore > _savedHighScore)
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
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, yPos + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                _brickCount++;
            }
        }
    }
    private void Update()
    {
        if (!_started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();
                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (_gameOver)
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
        _brickCount--;
        audioSource.PlayOneShot(vanishSfx);
        UpdateScoreText();
        UpdateHighScore();
        if (_brickCount > 0) return;
        LevelComplete();
    }
    void LevelComplete()
    {
        ball.gameObject.SetActive(false);
        _currentLevel++;
        UpdateHighScore();
        audioSource.PlayOneShot(levelUp);
        LoadSaveManager.Instance.currentScore = _currentScore;
        LoadSaveManager.Instance.currentLevel = _currentLevel;
        levelText.SetActive(true);
    }
    public void GameOver()
    {
        _gameOver = true;
        UpdateHighScore();
        audioSource.Stop();
        audioSource.PlayOneShot(gameOver);
        gameOverText.SetActive(true);
        
    }
    void UpdateScoreText()
    {
        currentLevelText.text = $"Level {_currentLevel}";
        scoreText.text = $"{_currentPlayer} : {_currentScore}";
    }
}
