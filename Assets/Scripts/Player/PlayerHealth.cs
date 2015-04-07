using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    // Declaration of public variables
    public float health = 100.0f;
    public float resetAfterDeathTime = 5.0f;
    public AudioClip deathClip;

    // Declaration of private variables
    private Animator anim;
    private PlayerMovement playerMovement;
    private HashIDs hash;
    private SceneFade sceneFade;
    private LastPlayerSighting lastPlayerSighting;
    private float timer;
    private bool playerDead;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        sceneFade = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFade>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public void Update()
    {
        if (health <= 0.0f)
        {
            if (playerDead)
            {
                PlayerDying();
            }
            else
            {
                PlayerDead();
                LevelReset();
            }
        }
    }

    void PlayerDying()
    {
        playerDead = true;
        anim.SetBool(hash.deadBool, true);
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    void PlayerDead()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.dyingState)
        {
            anim.SetBool(hash.deadBool, false);
        }

        anim.SetFloat(hash.speedFloat, 0.0f);
        playerMovement.enabled = false;
        lastPlayerSighting.position = lastPlayerSighting.resetPosition;
        GetComponent<AudioSource>().Stop();
    }

    void LevelReset()
    {
        timer += Time.deltaTime;

        if(timer >= resetAfterDeathTime)
        {
            sceneFade.EndScene();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
