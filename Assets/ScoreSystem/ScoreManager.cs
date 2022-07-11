using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    public int Score { get { return score; } set { score = value; } }

    public void IncreaseScore(int amount = 1)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void DecreaseScore(int amount = 1)
    {
        score -= amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }
}
