using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellAbility
{
    [Header("General")]
    public float cooldown;

    [Header("UI")]
    public Sprite icon;

    [Header("Animation")]
    public String animation;
    public float castDelay;
}

public class Spell : ScriptableObject
{
    [Header("General")]
    [ColorUsage(true, true)]
    public Color wandGlowColor;
    private bool isUsable = true;
    public bool IsUsable { get { return isUsable; } }

    [Header("Primary Ability")]
    public SpellAbility primaryAbility;

    [Header("Secondary Ability")]
    public SpellAbility secondaryAbility;


    public virtual void PrepareSpell(SpellManager manager) { }
    public virtual void CastSpell(SpellManager manager) { }
    public virtual void CastSpellSecondary(SpellManager manager) { }

    public void Lock() { isUsable = false; }
    public void Unlock() { isUsable = true; }
}
