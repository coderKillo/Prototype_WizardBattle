using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headline;
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    static private GameMenu instance;
    static public GameMenu Instance { get { return instance; } }

    private TextMeshProUGUI playButtonText;
    private TextMeshProUGUI exitButtonText;

    void Awake()
    {
        if (instance == null) instance = this;

        playButtonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
        exitButtonText = exitButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowGameStart()
    {
        headline.text = "Welcome to Wizard Battle!";
        playButtonText.text = "Play!";
        exitButtonText.text = "Exit";

        playButton.onClick.AddListener(() => { GameManager.Instance.StartLevel(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
    }

    public void ShowGameWon()
    {
        headline.text = "You Win!";
        playButtonText.text = "Again!";
        exitButtonText.text = "Im Done";

        playButton.onClick.AddListener(() => { GameManager.Instance.RestartLevel(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
    }

    public void ShowGameOver()
    {
        headline.text = "You Died!";
        playButtonText.text = "Again!";
        exitButtonText.text = "Im Done";

        playButton.onClick.AddListener(() => { GameManager.Instance.RestartLevel(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.ExitGame(); });
    }
}
