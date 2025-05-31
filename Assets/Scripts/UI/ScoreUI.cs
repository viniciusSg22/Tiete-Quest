using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalTime;
    public TextMeshProUGUI enemiesKilled;

    void Update()
    {
        if (ScoreManager.Instance == null) return;

        if (SceneManager.GetActiveScene().name == "EndGame")
        {
            int score = ScoreManager.Instance.currentScore;
            int kills = ScoreManager.Instance.totalEnemiesKilled;
            float time = ScoreManager.Instance.elapsedTime;

            string timeFormatted = System.TimeSpan.FromSeconds(time).ToString(@"mm\:ss");

            scoreText.text = score.ToString();
            totalTime.text = timeFormatted;
            enemiesKilled.text = kills.ToString();
        }
        else
        {
            scoreText.text = ScoreManager.Instance.currentScore.ToString();
            enemiesKilled.text = ScoreManager.Instance.totalEnemiesKilled.ToString();
        }
    }
}
