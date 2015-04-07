using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Declaration of public variables
    public AudioClip shoutingClip;
    public float turnSmoothing = 15.0f;
    public float speedDampTime = 0.1f;

    // Declaration of private variables
    private Animator anim;
    private HashIDs hash;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        anim.SetLayerWeight(1, 1.0f);
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool sneak = Input.GetButton("Sneak");

        MovementManagement(h, v, sneak);
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public void Update()
    {
        bool shout = Input.GetButtonDown("Attract");
        anim.SetBool(hash.shoutingBool, shout);
        AudioManagement(shout);
    }

    void MovementManagement(float horizontal, float vertical, bool sneaking)
    {
        anim.SetBool(hash.sneakingBool, sneaking);
        if(horizontal != 0.0f || vertical != 0.0f)
        {
            Rotating(horizontal, vertical);
            anim.SetFloat(hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(hash.speedFloat, 0.0f);
        }
    }

    void Rotating(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0.0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        GetComponent<Rigidbody>().MoveRotation(newRotation);
    }

    void AudioManagement(bool shout)
    {
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.locomotionState)
        {
            if(GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            else
            {
                GetComponent<AudioSource>().Stop();
            }

            if(shout)
            {
                AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
            }
        }
    }

}