using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    protected SpellConfig config;
    public SpellConfig Config { get { return config; } }

    protected SpellManager manager;

    private float[] cooldownTimer = new float[(int)global::Slot.MAX];
    private float[] cooldown = new float[(int)global::Slot.MAX];

    public void Init(SpellConfig config, SpellManager manager)
    {
        this.config = config;
        this.manager = manager;

        cooldown[(int)Slot.PRIMARY] = config.primaryAbility.cooldown;
        cooldown[(int)Slot.SECONDARY] = config.secondaryAbility.cooldown;
    }

    public void TriggerCooldown(Slot slot) { cooldownTimer[(int)slot] = cooldown[(int)slot]; }
    public float CooldownTimer(Slot slot) { return cooldownTimer[(int)slot]; }
    public void HandleCooldown(float deltaTime)
    {
        for (int i = 0; i < cooldownTimer.Length; i++)
        {
            if (cooldownTimer[i] > 0)
            {
                cooldownTimer[i] -= deltaTime;
            }
        }
    }

    public bool IsUsable(Slot slot) { return cooldownTimer[(int)slot] <= 0; }


    public virtual void PrepareSpell() { }
    public virtual void CastSpell() { }
    public virtual void CastSpellSecondary() { }
    public virtual void CancelSpell() { }
    public virtual void CancelSpellSecondary() { }


    public bool SpellHitTarget(out RaycastHit hit, LayerMask mask)
    {
        return Physics.Raycast(manager.AimDirection().position, manager.AimDirection().TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask);
    }

}
