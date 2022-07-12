using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 0f;
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private Slider slider;

    static public event Action OnDeath;

    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void Damage(float value)
    {
        health -= value;
        slider.value = health;

        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
