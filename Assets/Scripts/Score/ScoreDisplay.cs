using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    float score;
    public void IncreaseScore(float amount)
    {
        score += amount;
        UpdateScoreDisplay();
    }
    public void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score;
    }
}