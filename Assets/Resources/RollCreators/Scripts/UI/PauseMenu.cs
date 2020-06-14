using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image button;
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite inactiveButtonSprite;

    public void Show()
    {
        button.sprite = Options.isSound ? activeButtonSprite : inactiveButtonSprite;
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
        button.sprite = Options.isSound ? activeButtonSprite : inactiveButtonSprite;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }
}
