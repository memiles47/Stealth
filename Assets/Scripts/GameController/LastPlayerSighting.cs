using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour 
{
    // Declaration of public variables
    public Vector3 position = new Vector3(1000.0f, 1000.0f, 1000.0f);
    public Vector3 resetPosition = new Vector3(1000.0f, 1000.0f, 1000.0f);
    public float lightHighIntensity = 0.25f;
    public float lightLowIntensity = 0.0f;
    public float fadeSpeed = 7.0f;
    public float musicFadeSpeed = 1.0f;

    // Declaration of private variables
    private AlarmLight alarm;
    private Light mainLight;
    private AudioSource panicAudio;
    private AudioSource[] sirens;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        alarm = GameObject.FindGameObjectWithTag(Tags.alarm).GetComponent<AlarmLight>();
        mainLight = GameObject.FindGameObjectWithTag(Tags.mainLight).GetComponent<Light>();
        panicAudio = transform.Find("secondaryMusic").GetComponent<AudioSource>();
        GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag(Tags.siren);
        sirens = new AudioSource[sirenGameObjects.Length];

        for(int i = 0; i < sirens.Length; i++)
        {
            sirens[i] = sirenGameObjects[i].GetComponent<AudioSource>();
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        SwitchAlarms();
        MusicFading();
    }

    void SwitchAlarms()
    {
        alarm.alarmOn = (position != resetPosition);
        float newIntensity;

        if(position != resetPosition)
        {
            newIntensity = lightLowIntensity;
        }
        else
        {
            newIntensity = lightHighIntensity;
        }
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        for(int i = 0; i < sirens.Length; i++)
        {
            if(position != resetPosition && !sirens[i].isPlaying)
            {
                sirens[i].Play();
            }
            else if(position == resetPosition)
            {
                sirens[i].Stop();
            }
        }
    }

    void MusicFading()
    {
        if(position != resetPosition)
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0f, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
        }
        else
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.8f, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
        }
    }
}
