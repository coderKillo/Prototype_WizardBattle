using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Transform rayCastRef;
    public Transform AimDirection() { return rayCastRef; }

    [SerializeField] private Transform spellSourcePoint;
    public Transform SpellSource() { return spellSourcePoint; }

    [SerializeField] private Animator animator;
    [SerializeField] private Wand wand;

    [Header("Spells")]
    [SerializeField] private SpellConfig[] spellConfigs;
    private Spell[] spells;

    private Spell currentSpell = null;
    public Spell CurrentSpell { get { return currentSpell; } }

    private void Awake()
    {
        spells = new Spell[spellConfigs.Length];

        for (int i = 0; i < spellConfigs.Length; i++)
        {
            var config = spellConfigs[i];
            var spellObject = GameObject.Instantiate(config.spell, Vector3.zero, Quaternion.identity, transform);
            var spell = spellObject.GetComponent<Spell>();

            spell.Init(config, this);
            spells[i] = spell;
        }
    }

    private void Start()
    {
        for (int i = 0; i < spellConfigs.Length; i++)
        {
            SpellSlots.Instance.SetSlotColor(spellConfigs[i].wandGlowColor, i);
        }
    }

    public void FirePrimary()
    {
        StartCoroutine("CastSpell", global::Slot.PRIMARY);
    }

    public void FirePrimaryCanceled()
    {
        if (currentSpell == null) return;
        currentSpell.CancelSpell();
    }

    public void FireSecondary()
    {
        StartCoroutine("CastSpell", global::Slot.SECONDARY);
    }

    public void FireSecondaryCanceled()
    {
        if (currentSpell == null) return;
        currentSpell.CancelSpellSecondary();
    }

    public void ChangeSlot(int index)
    {
        if (index >= spells.Length)
        {
            return;
        }

        currentSpell = spells[index];

        wand.SetGlowColor(currentSpell.Config.wandGlowColor);

        SpellSlots.Instance.GetButton(global::Slot.PRIMARY).SetIcon(currentSpell.Config.primaryAbility.icon);
        SpellSlots.Instance.GetButton(global::Slot.SECONDARY).SetIcon(currentSpell.Config.secondaryAbility.icon);
    }

    private IEnumerator CastSpell(Slot slot)
    {
        if (currentSpell == null)
        {
            yield break;
        }

        if (!currentSpell.IsUsable(slot))
        {
            yield break;
        }

        currentSpell.Lock(slot);

        var spellAbility = slot == global::Slot.PRIMARY ? currentSpell.Config.primaryAbility : currentSpell.Config.secondaryAbility;

        animator.Play(spellAbility.animation);
        SpellSlots.Instance.GetButton(slot).TriggerCooldown(spellAbility.cooldown);
        yield return new WaitForSeconds(spellAbility.castDelay);

        if (slot == global::Slot.PRIMARY)
        {
            currentSpell.CastSpell();
        }
        else
        {
            currentSpell.CastSpellSecondary();
        }

        yield return new WaitForSeconds(spellAbility.cooldown - spellAbility.castDelay);

        currentSpell.Unlock(slot);
    }
}
