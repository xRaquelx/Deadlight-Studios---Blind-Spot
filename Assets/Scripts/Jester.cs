using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    //Alert,
    Investigate,
    Chase,
    Attack,
}

public class Jester : MonoBehaviour
{
    [Header("Waypoint Info")]
    public float wanderRadius = 10f;
    public float waitTime = 2f;
    public float minRange = 5f;
    public float maxRange = 35f;
    public List<GameObject> waypoints = new List<GameObject>();

    [Header("Enemy Info")]
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float attackRange = 2f;

    private NavMeshAgent jester;

    private EnemyState currentState = EnemyState.Patrol;
    //private EnemyState previousState = EnemyState.Patrol;

    private float timer;

    private Vector3 soundPosition;

    // these are for the hearing sound events from the sound system script
    //private void OnEnable()
    //{
    //    SoundSystem.OnSoundMade += HearSound;
    //}

    //private void OnDisable()
    //{
    //    SoundSystem.OnSoundMade -= HearSound;
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jester = GetComponent<NavMeshAgent>();
        jester.speed = walkSpeed;
        timer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Investigate: Investigate(); break;
            //case EnemyState.Alert: Alert(); break;
            //case EnemyState.Chase: Chase(); break;
            //case EnemyState.Attack: Attack(); break;
        }
    }
    public void Patrol()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime || (!jester.pathPending && jester.remainingDistance <= jester.stoppingDistance))
        {
            Vector3 newPos = GetRandomNavMeshPosition(transform.position, wanderRadius);
            jester.SetDestination(newPos);
            timer = 0f;
        }

        //jester.isStopped = false;

        //// wait till path has finished calculating
        //if (jester.pathPending)
        //    return;

        //// if jester reached destination, set new point
        //if (jester.remainingDistance <= jester.stoppingDistance)
        //{
        //    SetNewPatrolDestination();
        //}
    }

    private Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas))
        {
            return navHit.position;
        }

        return origin;
    }

    private void SetNewPatrolDestination()
    {
        GameObject waypoint = GetRandomWaypoint();

        if (waypoint != null)
        {
            jester.SetDestination(waypoint.transform.position);
        }
    }

    private GameObject GetRandomWaypoint()
    {
        // if there are no waypoints, return nothing
        if (waypoints == null || waypoints.Count == 0) return null;

        for (int i = 0; i < 20; i++)
        {
            GameObject wp = waypoints[Random.Range(0, waypoints.Count)];

            float dist = Vector3.Distance(transform.position, wp.transform.position);

            // if the waypoint is within the max and min range, give it
            if (dist >= minRange && dist <= maxRange)
            {
                return wp;
            }
        }

        Debug.Log("No valid waypoint found");
        return null;
    }

    private void HearSound(Vector3 position, float radius)
    {
        float distance = Vector3.Distance(transform.position, position);

        if (distance > radius)
            return;

        // Ignore sounds while already investigating
        // or searching.
        if (currentState != EnemyState.Patrol)
            return;

        soundPosition = position;
        currentState = EnemyState.Investigate;

        jester.isStopped = false;
        jester.SetDestination(soundPosition);
    }

    private void Investigate()
    {
        jester.isStopped = false;

        if (jester.pathPending)
            return;

        if (jester.remainingDistance <= jester.stoppingDistance)
        {
            currentState = EnemyState.Patrol;
        }
    }


#if UNITY_EDITOR
    // Optional: Draw the wander radius in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
#endif
}
