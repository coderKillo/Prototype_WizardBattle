using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    protected SpellConfig config;
    public SpellConfig Config { get { return config; } }

    protected SpellManager manager;

    private bool[] isOnCooldown = new bool[(int)global::Slot.MAX];

    public void Init(SpellConfig config, SpellManager manager)
    {
        this.config = config;
        this.manager = manager;
    }

    public void Lock(Slot slot) { isOnCooldown[(int)slot] = true; }
    public void Unlock(Slot slot) { isOnCooldown[(int)slot] = false; }
    public bool IsUsable(Slot slot) { return !isOnCooldown[(int)slot]; }


    public virtual void PrepareSpell() { }
    public virtual void CastSpell() { }
    public virtual void CastSpellSecondary() { }
    public virtual void CancelSpell() { }
    public virtual void CancelSpellSecondary() { }


    public bool SpellHitTarget(out RaycastHit hit, LayerMask mask)
    {
        if (Physics.Raycast(manager.AimDirection().position, manager.AimDirection().TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        {
            return true;
        }

        return false;
    }

}
