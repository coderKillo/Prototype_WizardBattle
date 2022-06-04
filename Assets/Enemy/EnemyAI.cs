using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float caseDistance = 10f;
    [SerializeField] private float attackDistance = 2f;

    private NavMeshAgent agent;
    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            if (IsAttackRange())
            {
                AttackTarget();
            }
            else
            {
                CaseTarget();
            }
        }
        else if (IsCaseRange())
        {
            isProvoked = true;
        }
    }

    private bool IsCaseRange()
    {
        return distanceToTarget < caseDistance;
    }

    private bool IsAttackRange()
    {
        return distanceToTarget < attackDistance;
    }

    private void CaseTarget()
    {
        agent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        Debug.Log("Attack");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, caseDistance);
    }
}
