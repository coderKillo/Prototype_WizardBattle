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

[CreateAssetMenu(menuName = "Spells/create Spell")]
public class SpellConfig : ScriptableObject
{
    [Header("General")]
    public String spellName;
    [ColorUsage(true, true)] public Color wandGlowColor;
    public Spell spell;

    [Header("Primary Ability")]
    public SpellAbility primaryAbility;

    [Header("Secondary Ability")]
    public SpellAbility secondaryAbility;
}
