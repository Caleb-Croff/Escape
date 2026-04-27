using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float killRange = 1f;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float waypointReachedDistance = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] chaseSounds;

    private NavMeshAgent _agent;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _wasChasing;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool chasing = distanceToPlayer <= detectionRange;

        if (chasing)
            Chase();
        else
            Patrol();

        _animator.SetBool("isChasing", chasing);

        if (chasing && !_wasChasing && chaseSounds.Length > 0)
        {
            _audioSource.clip = chaseSounds[Random.Range(0, chaseSounds.Length)];
            _audioSource.Play();
        }
        _wasChasing = chasing;

        if (distanceToPlayer <= killRange)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void Chase()
    {
        _agent.speed = chaseSpeed;
        _agent.SetDestination(player.position);
    }

    private void Patrol()
    {
        _agent.speed = patrolSpeed;

        if (_agent.remainingDistance <= waypointReachedDistance)
            SetRandomPatrolPoint();
    }

    private void SetRandomPatrolPoint()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRadius;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            _agent.SetDestination(hit.position);
    }
}
