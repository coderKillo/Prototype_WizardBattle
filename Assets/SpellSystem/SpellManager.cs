using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
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

    private int currentSpellIndex = 0;
    public int CurrentSpellIndex { get { return currentSpellIndex; } }

    private Spell currentSpell = null;
    public Spell CurrentSpell { get { return currentSpell; } }

    MMFeedbacks feedback;

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

        feedback = GetComponent<MMFeedbacks>();
    }

    public void FirePrimary()
    {
        CastSpell(global::Slot.PRIMARY);
    }

    public void FirePrimaryCanceled()
    {
        if (currentSpell == null)
            return;

        currentSpell.CancelSpell();
    }

    public void FireSecondary()
    {
        CastSpell(global::Slot.SECONDARY);
    }

    public void FireSecondaryCanceled()
    {
        if (currentSpell == null)
            return;

        currentSpell.CancelSpellSecondary();
    }

    public void NextSlot()
    {
        var nextIndex = currentSpellIndex + 1;
        if (nextIndex >= spells.Length)
        {
            nextIndex = 0;
        }

        ChangeSlot(nextIndex);
    }

    public void PreviousSlot()
    {
        var previousIndex = currentSpellIndex - 1;
        if (previousIndex < 0)
        {
            previousIndex = spells.Length - 1;
        }

        ChangeSlot(previousIndex);
    }

    public void ChangeSlot(int index)
    {
        if (index >= spells.Length || index < 0)
            return;

        currentSpell = spells[index];
        currentSpellIndex = index;

        wand.SetGlowColor(currentSpell.Config.wandGlowColor);
    }

    private void CastSpell(Slot slot)
    {
        if (currentSpell == null)
            return;

        if (!currentSpell.IsUsable(slot))
            return;

        var spellAbility = slot == global::Slot.PRIMARY ? currentSpell.Config.primaryAbility : currentSpell.Config.secondaryAbility;

        currentSpell.TriggerCooldown(slot);
        animator.Play(spellAbility.animation);

        if (slot == global::Slot.PRIMARY)
        {
            currentSpell.Invoke("CastSpell", spellAbility.castDelay);
        }
        else
        {
            currentSpell.Invoke("CastSpellSecondary", spellAbility.castDelay);
        }

        feedback.Invoke("PlayFeedbacks", spellAbility.castDelay);
    }

    private void Update()
    {
        foreach (var spell in spells)
        {
            spell.HandleCooldown(Time.deltaTime);
        }
    }
}
