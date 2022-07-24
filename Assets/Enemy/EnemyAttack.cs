using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage = 40f;

    private PlayerHealth target;
    private EnemyAI ai;

    private void Start()
    {
        target = GameObject.FindObjectOfType<PlayerHealth>();
        ai = GetComponent<EnemyAI>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;

        if (Vector3.Distance(target.transform.position, transform.position) <= ai.AttackDistance)
        {
            target.Damage(damage);
        }
    }
}
