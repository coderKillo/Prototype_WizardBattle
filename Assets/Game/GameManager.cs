using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    Init,
    Play,
    GameOver,
    PlayerWon
}

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }

    static public event Action<GameState> OnGameStateChange;

    [SerializeField] private Canvas gameMenu;
    [SerializeField] private TextMeshProUGUI gameMenuHeadline;

    [SerializeField] private Canvas playerUI;

    private GameState currentState;
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
        PlayerHealth.OnDeath += PlayerDied;

        StartLevel();
    }

    void Update()
    {
        if (ScoreManager.Instance.Score <= 0)
        {
            GameWon();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    ////////////////////////////////////////////////////////////////////////////////

    private void StartLevel()
    {
        SpawnEnemies();
        ChangeState(GameState.Play);
    }

    private void PlayerDied()
    {
        ChangeState(GameState.GameOver);
    }

    private void GameWon()
    {
        ChangeState(GameState.PlayerWon);
    }

    ////////////////////////////////////////////////////////////////////////////////

    private void ChangeState(GameState state)
    {
        if (currentState == state)
            return;

        switch (state)
        {
            case GameState.Play:
                Show(playerUI, false);
                break;
            case GameState.GameOver:
                gameMenuHeadline.text = "You Died!";
                Show(gameMenu);
                break;
            case GameState.PlayerWon:
                gameMenuHeadline.text = "You Won!";
                Show(gameMenu);
                break;
            default:
                break;
        }

        currentState = state;
        OnGameStateChange?.Invoke(currentState);
    }

    private static void SpawnEnemies()
    {
        foreach (var enemyPool in GameObject.FindObjectsOfType<EnemyPool>())
        {
            enemyPool.StartSpawn();
        }
    }

    private void Show(Canvas ui, bool enableCursor = true)
    {
        playerUI.enabled = false;
        gameMenu.enabled = false;

        ui.enabled = true;

        Cursor.lockState = enableCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enableCursor;
    }

}
