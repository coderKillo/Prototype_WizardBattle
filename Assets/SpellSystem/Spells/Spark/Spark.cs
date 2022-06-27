using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : Spell
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

    private bool freeze = false;
    private float speed = 0f;

    private void Awake()
    {
        speed = missileSpeed;
    }

    public override void CastSpell()
    {
        var missile = GameObject.Instantiate(missilePrefab, manager.SpellSource().position, Quaternion.identity, transform);
        missile.transform.LookAt(GetTarget());

        var projectile = missile.AddComponent<Projectile>();
        projectile.Speed = speed;
        projectile.HitMask = hitMask;
        projectile.OnDestroyMissile.AddListener(() => DestroyMissile(missile));
        projectile.OnHit.AddListener((hit) => MissileHit(missile, hit));

        Destroy(missile, missileLifetime);
    }

    public override void CastSpellSecondary()
    {
        freeze = !freeze;
        speed = freeze ? 0f : missileSpeed;

        foreach (Transform missile in transform)
        {
            var projectile = missile.GetComponent<Projectile>();
            if (projectile == null) return;

            projectile.Speed = speed;

            missile.transform.LookAt(GetTarget());
        }
    }

    private Vector3 GetTarget()
    {
        RaycastHit hit;
        Vector3 target;

        if (SpellHitTarget(out hit, hitMask))
        {
            target = hit.point;
        }
        else
        {
            var direction = manager.AimDirection();
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
        var projectile = missile.GetComponent<Projectile>();

        if (LayerMaskUtils.IsInLayerMask(hit.transform.gameObject, bounceMask))
        {
            if (projectile.BounceCount >= 3)
            {
                projectile.DestroyMissile();
            }
            else
            {
                projectile.Bounce(hit.normal);
            }
        }
        else
        {
            projectile.DestroyMissile();

            var enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.Damage(spellDamage);
            }
        }
    }
}