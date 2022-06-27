using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private float forcePull = 800f;
    [SerializeField] private float upwardsForceScale = 0.3f;
    [SerializeField] private LayerMask mask;

    public override void CastSpell()
    {
        RaycastHit hit;
        if (!SpellHitTarget(out hit, mask)) return;
        var target = hit.transform.gameObject;

        var rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody == null) return;

        Vector3 forceDirection = Vector3.back + (upwardsForceScale * Vector3.up);
        rigidbody.AddForce(manager.AimDirection().TransformDirection(forceDirection) * forcePull);
    }

    // FIXME: Force Spell for new spells
}