using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostSpike : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speed = 2000f;
    [SerializeField] private float spikeLifetime = 10f;
    [SerializeField] private int spellBaseDamage = 10;
    [SerializeField] private float spellHoldDamageFactor = 15f;
    [SerializeField] private float spellHoldScaleFactor = 0.2f;
    [SerializeField] private int spellMaxDamage = 100;
    [SerializeField] private LayerMask hitMask;

    private GameObject spike;
    private float spellDamage;
    private bool holdSpell = false;

    public override void CastSpell()
    {
        holdSpell = true;

        spike = GameObject.Instantiate(spikePrefab, manager.SpellSource().position, manager.AimDirection().rotation, transform);

        spellDamage = spellBaseDamage;
    }

    public override void CancelSpell()
    {
        holdSpell = false;

        if (spike == null)
            return;

        spike.transform.LookAt(GetTarget(hitMask));

        int damage = Mathf.Clamp((int)spellDamage, 0, spellMaxDamage);
        spellDamage = 0;

        var projectile = spike.AddComponent<Projectile>();
        projectile.Speed = speed;
        projectile.HitMask = hitMask;
        projectile.OnDestroyMissile.AddListener(() => OnDestroySpike(spike));
        projectile.OnHit.AddListener((hit) => OnSpikeHit(spike, hit, damage));

        Destroy(spike, spikeLifetime);
    }

    private void OnDestroySpike(GameObject missile)
    {
        if (missile == null)
            return;

        var explosion = GameObject.Instantiate(explosionPrefab, missile.transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }

    private void OnSpikeHit(GameObject missile, RaycastHit hit, int damage)
    {
        var projectile = missile.GetComponent<Projectile>();
        if (projectile)
        {
            projectile.DestroyMissile();
        }

        var enemyHealth = hit.transform.GetComponent<EnemyHealth>();
        if (enemyHealth)
        {
            enemyHealth.Damage(damage);

            if (enemyHealth.IsDead())
            {
                var rigidbody = hit.transform.GetComponent<Rigidbody>();
                if (rigidbody)
                {
                    rigidbody.AddForce(missile.transform.forward * 2000f);
                }
            }
        }
    }

    private void Update()
    {
        if (holdSpell)
        {
            spike.transform.position = manager.SpellSource().position;
            spike.transform.rotation = manager.AimDirection().rotation;

            if (spellDamage <= spellMaxDamage)
            {
                spike.transform.localScale += new Vector3(
                    spellHoldScaleFactor * Time.deltaTime,
                    spellHoldScaleFactor * Time.deltaTime,
                    spellHoldScaleFactor * Time.deltaTime
                );

                spellDamage += spellHoldDamageFactor * Time.deltaTime;
            }
        }
    }


}

