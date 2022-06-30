using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private PlayerHealth target;
    [SerializeField] private float damage = 40f;
    [SerializeField] private float attackDistance = 2f;

    void Start()
    {

    }

    public void AttackHitEvent()
    {
        if (target == null) return;

        if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
        {
            target.Damage(damage);
        }
    }
}
