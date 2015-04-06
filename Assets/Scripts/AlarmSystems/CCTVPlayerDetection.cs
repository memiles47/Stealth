using UnityEngine;
using System.Collections;

public class CCTVPlayerDetection : MonoBehaviour
{
    // Declaration of public variables

    // Declaration of private variables
    private GameObject player;
    private LastPlayerSighting lastPlayerSighting;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            Vector3 relPlayerPos = player.transform.position - transform.position;
            RaycastHit hit;

            if(Physics.Raycast(transform.position, relPlayerPos, out hit))
            {
                if(hit.collider.gameObject == player)
                {
                    lastPlayerSighting.position = player.transform.position;
                }
            }
        }
    }
}
