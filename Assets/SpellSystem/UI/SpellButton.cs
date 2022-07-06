using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    [SerializeField] private Image icon;

    private Slider cooldownSlider;
    private float cooldownTime;
    private bool isCooldown;

    private void Start()
    {
        cooldownSlider = GetComponent<Slider>();
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void TriggerCooldown(float cooldown)
    {
        if (isCooldown)
        {
            return;
        }

        cooldownTime = cooldown;
        isCooldown = true;

        cooldownSlider.maxValue = cooldown;
        cooldownSlider.value = cooldown;
    }

    private void Update()
    {
        HandleCooldown();
    }

    private void HandleCooldown()
    {
        if (!isCooldown)
        {
            return;
        }

        cooldownTime -= Time.deltaTime;
        cooldownSlider.value = cooldownTime;

        if (cooldownTime <= 0)
        {
            isCooldown = false;
        }
    }
}
