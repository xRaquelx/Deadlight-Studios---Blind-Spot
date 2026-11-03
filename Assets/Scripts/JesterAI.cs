
using System.Collections;
using System.Diagnostics.Metrics;
using UnityEngine;
using UnityEngine.AI;

public class JesterAI : MonoBehaviour
{
    public enum State
    {
        Inactive,
        Wander,
        Investigate,
        Kill
    }

    [Header("State")]
    public State state = State.Inactive;

    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;

    [Header("Movement")]
    public float wanderSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float attackRadius = 5f;

    [Header("Wandering")]
    public float wanderRadius = 12f;
    public float wanderChangeInterval = 10f;
    private float wanderTimer;

    [Header("Investigation")]
    public float playerDetectRadius = 3f;
    public float soundStoppingDistance = 5f;

    [Header("Spawning")]
    public float spawnDelay = 3f;
    private float spawnAt;

    [Header("Attacking")]
    private bool activated; 
    private bool hasKilledPlayer;

    private Vector3 soundPosition;
    private SoundPriority currentSoundPriority = SoundPriority.Low;

    [Header("Waiting")]
    public float waitBeforeWandering = 5f;
    private bool isWaiting;



    // these are for the hearing sound events from the sound system script
    private void OnEnable()
    {
        SoundSystem.OnSoundMade += HearSound;
    }

    private void OnDisable()
    {
        SoundSystem.OnSoundMade -= HearSound;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = wanderSpeed;
        agent.stoppingDistance = soundStoppingDistance;

        spawnAt = Time.time + spawnDelay;
        agent.isStopped = true;
    }

    void Update()
    {
        if (!activated)
        {
            if (Time.time >= spawnAt)
                Activate();

            return;
        }

        switch (state)
        {
            case State.Wander:
                DoWander();
                break;

            case State.Investigate:
                DoInvestigate();
                break;

            case State.Kill:
                DoAttack();
                break;
        }
    }

    public void Activate()
    {
        activated = true;
        state = State.Wander;

        agent.speed = wanderSpeed;
        agent.isStopped = false;

        PickWanderDestination();
    }

    void DoWander()
    {
        agent.speed = wanderSpeed;
        agent.isStopped = false;

        wanderTimer -= Time.deltaTime;

        if (agent.pathPending) return;

        if (wanderTimer <= 0f || agent.remainingDistance <= agent.stoppingDistance)
        {
            PickWanderDestination();
        }
    }

    void DoInvestigate()
    {
        agent.isStopped = false;
        agent.speed = chaseSpeed;

        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            if (agent.velocity.sqrMagnitude > 0.1f) return;

            if (isWaiting) return;

            agent.ResetPath();
            agent.isStopped = true;

            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(player.position, soundPosition);

                if (distanceToPlayer <= playerDetectRadius)
                {
                    state = State.Kill;
                    return;
                }
            }

            StartCoroutine(WaitBeforeWander());
        }
    }

    void DoAttack()
    {
        if (player == null)
        {
            state = State.Wander;
            agent.isStopped = false;
            return;
        }

        agent.isStopped = true;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasKilledPlayer)
        {
            hasKilledPlayer = true;
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        Debug.Log("DEADDDDD");

        // start death logic
    }

    void PickWanderDestination()
    {
        wanderTimer = wanderChangeInterval;

        Vector3 randomPoint = transform.position + new Vector3(
            Random.Range(-wanderRadius, wanderRadius),
            0f,
            Random.Range(-wanderRadius, wanderRadius)
        );

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomPoint,
            out hit,
            wanderRadius,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


    private void HearSound(Vector3 position, float radius, SoundPriority priority)
    {
        if (!activated)
            return;

        float distance = Vector3.Distance(transform.position, position);


        if (state == State.Kill) return;

        // prioritizing
        // basically if jester is already investigating a sound, it'll only change if the sound is higher
        if (state == State.Investigate && priority < currentSoundPriority) return;

        if (distance > radius) return;

        soundPosition = position;
        currentSoundPriority = priority;

        state = State.Investigate;

        agent.speed = chaseSpeed;
        agent.stoppingDistance = soundStoppingDistance;
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(soundPosition);
    }

    IEnumerator WaitBeforeWander()
    {
        isWaiting = true;

        agent.isStopped = true;

        yield return new WaitForSeconds(waitBeforeWandering);

        currentSoundPriority = SoundPriority.Low;

        state = State.Wander;

        agent.speed = wanderSpeed;
        agent.isStopped = false;

        // Reset timer before choosing a destination
        wanderTimer = wanderChangeInterval;

        PickWanderDestination();

        isWaiting = false;
    }
}
