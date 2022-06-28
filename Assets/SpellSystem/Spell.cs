using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    protected SpellConfig config;
    public SpellConfig Config { get { return config; } }

    protected SpellManager manager;

    private bool isUsable = true;
    public bool IsUsable { get { return isUsable; } }

    public void Init(SpellConfig config, SpellManager manager)
    {
        this.config = config;
        this.manager = manager;
    }

    public void Lock() { isUsable = false; }
    public void Unlock() { isUsable = true; }


    public virtual void PrepareSpell() { }
    public virtual void CastSpell() { }
    public virtual void CastSpellSecondary() { }


    public bool SpellHitTarget(out RaycastHit hit, LayerMask mask)
    {
        if (Physics.Raycast(manager.AimDirection().position, manager.AimDirection().TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        {
            return true;
        }

        return false;
    }

}
