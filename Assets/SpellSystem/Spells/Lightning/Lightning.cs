using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Lightning : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private GameObject lightningBoltPrefab;
    [SerializeField] private float maxHitDistance = 1000f;
    [SerializeField] private int damage = 50;
    [SerializeField] private float lightningDuration = 0.3f;
    [SerializeField] private LayerMask hitMask;

    [Header("Coin")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Vector3 coinTossForce;

    public override void CastSpell()
    {
        RaycastHit hit;
        if (!Physics.Raycast(manager.AimDirection().position, manager.AimDirection().TransformDirection(Vector3.forward), out hit, maxHitDistance, hitMask))
            return;

        var lightningBolt = GameObject.Instantiate(lightningBoltPrefab, Vector3.zero, Quaternion.identity, transform);

        var lightningBoltScript = lightningBolt.GetComponent<LightningBoltScript>();
        lightningBoltScript.StartObject = manager.SpellSource().gameObject;
        lightningBoltScript.EndObject = hit.collider.gameObject;

        Destroy(lightningBolt, lightningDuration);

        StartCoroutine(nameof(DamageEnemy), hit.collider.gameObject);
    }

    private IEnumerator DamageEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(config.primaryAbility.castDelay);

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth)
        {
            enemyHealth.Damage(damage);
        }
    }

    public override void CastSpellSecondary()
    {
        var coin = GameObject.Instantiate(coinPrefab, manager.SpellSource().position, manager.SpellSource().rotation);

        var coinRigidbody = coin.GetComponent<Rigidbody>();
        coinRigidbody.AddRelativeForce(coinTossForce);
    }

}
