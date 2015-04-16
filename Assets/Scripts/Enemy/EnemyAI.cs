using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // Declaration of public variables
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    public float chaseWaitTime = 5.0f;
    public float patrolWaitTime = 1.0f;
    public Transform[] patrolWayPoints;

    // Declaration of private reference veraibles
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private PlayerHealth playerHealth;
    private LastPlayerSighting lastPlayerSighting;

    // Declaration of private misc variables
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public void Update()
    {
        if(enemySight.playerInSight && playerHealth.health > 0.0f)
        {
            Shooting();
        }
        else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0.0f)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    void Shooting()
    {
        nav.Stop();
    }

    void Chasing()
    {
        Vector3 sightingDeltaPos = enemySight.personalLastSighting = transform.position;
        if(sightingDeltaPos.sqrMagnitude > 4.0f)
        {
            nav.destination = enemySight.personalLastSighting;
        }

        nav.speed = chaseSpeed;

        if(nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if(chaseTimer > chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0.0f;
            }
        }
        else
        {
            chaseTimer = 0.0f;
        }
    }

    void Patroling()
    {
        if(nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if(patrolTimer >= patrolWaitTime)
            {
                if(wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }

                patrolTimer = 0.0f;
            }
        }
        else
        {
            patrolTimer = 0.0f;
        }

        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}
