using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spark")]
public class SparkSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private int spellDamage = 10;
    [SerializeField] private float missileSpeed = 1000f;
    [SerializeField] private LayerMask bounceMask;
    [SerializeField] private LayerMask hitMask;

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
        projectile.BounceMask = bounceMask;
        projectile.BounceCount = 3;
        projectile.OnDestroyMissile.AddListener(() => DestroyMissile(missile));
        projectile.OnHit.AddListener((hit) => MissileHit(missile, hit));

        Destroy(projectile, 10f);
    }

    public override void CastSpellSecondary(SpellManager manager) { }

    private void DestroyMissile(GameObject missile)
    {
        var explosion = GameObject.Instantiate(explosionPrefab, missile.transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }

    private void MissileHit(GameObject missile, RaycastHit hit)
    {
        if (LayerMaskUtils.IsInLayerMask(hit.transform.gameObject, hitMask))
        {
            var projectile = missile.GetComponent<Projectile>();
            if (projectile)
            {
                projectile.DestroyMissile();
            }

            var enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.Damage(spellDamage);
            }
        }
    }
}