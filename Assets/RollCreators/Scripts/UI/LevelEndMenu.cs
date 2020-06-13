﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndMenu : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Text levelEndText;
    [SerializeField] private Text totalTimeText;
    [SerializeField] private Text wastedTimeText;
    [SerializeField] private Text remainedTimeText;
    [SerializeField] private Text earnedResourcesText;
    [SerializeField] private Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;
    [SerializeField] private Sprite completedButtonSprite;
    [SerializeField] private Sprite failedButtonSprite;
    
    public bool IsShown()
    {
        return gameObject.activeSelf;
    }
    
    public void Show(bool completed)
    {
        if (completed)
        {
            levelEndText.text = "LEVEL COMPLETE!";
            wastedTimeText.color = Color.white;
            remainedTimeText.color = Color.cyan;
            earnedResourcesText.color = Color.cyan;
            buttonText.text = "NEXT";
            buttonImage.sprite = completedButtonSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(NextLevel);
        }
        else
        {
            levelEndText.text = "LEVEL FAILED!";
            wastedTimeText.color = Color.red;
            remainedTimeText.color = Color.red;
            earnedResourcesText.color = Color.red;
            buttonText.text = "RETRY";
            buttonImage.sprite = failedButtonSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(RetryLevel);
        }

        totalTimeText.text = $"{LevelLoader.currentLevelData.levelTime}";
        wastedTimeText.text = $"{LevelLoader.currentLevelData.levelTime - game.currentTime}";
        remainedTimeText.text = $"{game.currentTime}";
        earnedResourcesText.text = $"+{Mathf.FloorToInt(game.currentTime)}";
    }

    public void NextLevel()
    {
        GameProgress.resources += Mathf.FloorToInt(game.currentTime);
        LevelLoader.currentLevelNumber++;
        if (LevelLoader.currentLevelNumber == 12)
        {
            SceneManager.LoadScene("Menu");
            return;
        }
        LevelLoader.Load(LevelLoader.currentPackName, LevelLoader.currentLevelNumber);
    }

    public void RetryLevel()
    {
        LevelLoader.Load(LevelLoader.currentPackName, LevelLoader.currentLevelNumber);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
