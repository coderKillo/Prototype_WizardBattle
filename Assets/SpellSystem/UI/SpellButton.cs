using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private SpellManager manager;
    [SerializeField] private Slot slot;

    private Slider cooldownSlider;
    private int index = -1;

    private void Start()
    {
        cooldownSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (SlotChanged())
        {
            index = manager.CurrentSpellIndex;

            icon.sprite = GetCurrentSpellAbility().icon;

            cooldownSlider.maxValue = GetCurrentSpellAbility().cooldown;
            cooldownSlider.value = GetCurrentSpellAbility().cooldown;
        }

        cooldownSlider.value = manager.CurrentSpell.CooldownTimer(slot);
    }

    private bool SlotChanged()
    {
        return manager.CurrentSpellIndex != index;
    }

    private SpellAbility GetCurrentSpellAbility()
    {
        return slot == Slot.PRIMARY ? manager.CurrentSpell.Config.primaryAbility : manager.CurrentSpell.Config.secondaryAbility;
    }
}
