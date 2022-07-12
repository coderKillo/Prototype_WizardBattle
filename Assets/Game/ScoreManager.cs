using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager instance;
    static public ScoreManager Instance { get { return instance; } }

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;
    public int Score { get { return score; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Setup();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Setup()
    {
        EnemyHealth.OnSpawn += Increase;
        EnemyHealth.OnDeath += Decrease;
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public void Increase() => Increase(1);

    public void Increase(int amount = 1) => score += amount;

    public void Decrease() => Decrease(1);

    public void Decrease(int amount = 1) => score -= amount;
}
