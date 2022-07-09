using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas menuUI;
    [SerializeField] private Canvas playerUI;

    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }

    public int enemyCounter = 0;

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
        SpawnEnemies();
        ShowStartMenu();
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
    }

    public void EnemyDied(GameObject enemy)
    {
        enemyCounter--;
    }

    public void PlayerDied()
    {
        SwitchUI(menuUI);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void StartLevel()
    {

    }

    public void RestartLevel()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void ShowStartMenu()
    {
        SwitchUI(menuUI);
    }

    private static void SpawnEnemies()
    {
        foreach (var enemyPool in GameObject.FindObjectsOfType<EnemyPool>())
        {
            enemyPool.StartSpawn();
        }
    }

    private void SwitchUI(Canvas ui, bool cursorEnabled = true)
    {
        menuUI.enabled = false;
        playerUI.enabled = false;

        ui.enabled = true;

        Cursor.lockState = cursorEnabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = cursorEnabled;
    }

    private void GameWon()
    {
        throw new NotImplementedException();
    }

    private bool CheckWinCondition()
    {
        return enemyCounter <= 0;
    }
}
