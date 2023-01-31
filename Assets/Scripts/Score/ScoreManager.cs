using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public float score = 0;
    public float transitionSpeed = 100;
    float displayScore;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        displayScore = Mathf.MoveTowards(displayScore, score, transitionSpeed * Time.deltaTime);
        UpdateScoreDisplay();
    }
    public void IncreaseScore(float amount)
    {
        score += amount;
    }

    public void UpdateScoreDisplay()
    {
        scoreText.text = string.Format("Score: {0:00000}", displayScore);
    }
}
