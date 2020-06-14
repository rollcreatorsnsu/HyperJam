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
            wastedTimeText.text = $"{(LevelLoader.currentLevelData.levelTime - game.currentTime)*100:00:00}";
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
            wastedTimeText.text = "00:00";
        }

        totalTimeText.text = $"{(LevelLoader.currentLevelData.levelTime)*100:00:00}";
        remainedTimeText.text = $"{game.currentTime*100:00:00}";
        earnedResourcesText.text = $"+{Mathf.FloorToInt(game.currentTime)}";
        gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        if (LevelLoader.currentLevelNumber == 8)
        {
            if (LevelLoader.currentPackName == "Easy")
            {
                LevelLoader.Load("Medium", 1);
                return;
            }
            if (LevelLoader.currentPackName == "Medium")
            {
                LevelLoader.Load("Hard", 1);
                return;
            }
            SceneManager.LoadScene("Menu");
            return;
        }
        LevelLoader.Load(LevelLoader.currentPackName, LevelLoader.currentLevelNumber + 1);
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
