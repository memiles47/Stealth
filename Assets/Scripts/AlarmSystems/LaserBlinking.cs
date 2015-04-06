using UnityEngine;
using System.Collections;

public class LaserBlinking : MonoBehaviour
{
    // Declaration of public variables
    public float onTime;
    public float offTime;

    // Dedlaration of private variables
    private float timer;

    
    
    
    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        timer += Time.deltaTime;

        if (GetComponent<MeshRenderer>().enabled && timer >= onTime)
        {
            SwitchBeam();
        }

        if(!GetComponent<MeshRenderer>().enabled && timer >= offTime)
        {
            SwitchBeam();
        }
    }

    void SwitchBeam()
    {
        timer = 0.0f;
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
        GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
    }
}
