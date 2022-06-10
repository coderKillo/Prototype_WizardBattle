using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/ForcePullSpell")]
public class ForcePullSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private float force = 2000f;
    [SerializeField] private float upwardsForceScale = 1f;

    public override void CastSpell(SpellManager manager)
    {
        var target = manager.SpellHitTarget();
        if (target == null) return;

        var rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody == null) return;

        Vector3 forceDirection = Vector3.back + (upwardsForceScale * Vector3.up);
        rigidbody.AddForce(manager.AimDirection().TransformDirection(forceDirection) * force);
    }
}