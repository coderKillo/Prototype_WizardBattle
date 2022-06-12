using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    private Renderer wandRenderer;

    private void Awake()
    {
        wandRenderer = GetComponent<Renderer>();
    }

    public void SetGlowColor(Color glowColor)
    {
        wandRenderer.material.color = glowColor;
        wandRenderer.material.SetColor("_EmissionColor", glowColor);
    }
}
