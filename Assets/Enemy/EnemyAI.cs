using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float caseDistance = 10f;
    [SerializeField] private float attackDistance = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = attackDistance;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
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

    private void OnEnable()
    {
        isProvoked = false;
    }

    private bool IsCaseRange()
    {
        return distanceToTarget < caseDistance;
    }

    private bool IsAttackRange()
    {
        return distanceToTarget < attackDistance;
    }

    private void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void CaseTarget()
    {
        animator.SetBool("attack", false);
        animator.SetTrigger("move");

        agent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        animator.SetBool("attack", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, caseDistance);
    }
}
