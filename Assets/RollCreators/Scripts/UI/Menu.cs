using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private bool isLoaded = false;
    void Awake()
    {
        GameProgress.Init();
        Options.Init();
        isLoaded = true;
    }
    
    public void _Start()
    {
        if (!isLoaded) return;
        SceneManager.LoadScene("ChooseLevel");
    }
}
