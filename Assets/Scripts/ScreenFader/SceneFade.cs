using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    // Declaration of public variables


    // Declaration of private variables
    private bool sceneStarting = true;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       if(sceneStarting)
       {
           StartScreen();
       }

    }
    
    void FadeToClear()
    {
        anim.SetTrigger("StartScene");
    }

    void FadeToBlack()
    {
        anim.SetTrigger("EndScene");
    }

    void StartScreen()
    {
        FadeToClear();
        sceneStarting = false;
    }

    public void EndScene()
    {
        FadeToBlack();
        Application.LoadLevel(1);
    }
}
