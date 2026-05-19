using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text bestScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;
    private string currentPlayer;
    private GameData gameData;
    private PlayerRecord currentPlayerRecord;
    
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = PlayerPrefs.GetString("CurrentPlayerName", "Guest");

        gameData = SaveSystem.LoadGame();
        currentPlayerRecord = gameData.playerRecords.Find(p => p.playerName.Equals(currentPlayer, System.StringComparison.OrdinalIgnoreCase));

        if (currentPlayerRecord == null )
        {
            currentPlayerRecord = new PlayerRecord { playerName = currentPlayer, highScore = 0 };
            gameData.playerRecords.Add(currentPlayerRecord);
        }

        UpdateHighScoreUI();

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
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    private void UpdateHighScoreUI()
    {
        if (gameData.playerRecords.Count == 0) return;

        PlayerRecord topPlayer = gameData.playerRecords[0];
        foreach (PlayerRecord playerRecord in gameData.playerRecords)
        {
            if (playerRecord.highScore >  topPlayer.highScore)
            {
                topPlayer = playerRecord;
            }
        }

        bestScoreText.text = "Best Score: " + topPlayer.playerName + ": " + topPlayer.highScore;
    }

    public void CheckAndSaveNewScore(int finalScore)
    {
        if (finalScore > currentPlayerRecord.highScore)
        {
            currentPlayerRecord.highScore = finalScore;
            SaveSystem.SaveGame(gameData);

            UpdateHighScoreUI();
        }
    }

    public void GameOver()
    {
        CheckAndSaveNewScore(m_Points);
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
