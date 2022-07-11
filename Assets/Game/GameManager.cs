using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }

    [SerializeField] private GameEvent levelWonEvent;
    [SerializeField] private GameEvent startLevelEvent;

    private ScoreManager scoreManager;

    private int enemyCounter = 0;
    private int currentSceneIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    void Start()
    {
        StartLevel();
    }

    void Update()
    {
        if (CheckWinCondition())
        {
            GameWon();
        }
    }

    public void EnemySpawned()
    {
        enemyCounter++;
        scoreManager.Score = enemyCounter;
    }

    public void EnemyDied()
    {
        enemyCounter--;
        scoreManager.Score = enemyCounter;
    }

    public void StartLevel()
    {
        SpawnEnemies();
        startLevelEvent?.Invoke();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private static void SpawnEnemies()
    {
        foreach (var enemyPool in GameObject.FindObjectsOfType<EnemyPool>())
        {
            enemyPool.StartSpawn();
        }
    }

    private void GameWon()
    {
        levelWonEvent?.Invoke();
    }

    private bool CheckWinCondition()
    {
        return enemyCounter <= 0;
    }
}
