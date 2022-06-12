using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : ScriptableObject
{
    [Header("General")]
    public float cooldown;
    private bool isUsable = true;
    public bool IsUsable { get { return isUsable; } }

    [Header("UI")]
    public Sprite icon;

    [Header("Animation")]
    public String animation;
    public float castDelay;
    [ColorUsage(true, true)]
    public Color wandGlowColor;

    public virtual void CastSpell(SpellManager manager) { }

    public void Lock() { isUsable = false; }
    public void Unlock() { isUsable = true; }
}
