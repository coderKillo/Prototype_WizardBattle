using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float caseDistance = 10f;
    [SerializeField] private float attackDistance = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;
    private bool isDead = false;

    [Header("Patrolling")]
    private float patrolRadius = 5f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isDead)
        {

        }
        else if (isProvoked)
        {
            if (IsAttackRange())
            {
                AttackTarget();
            }
            else
            {
                CaseTarget(target.position);
            }
        }
        else if (IsCaseRange())
        {
            isProvoked = true;
        }
        else if (!IsPatrolling())
        {
            CaseTarget(GetRandomPosition(patrolRadius));
        }
    }

    private bool IsPatrolling()
    {
        if (isProvoked)
            return false;

        if (agent.velocity.magnitude < 0.1f)
            return false;

        return true;
    }

    private void OnEnable()
    {
        isProvoked = false;
        isDead = false;
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

    private void OnDeath()
    {
        isDead = true;
        animator.Play("Dead");
        Invoke(nameof(DisableEnemy), 1f);
    }

    private void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    private Vector3 GetRandomPosition(float radius)
    {
        var pos = transform.position;
        pos.x += Random.Range(-radius, radius);
        pos.z += Random.Range(-radius, radius);
        return pos;
    }

    private void CaseTarget(Vector3 targetPosition)
    {
        animator.SetBool("attack", false);
        animator.SetTrigger("move");

        agent.SetDestination(targetPosition);
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
