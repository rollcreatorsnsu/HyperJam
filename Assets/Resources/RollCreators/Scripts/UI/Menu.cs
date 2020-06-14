using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    
    void Awake()
    {
        BackgroundMusic.SetBackgroundMusic(music);
    }
    
    public void _Start()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
