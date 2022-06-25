using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Transform rayCastRef;
    [SerializeField] private Transform spellSourcePoint;
    [SerializeField] private Transform spellContainer;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Animator animator;
    [SerializeField] private Wand wand;

    [Header("Spells")]
    [SerializeField] private Spell[] slots;
    public Spell[] Slots { get { return slots; } }
    private Spell currentSpell = null;
    public Spell CurrentSpell { get { return currentSpell; } }

    static private SpellManager instance = null;
    static public SpellManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ChangeSlot(0);

        foreach (var spell in slots)
        {
            spell.Unlock();
            spell.PrepareSpell(this);
        }
    }

    private void OnFirePrimary(InputValue value)
    {
        StartCoroutine("CastSpell", global::Slot.PRIMARY);
    }

    private void OnFireSecondary(InputValue value)
    {
        StartCoroutine("CastSpell", global::Slot.SECONDARY);
    }

    private void OnSlot0(InputValue value)
    {
        ChangeSlot(0);
    }

    private void OnSlot1(InputValue value)
    {
        ChangeSlot(1);
    }

    public int SlotLength() { return slots.Length; }
    public Spell Slot(int index) { return slots[index]; }

    private void ChangeSlot(int index)
    {
        if (index >= slots.Length)
        {
            return;
        }

        currentSpell = slots[index];

        wand.SetGlowColor(currentSpell.wandGlowColor);

        SpellSlots.Instance.GetSlot(global::Slot.PRIMARY).SetIcon(currentSpell.primaryAbility.icon);
        SpellSlots.Instance.GetSlot(global::Slot.SECONDARY).SetIcon(currentSpell.secondaryAbility.icon);
    }

    private IEnumerator CastSpell(Slot button)
    {
        if (currentSpell == null)
        {
            yield break;
        }

        if (!currentSpell.IsUsable)
        {
            yield break;
        }

        currentSpell.Lock();

        var spellAbility = button == global::Slot.PRIMARY ? currentSpell.primaryAbility : currentSpell.secondaryAbility;

        animator.Play(spellAbility.animation);
        SpellSlots.Instance.GetSlot(button).TriggerCooldown(spellAbility.cooldown);
        yield return new WaitForSeconds(spellAbility.castDelay);

        if (button == global::Slot.PRIMARY)
        {
            currentSpell.CastSpell(this);
        }
        else
        {
            currentSpell.CastSpellSecondary(this);
        }

        yield return new WaitForSeconds(spellAbility.cooldown - spellAbility.castDelay);

        currentSpell.Unlock();
    }

    // TODO: use cylinder for Raycast
    public bool SpellHitTarget(out RaycastHit hit)
    {
        if (Physics.Raycast(rayCastRef.position, rayCastRef.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        {
            return true;
        }

        return false;
    }

    public Transform AimDirection() { return rayCastRef; }

    public Transform SpellSource() { return spellSourcePoint; }

    public Transform SpellContainer() { return spellContainer; }
}
