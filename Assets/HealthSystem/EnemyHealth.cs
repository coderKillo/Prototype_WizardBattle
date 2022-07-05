using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitpoints = 5;
    [SerializeField] private Slider slider;

    private int m_currentHitpoints = 0;

    void OnEnable()
    {
        m_currentHitpoints = maxHitpoints;
        slider.maxValue = maxHitpoints;
        slider.value = maxHitpoints;
    }

    public void Damage(int damage)
    {
        BroadcastMessage("OnDamageTaken");

        DamagePopup.Create(transform.position, damage);

        m_currentHitpoints -= damage;

        slider.value = m_currentHitpoints;

        if (m_currentHitpoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

