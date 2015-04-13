using UnityEngine;
using System.Collections;

public class LiftTrigger : MonoBehaviour
{
    // Declaration of public variables
    public float timeToDoorsClose = 2.0f;
    public float timeToLiftStart = 3.0f;
    public float timeToEndLevel = 6.0f;
    public float liftSpeed = 3.0f;

    // Declaration of private reference variables
    private GameObject player;
    private Animator playerAnim;
    private HashIDs hash;
    private CameraMovement cameraMovement;
    private SceneFade sceneFade;
    private LiftDoorsTracking liftDoorsTracking;
    
    // Declaration of private misc variables
    private bool playerInLift;
    private float timer;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerAnim = player.GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        cameraMovement = Camera.main.gameObject.GetComponent<CameraMovement>();
        sceneFade = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFade>();
        liftDoorsTracking = GetComponent<LiftDoorsTracking>();
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        if(playerInLift)
        {
            LiftActivation();
        }

        if(timer < timeToDoorsClose)
        {
            liftDoorsTracking.DoorFollowing();
        }
        else
        {
            liftDoorsTracking.CloseDoors();
        }
    }



    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInLift = true;
        }
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == playerInLift)
        {
            playerInLift = false;
            timer = 0.0f;
        }
    }

    void LiftActivation()
    {
        timer += Time.deltaTime;

        if(timer >= timeToLiftStart)
        {
            playerAnim.SetFloat(hash.speedFloat, 0.0f);
            cameraMovement.enabled = false;
            player.transform.parent = transform;

            transform.Translate(Vector3.up * liftSpeed * Time.deltaTime);

            if(!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

            if(timer >= timeToEndLevel)
            {
                sceneFade.EndScene();
            }
        }
    }
}