using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Transform rayCastRef;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Animator animator;

    [Header("Spells")]
    [SerializeField] private Spell[] slots;
    public Spell[] Slots { get { return slots; } }

    private void OnFire(InputValue value)
    {
        StartCoroutine("CastSpell", 0);
    }

    public int SlotLength() { return slots.Length; }
    public Spell Slot(int index) { return slots[index]; }

    private IEnumerator CastSpell(int index)
    {
        if (index >= slots.Length)
        {
            yield return null;
        }

        var spell = slots[index];

        if (!spell.IsUsable)
        {
            yield return null;
        }

        spell.Lock();

        animator.Play(spell.animation);
        yield return new WaitForSeconds(spell.castDelay);

        // TODO: wand glow

        spell.CastSpell(this);

        yield return new WaitForSeconds(spell.cooldown - spell.castDelay);

        spell.Unlock();
    }

    // TODO: use cylinder for Raycast
    public Transform SpellHitTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayCastRef.position, rayCastRef.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        {
            return hit.transform;
        }

        return null;
    }

    public Transform AimDirection() { return rayCastRef; }
}
