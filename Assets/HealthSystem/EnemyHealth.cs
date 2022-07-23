using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitpoints = 5;
    [SerializeField] private Slider slider;

    static public event Action OnSpawn;
    static public event Action OnDeath;

    private int m_currentHitpoints = 0;
    public int Hitpoints { get { return m_currentHitpoints; } }

    void OnEnable()
    {
        m_currentHitpoints = maxHitpoints;
        slider.maxValue = maxHitpoints;
        slider.value = maxHitpoints;

        OnSpawn?.Invoke();
    }

    public void Damage(int damage)
    {
        BroadcastMessage("OnDamageTaken");

        DamagePopup.Create(transform.position, damage);

        m_currentHitpoints -= damage;

        slider.value = m_currentHitpoints;

        if (IsDead())
        {
            BroadcastMessage("OnDeath");
            OnDeath?.Invoke();
        }
    }

    public bool IsDead() => m_currentHitpoints <= 0;
}

