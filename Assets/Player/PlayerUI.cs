using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class PlayerUI : MonoBehaviour
{
    static private PlayerUI instance;
    static public PlayerUI Instance { get { return instance; } }

    private Canvas canvas;

    void Awake()
    {
        if (instance == null) instance = this;

        canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
}
