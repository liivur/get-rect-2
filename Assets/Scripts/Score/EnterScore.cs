using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnterScore : MonoBehaviour
{
    public TMP_InputField nameField;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = ScoreManager.instance.score.ToString();
    }

    public void Save()
    {
        HighScores.instance.AddNewScore(nameField.text, ScoreManager.instance.score);
        Menu.instance.NewGame();
    }
}
