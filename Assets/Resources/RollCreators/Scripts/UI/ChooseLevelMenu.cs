using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelMenu : MonoBehaviour
{
    [SerializeField] private List<Image> buttons;
    [SerializeField] private Sprite activeLevelSprite;
    [SerializeField] private Sprite inactiveLevelSprite;
    private string currentPack;

    void Start()
    {
        for (int i = 0; i < buttons.Capacity; i++)
        {
            buttons[i].sprite = inactiveLevelSprite;
        }
    }

    public void ChoosePack(string packName)
    {
        int currentLevel = GameProgress.progress[packName];
        for (int i = 0; i < currentLevel; i++)
        {
            buttons[i].sprite = activeLevelSprite;
        }

        for (int i = currentLevel; i < buttons.Capacity; i++)
        {
            buttons[i].sprite = inactiveLevelSprite;
        }

        currentPack = packName;
    }

    public void ChooseLevel(int level)
    {
        if (GameProgress.progress[currentPack] >= level)
        {
            LevelLoader.Load(currentPack, level);
        }
    }
}
