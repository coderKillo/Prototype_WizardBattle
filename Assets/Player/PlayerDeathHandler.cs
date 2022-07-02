using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Canvas gameOverUI;

    void Start()
    {
        gameOverUI.enabled = false;
    }

    public void OnPlayerDeath()
    {
        gameOverUI.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
