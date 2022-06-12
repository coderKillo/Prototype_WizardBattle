using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void SetGlowColor(Color glowColor)
    {
        renderer.material.color = glowColor;
        renderer.material.SetColor("_EmissionColor", glowColor);
    }
}
