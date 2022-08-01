using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
using MoreMountains.Feedbacks;

public class Lightning : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private GameObject lightningBoltPrefab;
    [SerializeField] private float maxHitDistance = 25f;
    [SerializeField] private int damage = 50;
    [SerializeField] private float lightningDuration = 0.3f;
    [SerializeField] private LayerMask hitMask;

    [Header("Coin")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Vector3 coinTossForce;
    [SerializeField] private float coinChainLightningRadius = 5f;

    private MMFeedbacks feedbacks;

    private void Awake()
    {
        feedbacks = GetComponent<MMFeedbacks>();
    }

    public override void CastSpell()
    {
        RaycastHit hit;
        if (!Physics.Raycast(manager.AimDirection().position, manager.AimDirection().TransformDirection(Vector3.forward), out hit, maxHitDistance, hitMask))
            return;

        CreateLightningBolt(manager.SpellSource().gameObject, hit.collider.gameObject);

        feedbacks.PlayFeedbacks();

        if (hit.collider.GetComponent<Coin>() != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.collider.transform.position, coinChainLightningRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<EnemyHealth>() != null)
                {
                    CreateLightningBolt(hit.collider.gameObject, hitCollider.gameObject);
                }
            }
        }
    }

    private void CreateLightningBolt(GameObject source, GameObject target)
    {
        var lightningBolt = GameObject.Instantiate(lightningBoltPrefab, Vector3.zero, Quaternion.identity, transform);

        var lightningBoltScript = lightningBolt.GetComponent<LightningBoltScript>();
        lightningBoltScript.StartObject = source;
        lightningBoltScript.EndObject = target;

        Destroy(lightningBolt, lightningDuration);

        DamageEnemy(target);
    }


    private void DamageEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;

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
