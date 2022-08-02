using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float caseDistance = 10f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackDistance = 2f;
    public float AttackDistance { get { return attackDistance; } }

    [Header("Patrolling")]
    [SerializeField] float patrolRadius = 5f;
    [SerializeField] float patrolSpeed = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private Collider enemyCollider;
    private Transform target;
    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;
    private bool isDead = false;

    private int animationAttack = Animator.StringToHash("attack");
    private int animationRun = Animator.StringToHash("run");
    private int animationMove = Animator.StringToHash("move");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider>();

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
            return;

        if (isProvoked)
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

        agent.isStopped = false;
        agent.speed = patrolSpeed;
        enemyCollider.enabled = true;
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

        agent.isStopped = true;
        enemyCollider.enabled = false;

        animator.Play("Dead");
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
        animator.SetBool(animationAttack, false);
        animator.SetTrigger(animationMove);
        animator.SetBool(animationRun, isProvoked);

        agent.speed = isProvoked ? speed : patrolSpeed;
        agent.SetDestination(targetPosition);
    }

    private void AttackTarget()
    {
        animator.SetBool(animationAttack, true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, caseDistance);
    }
}
