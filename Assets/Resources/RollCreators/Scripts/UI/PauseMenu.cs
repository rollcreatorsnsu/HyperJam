using UnityEngine;
using UnityEngine.SceneManagement;

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
        Options.isSound = !Options.isSound;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }
}
