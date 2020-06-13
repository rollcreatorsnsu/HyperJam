using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SwitchSound()
    {
        Options.IsSound = !Options.IsSound;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }
}
