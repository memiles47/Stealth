using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{
    // Declaration of public variables
    public float deadZone = 5.0f;

    // Declaration of private reference variables
    private Transform player;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Animator anim;
    private HashIDs hash;
    private AnimatorSetup animSetup;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        player = GameObject.FindWithTag(Tags.player).transform;
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hash = GameObject.FindWithTag(Tags.gameController).GetComponent<HashIDs>();
        animSetup = new AnimatorSetup(anim, hash);

        nav.updateRotation = false;

        anim.SetLayerWeight(1, 1.0f);
        anim.SetLayerWeight(2, 1.0f);

        deadZone *= Mathf.Deg2Rad;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        NavAnimSetup();
    }

    // This callback will be invoked at each frame after the state machines and the animations have been evaluated, but before OnAnimatorIK
    void OnAnimatorMove()
    {
        nav.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }

    void NavAnimSetup()
    {
        float speed;
        float angle;

        if(enemySight.playerInSight)
        {
            speed = 0.0f;
            angle = FindAngle(transform.forward, player.position - transform.position, transform.up);
        }
        else
        {
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
            angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

            if(Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + nav.desiredVelocity);
                angle = 0.0f;
            }
        }

        animSetup.Setup(speed, angle);
    }

    float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if(toVector == Vector3.zero)
        {
            return 0.0f;
        }

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;

        return angle;
    }
}
