using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class GameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headline;
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    static private GameMenu instance;
    static public GameMenu Instance { get { return instance; } }

    private TextMeshProUGUI playButtonText;
    private TextMeshProUGUI exitButtonText;
    private Canvas canvas;

    void Awake()
    {
        if (instance == null) instance = this;

        playButtonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
        exitButtonText = exitButton.GetComponentInChildren<TextMeshProUGUI>();
        canvas = GetComponent<Canvas>();
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    public void ShowGameStart()
    {
        EnableUI();

        headline.text = "Welcome to Wizard Battle!";
        playButtonText.text = "Play!";
        exitButtonText.text = "Exit";

        playButton.onClick.AddListener(() => { GameManager.Instance.StartLevel(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
    }

    public void ShowGameWon()
    {
        EnableUI();

        headline.text = "You Win!";
        playButtonText.text = "Again!";
        exitButtonText.text = "Im Done";

        playButton.onClick.AddListener(() => { GameManager.Instance.RestartLevel(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
    }

    public void ShowGameOver()
    {
        EnableUI();

        headline.text = "You Died!";
        playButtonText.text = "Again!";
        exitButtonText.text = "Im Done";

        playButton.onClick.AddListener(() => { GameManager.Instance.RestartLevel(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
    }

    private void EnableUI()
    {
        canvas.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
