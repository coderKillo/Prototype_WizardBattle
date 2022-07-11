using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }

    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Events")]
    [SerializeField] private GameEvent levelWonEvent;
    [SerializeField] private GameEvent startLevelEvent;

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
        //TODO: find solution cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        scoreText.text = enemyCounter.ToString();
    }

    public void EnemyDied()
    {
        enemyCounter--;
        scoreText.text = enemyCounter.ToString();
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
