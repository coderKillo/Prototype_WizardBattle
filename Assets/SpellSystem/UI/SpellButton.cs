using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    [SerializeField] private Image icon;

    private float cooldownTime;
    private bool isCooldown;

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
        if (cooldownTime <= 0)
        {
            isCooldown = false;
        }
    }
}
