using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    private Animator animator;
    private bool isLoaded = false;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        BackgroundMusic.SetBackgroundMusic(music);
        GameProgress.Init();
        Options.Init();
        isLoaded = true;
    }

    public void Loading()
    {
        animator.Play(isLoaded ? "FadeIn" : "Idle");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
