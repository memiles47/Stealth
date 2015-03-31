using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{
    // Declaration of public variables
    public float fadeSpeed = 2.0f;
    public float highIntensity = 2.0f;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;
    public bool alarmOn;
    public Light light;

    // Declaration of private variables
    private float targetIntensity;
    //private Light light;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        light.intensity = 0.0f;
        targetIntensity = highIntensity;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public void Update()
    {
        if(alarmOn)
        {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            CheckTargetIntensity();
        }
        else
        {
            light.intensity = Mathf.Lerp(light.intensity, 0.0f, fadeSpeed * Time.deltaTime);
        }
    }

    void CheckTargetIntensity()
    {
        if(Mathf.Abs(targetIntensity - light.intensity) < changeMargin)
        {
            if(targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }
            else
            {
                targetIntensity = highIntensity;
            }
        }
    }
}
