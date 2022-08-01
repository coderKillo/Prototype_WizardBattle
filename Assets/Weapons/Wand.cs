using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] private Light light;

    private Renderer wandRenderer;

    private void Awake()
    {
        wandRenderer = GetComponent<Renderer>();
    }

    public void SetGlowColor(Color glowColor)
    {
        light.color = glowColor;

        wandRenderer.material.color = glowColor;
        wandRenderer.material.SetColor("_EmissionColor", glowColor);
    }
}
