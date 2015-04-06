using UnityEngine;
using System.Collections;

public class LaserPlayerDetection : MonoBehaviour
{
    // Declaration of public variables

    // Declaration of private veriables
    private GameObject player;
    private LastPlayerSighting lastPlayerSighting;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    void OnTriggerStay(Collider other)
    {
        if(GetComponent<MeshRenderer>().enabled)
        {
            if(other.gameObject == player)
            {
                lastPlayerSighting.position = other.transform.position;
            }
        }
    }
}