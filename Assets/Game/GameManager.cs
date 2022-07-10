using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }

    private int enemyCounter = 0;
    private int currentSceneIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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

    public void EnemySpawned(GameObject enemy)
    {
        enemyCounter++;
        PlayerScoreUI.Instance.SetScore(enemyCounter);
    }

    public void EnemyDied(GameObject enemy)
    {
        enemyCounter--;
        PlayerScoreUI.Instance.SetScore(enemyCounter);
    }

    public void PlayerDied()
    {
        GameMenu.Instance.ShowGameOver();
        PlayerUI.Instance.Hide();
    }

    public void StartLevel()
    {
        SpawnEnemies();
        GameMenu.Instance.Hide();
        PlayerUI.Instance.Show();
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
        GameMenu.Instance.ShowGameWon();
        PlayerUI.Instance.Hide();
    }

    private bool CheckWinCondition()
    {
        return enemyCounter <= 0;
    }
}
