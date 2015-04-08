using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
    // Declaration of public variables
    public bool requireKey;
    public AudioClip doorSwishClip;
    public AudioClip accessDeniedClip;

    // Declaration of private reference variables
    private Animator anim;
    private HashIDs hash;
    private GameObject player;
    private PlayerInventory playerInventory;

    // Declaration of other private variables
    private int count;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            if(requireKey)
            {
                if(playerInventory.hasKey)
                {
                    count++;
                }
                else
                {
                    GetComponent<AudioSource>().clip = accessDeniedClip;
                    GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                count++;
            }
        }
        else if(other.gameObject.tag == Tags.enemy)
        {
            if(other is CapsuleCollider)
            {
                count++;
            }
        }
    }

    // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player || (other.gameObject.tag == Tags.enemy && other is CapsuleCollider))
        {
            count = Mathf.Max(0, -1);
        }
    }

    void Update()
    {
        anim.SetBool(hash.openBool, count > 0);

        if(anim.IsInTransition(0) && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = doorSwishClip;
            GetComponent<AudioSource>().Play();
        }
    }
}
