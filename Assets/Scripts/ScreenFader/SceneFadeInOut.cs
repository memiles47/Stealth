using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour
{
    // Declaration of public variables
    public float fadeSpeed = 1.5f;

    // Declaration of private variables
    private bool sceneStarting = true;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        //guiTexture.pixelInset = new Rect(0.0f, 0.0f, Screen.width, Screen.height);

    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public void Update()
    {
        if(sceneStarting)
        {
            StartScreen();
        }
    }

    void FadeToClear()
    {
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack()
    {
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScreen()
    {
        FadeToClear();

        if(GetComponent<GUITexture>().color.a <= 0.05f)
        {
            GetComponent<GUITexture>().color = Color.clear;
            GetComponent<GUITexture>().enabled = false;
            sceneStarting = false;
        }
    }

    public void EndScene()
    {
        GetComponent<GUITexture>().enabled = true;
        FadeToBlack();

        if(GetComponent<GUITexture>().color.a >= 0.95f)
        {
            Application.LoadLevel(1);

        }

    }
}
