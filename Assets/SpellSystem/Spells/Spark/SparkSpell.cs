using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spark")]
public class SparkSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float missileSpeed = 1000f;

    [Header("Prefabs")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject explosionPrefab;

    public override void CastSpell(SpellManager manager)
    {
        RaycastHit hit;
        Vector3 target;

        if (manager.SpellHitTarget(out hit))
        {
            target = hit.point;
        }
        else
        {
            var direction = manager.AimDirection();
            target = direction.position + direction.forward * 1000f;
        }

        var missile = GameObject.Instantiate(missilePrefab, manager.SpellSource().position, Quaternion.identity);
        missile.transform.LookAt(target);

        var projectile = missile.AddComponent<Projectile>();
        projectile.Speed = missileSpeed;
    }

    public override void CastSpellSecondary(SpellManager manager) { }
}