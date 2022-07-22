using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostSpike : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask hitMask;

    private GameObject spike;
    private bool spikeCasted = false;

    public override void CastSpell()
    {
        spike = GameObject.Instantiate(spikePrefab, manager.SpellSource().position, manager.AimDirection().rotation, transform);
    }

    public override void CancelSpell()
    {
        var projectile = spike.AddComponent<Projectile>();
        projectile.Speed = speed;
        projectile.HitMask = hitMask;
        projectile.OnDestroyMissile.AddListener(() => OnDestroySpike(spike));

        spike = null;
    }

    private void OnDestroySpike(GameObject missile)
    {
    }

    private void Update()
    {
        if (spike != null)
        {
            spike.transform.position = manager.SpellSource().position;
            spike.transform.rotation = manager.AimDirection().rotation;
        }
    }


}

