using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private LevelEndMenu levelEndMenu;
    [SerializeField] private Text timeText;
    [SerializeField] private Text resourcesText;
    [HideInInspector] public float currentTime;

    void Start()
    {
        currentTime = LevelLoader.currentLevelData.levelTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.Show();
        }

        if (!pauseMenu.IsShown() && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            levelEndMenu.Show(false);
        }

        timeText.text = $"{currentTime}";
        resourcesText.text = $"{GameProgress.resources}";
    }

    public void ShowPause()
    {
        pauseMenu.Show();
    }
    
}
