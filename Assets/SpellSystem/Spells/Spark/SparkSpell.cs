using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spark")]
public class SparkSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private int spellDamage = 10;
    [SerializeField] private float missileLifetime = 10f;
    [SerializeField] private float missileSpeed = 1000f;
    [SerializeField] private LayerMask bounceMask;
    [SerializeField] private LayerMask hitMask;

    [Header("Prefabs")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject explosionPrefab;

    private GameObject missileContainer;
    private bool missileStop = false;
    private float missileInitialSpeed = 0f;

    public override void PrepareSpell(SpellManager manager)
    {
        missileContainer = new GameObject("Sparks");
        missileContainer.transform.parent = manager.SpellContainer();

        missileInitialSpeed = missileSpeed;
    }

    public override void CastSpell(SpellManager manager)
    {
        var missile = GameObject.Instantiate(missilePrefab, manager.SpellSource().position, Quaternion.identity, missileContainer.transform);
        missile.transform.LookAt(GetTarget());

        var projectile = missile.AddComponent<Projectile>();
        projectile.Speed = missileInitialSpeed;
        projectile.BounceMask = bounceMask;
        projectile.BounceCount = 3;
        projectile.OnDestroyMissile.AddListener(() => DestroyMissile(missile));
        projectile.OnHit.AddListener((hit) => MissileHit(missile, hit));

        Destroy(missile, missileLifetime);
    }

    public override void CastSpellSecondary(SpellManager manager)
    {
        missileStop = !missileStop;
        missileInitialSpeed = missileStop ? 0f : missileSpeed;

        foreach (Transform missile in missileContainer.transform)
        {
            var projectile = missile.GetComponent<Projectile>();
            if (projectile == null) return;

            projectile.Speed = missileInitialSpeed;

            missile.transform.LookAt(GetTarget());
        }
    }

    private Vector3 GetTarget()
    {
        RaycastHit hit;
        Vector3 target;

        if (SpellManager.Instance.SpellHitTarget(out hit))
        {
            target = hit.point;
        }
        else
        {
            var direction = SpellManager.Instance.AimDirection();
            target = direction.position + direction.forward * 1000f;
        }

        return target;
    }

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