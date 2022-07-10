using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PlayerScoreUI : MonoBehaviour
{
    static private PlayerScoreUI instance;
    static public PlayerScoreUI Instance { get { return instance; } }

    private TextMeshProUGUI scoreText;

    void Awake()
    {
        if (instance == null) instance = this;

        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
