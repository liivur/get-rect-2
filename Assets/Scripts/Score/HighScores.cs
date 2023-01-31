using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public static HighScores instance;
    public HighScoreDisplay[] highScoreDisplayArray;
    List<HighScoreEntry> scores = new List<HighScoreEntry>();

    void Awake()
    {
        instance = this;
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void Start()
    {
        Load();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        scores.Sort((HighScoreEntry x, HighScoreEntry y) => y.score.CompareTo(x.score));
        for (int i = 0; i < highScoreDisplayArray.Length; i++)
        {
            if (i < scores.Count)
            {
                highScoreDisplayArray[i].DisplayHighScore(scores[i].name, scores[i].score);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }
    }

    public void AddNewScore(string entryName, float entryScore)
    {
        scores.Add(new HighScoreEntry { name = entryName, score = entryScore });
        Save();
    }

    void Load()
    {
        scores = XMLManager.instance.LoadScores();
    }

    void Save()
    {
        XMLManager.instance.SaveScores(scores);
    }
}

public class HighScoreEntry
{
    public string name;
    public float score;
}