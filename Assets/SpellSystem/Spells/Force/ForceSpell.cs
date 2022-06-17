using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Force")]
public class ForceSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private float forcePull = 800f;
    [SerializeField] private float upwardsForceScale = 0.3f;
    [SerializeField] private LayerMask mask;

    public override void CastSpell(SpellManager manager)
    {
        RaycastHit hit;
        if (!manager.SpellHitTarget(out hit)) return;
        var target = hit.transform.gameObject;

        if (!LayerMaskUtils.IsInLayerMask(target, mask)) return;

        var rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody == null) return;

        Vector3 forceDirection = Vector3.back + (upwardsForceScale * Vector3.up);
        rigidbody.AddForce(manager.AimDirection().TransformDirection(forceDirection) * forcePull);
    }
}