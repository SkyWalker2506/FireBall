using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class GUIController : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highscoreText;
    [SerializeField]
    private Text realtimeScoreText;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject holdStartText;

    private void OnEnable()
    {
        ActionSystem.OnLevelLoaded += SetLevel;
        ActionSystem.OnGameStarted += SetGameStarted;
        ActionSystem.OnGameEnded += SetGameOverScreen;
        ActionSystem.OnScoreChanged += SetRealTimeScore;
    }

    private void OnDisable()
    {
        ActionSystem.OnLevelLoaded -= SetLevel;
        ActionSystem.OnGameStarted -= SetGameStarted;
        ActionSystem.OnGameEnded -= SetGameOverScreen;
        ActionSystem.OnScoreChanged -= SetRealTimeScore;
    }

    void SetLevel()
    {
        holdStartText.SetActive(true);
    }

    void SetGameStarted()
    {
        holdStartText.SetActive(false);
    }

    public void HandleRestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetRealTimeScore()
    {
        realtimeScoreText.text = GameManager.Instance.CurrentScore.ToString("0.00");
    }

    public void SetGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        realtimeScoreText.gameObject.SetActive(false);
        scoreText.text = GameManager.Instance.CurrentScore.ToString("0.00");
        highscoreText.text = "HighestScore: " + GlobalValues.HighestScore.ToString("0.00");
    }
}