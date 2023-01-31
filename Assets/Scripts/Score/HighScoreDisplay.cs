using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public void DisplayHighScore(string name, float score)
    {
        nameText.text = name;
        scoreText.text = string.Format("{0:000000}", score);
    }
    public void HideEntryDisplay()
    {
        nameText.text = "";
        scoreText.text = "";
    }
}