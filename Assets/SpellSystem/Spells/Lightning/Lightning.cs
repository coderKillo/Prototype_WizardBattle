using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Lightning : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private GameObject lightningBoltPrefab;
    [SerializeField] private float maxHitDistance = 1000f;
    [SerializeField] private float lightningDuration = 0.5f;
    [SerializeField] private LayerMask hitMask;

    public override void CastSpell()
    {
        RaycastHit hit;
        if (!Physics.Raycast(manager.AimDirection().position, manager.AimDirection().TransformDirection(Vector3.forward), out hit, maxHitDistance, hitMask))
            return;

        // TODO: fix visuals
        // TODO: fix position
        var lightningBolt = GameObject.Instantiate(lightningBoltPrefab, Vector3.zero, Quaternion.identity);

        var lightningBoltScript = lightningBolt.GetComponent<LightningBoltScript>();
        lightningBoltScript.StartPosition = manager.SpellSource().position;
        lightningBoltScript.EndPosition = hit.point;

        Destroy(lightningBolt, lightningDuration);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
