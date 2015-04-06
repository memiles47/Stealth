using UnityEngine;
using System.Collections;

public class LaserSwitchDeactivation : MonoBehaviour
{
    // Declaration of public variables
    public GameObject laser;
    public Material unlockedMat;

    // Declaration of private variables
    private GameObject player;
    private AudioSource deactivationSound;

    // Awake is called when the script instance is being loaded (Since v1.0)
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        deactivationSound = GetComponent<AudioSource>();
    }

    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            if(Input.GetButton("Switch"))
            {
                LaserDeactivation();
            }
        }
    }

    void LaserDeactivation()
    {
        laser.SetActive(false);

        Renderer screen = transform.Find("prop_switchUnit_screen").GetComponent<Renderer>();
        screen.material = unlockedMat;
        deactivationSound.Play();
    }
}