
using System.Collections;
using System.Diagnostics.Metrics;
using UnityEngine;
using UnityEngine.AI;

public class JesterAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Wander,
        Investigate,
        Attack,
        Kill
    }

    [Header("State")]
    public State state = State.Idle;

    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    public Animator animator;

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
        agent.acceleration = 12f;

        spawnAt = Time.time + spawnDelay;
        agent.isStopped = true;

        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Captured", false);
    }

    void Update()
    {
        if (!activated)
        {
            if (Time.time >= spawnAt)
                Activate();

            return;
        }
        if (!hasKilledPlayer && player != null)
        {
            float distance = Vector3.Distance(
                player.position,
                transform.position
            );

            if (distance <= playerDetectRadius)
            {
                Debug.Log("PLAYER DETECTED! DISTANCE: " + distance);

                state = State.Kill;
            }
        }
        switch (state)
        {
            //case State.Wander:
            //    DoWander();
            //    break;

            case State.Investigate:
                DoInvestigate();
                break;
            case State.Kill:
                KillPlayer();
                break;
            case State.Attack:
                DoAttack(); 
                break;
        }
    }

    public void Activate()
    {
        activated = true;
        //state = State.Wander;

        agent.speed = wanderSpeed;
        agent.isStopped = false;

        //PickWanderDestination();
    }

    void DoWander()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Captured", false);

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
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            if (distanceToPlayer <= playerDetectRadius)
            {
                state = State.Kill;
                return;
            }
        }

        if (isWaiting)
        {
            agent.isStopped = true;
            animator.SetBool("Run", false);
            return;
        }

        animator.SetBool("Attack", false);
        animator.SetBool("Captured", false);

        agent.speed = chaseSpeed;

        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            if (agent.velocity.sqrMagnitude > 0.1f) return;

            agent.isStopped = true;
            agent.ResetPath();

            Debug.Log("Jester reached sound position.");

            state = State.Attack;
            StartCoroutine(WaitBeforeWander());
            return;
        }
        
        agent.isStopped = false;
        animator.SetBool("Run", true);
    }

    void DoAttack()
    {
        agent.isStopped = true;

        animator.SetBool("Run", false);
        animator.SetBool("Attack", true);
        animator.SetBool("Captured", false);
    }

void KillPlayer()
{
    if (hasKilledPlayer) return;

    hasKilledPlayer = true;

    Debug.Log("KILL PLAYER CALLED!");

    agent.isStopped = true;
    agent.ResetPath();

    animator.SetBool("Run", false);
    animator.SetBool("Captured", true);

    Debug.Log("Captured: " + animator.GetBool("Captured"));
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


        if (hasKilledPlayer || state == State.Kill) return;

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

        animator.SetBool("Attack", false);
        
        //yield return new WaitForSeconds(waitBeforeWandering);

        currentSoundPriority = SoundPriority.Low;

        state = State.Wander;

        agent.speed = wanderSpeed;
        agent.isStopped = false;

        // Reset timer before choosing a destination
        wanderTimer = wanderChangeInterval;

        //PickWanderDestination();

        isWaiting = false;
    }
}
