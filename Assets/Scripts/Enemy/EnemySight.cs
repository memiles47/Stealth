using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
    // Declaration of public variables
    public float fieldOfViewAngle = 110.0f;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    // Declaration of private reference variables
    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private LastPlayerSighting lastPlayerSighting;
    private GameObject player;
    private Animator playerAnim;
    private PlayerHealth playerHealth;
    private HashIDs hash;

    // Declaration of private misc variables
    private Vector3 previousSighting;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        // Initialize reference variables
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        lastPlayerSighting = GameObject.FindWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
        player = GameObject.FindWithTag(Tags.player);
        playerAnim = player.GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>();
        hash = GameObject.FindWithTag(Tags.gameController).GetComponent<HashIDs>();
        
        // Initialize misc variables
        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        if(lastPlayerSighting.position != previousSighting)
        {
            personalLastSighting = lastPlayerSighting.position;
        }

        previousSighting = lastPlayerSighting.position;

        if(playerHealth.health > 0.0f)
        {
            anim.SetBool(hash.playerInSightBool, playerInSight);
        }
        else
        {
            anim.SetBool(hash.playerInSightBool, false);
        }
    }

    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if(hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerSighting.position = player.transform.position;
                    }
                }
            }

            int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).fullPathHash;
            int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).fullPathHash;

            if(playerLayerZeroStateHash == hash.locomotionState || playerLayerOneStateHash == hash.shoutState)
            {
                if(CalculatePathLength(player.transform.position) <= col.radius)
                {
                    personalLastSighting = player.transform.position;
                }
            }
        }
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;
        }
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if(nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for(int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0.0f;

        for(int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}
