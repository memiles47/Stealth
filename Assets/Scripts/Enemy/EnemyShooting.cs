using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    // Declaration of public variables
    public float maximumDamage = 120.0f;
    public float minimumDamage = 45.0f;
    public AudioClip shotClip;
    public float flashIntensity = 3.0f;
    public float fadeSpeed = 10.0f;

    // Declaration of private reference variables
    private Animator anim;
    private HashIDs hash;
    private LineRenderer laserShotLine;
    private Light laserShotLight;
    private SphereCollider col;
    private Transform player;
    private PlayerHealth playerHealth;

    // Declaration of private misc variables
    private bool shooting;
    private float scaledDamage;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        laserShotLine = GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.GetComponent<Light>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        laserShotLine.enabled = false;
        laserShotLight.intensity = 0.0f;
        scaledDamage = maximumDamage - minimumDamage;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        float shot = anim.GetFloat(hash.shotFloat);

        if(shot > 0.5f && !shooting)
        {
            Shoot();
        }

        if(shot < 0.5f)
        {
            shooting = false;
            laserShotLight.enabled = false;
        }

        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0.0f, fadeSpeed * Time.deltaTime);
    }

    // Callback for setting up animation IK (inverse kinematics)
    public void OnAnimatorIK(int layerIndex)
    {
        float aimWeight = anim.GetFloat(hash.aimWeightFloat);
        anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }

    void Shoot()
    {
        shooting = true;
        float fractionalDistance = (col.radius - Vector3.Distance(transform.position, player.position)) / col.radius;
        float damage = scaledDamage * fractionalDistance + minimumDamage;
        playerHealth.TakeDamage(damage);
        ShotEffects();
    }

    void ShotEffects()
    {
        laserShotLine.SetPosition(0, laserShotLine.transform.position);
        laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
        laserShotLine.enabled = true;
        laserShotLight.intensity = flashIntensity;
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }


}
