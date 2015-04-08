using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour
{
    // Declaration of public variables
    public AudioClip keyGrab;

    // Declaration of private variables
    private GameObject player;
    private PlayerInventory playerInventory;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            AudioSource.PlayClipAtPoint(keyGrab, transform.position);
            playerInventory.hasKey = true;
            Destroy(gameObject);
        }
    }
}
