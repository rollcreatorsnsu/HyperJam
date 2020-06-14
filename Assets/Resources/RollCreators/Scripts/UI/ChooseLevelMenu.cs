using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelMenu : MonoBehaviour
{
    [SerializeField] private List<Image> buttons;
    [SerializeField] private Sprite activeLevelSprite;
    [SerializeField] private Sprite inactiveLevelSprite;
    [SerializeField] private Image easyPackButton;
    [SerializeField] private Image mediumPackButton;
    [SerializeField] private Image hardPackButton;
    [SerializeField] private Image background;
    [SerializeField] private Image panel;
    [SerializeField] private Sprite currentPackSprite;
    [SerializeField] private Sprite activePackSprite;
    [SerializeField] private Sprite inactivePackSprite;
    [SerializeField] private AudioSource music;
    private string currentPack;

    void Start()
    {
        BackgroundMusic.SetBackgroundMusic(music);
        for (int i = 0; i < buttons.Capacity; i++)
        {
            buttons[i].sprite = inactiveLevelSprite;
        }
        ChoosePack("Easy");
    }

    public void ChoosePack(string packName)
    {
        if (GameProgress.progress[packName] == 0) return;
        easyPackButton.sprite = GameProgress.progress["Easy"] > 0 ? activePackSprite : inactivePackSprite;
        mediumPackButton.sprite = GameProgress.progress["Medium"] > 0 ? activePackSprite : inactivePackSprite;
        hardPackButton.sprite = GameProgress.progress["Hard"] > 0 ? activePackSprite : inactivePackSprite;
        Color color = Color.white;
        if (packName == "Easy")
        {
            easyPackButton.sprite = currentPackSprite;
            color = new Color(0.757f, 1f, 0.588f);
        }
        else if (packName == "Hard")
        {
            hardPackButton.sprite = currentPackSprite;
            color = new Color(0.976f, 0.275f, 0.275f);
        }
        easyPackButton.color = color;
        mediumPackButton.color = color;
        hardPackButton.color = color;
        background.color = color;
        panel.color = color;
        int currentLevel = GameProgress.progress[packName];
        for (int i = 0; i < currentLevel; i++)
        {
            buttons[i].sprite = activeLevelSprite;
            buttons[i].color = color;
        }

        for (int i = currentLevel; i < buttons.Capacity; i++)
        {
            buttons[i].sprite = inactiveLevelSprite;
            buttons[i].color = color;
        }

        currentPack = packName;
    }

    public void ChooseLevel(int level)
    {
        if (currentPack != null && GameProgress.progress[currentPack] >= level)
        {
            LevelLoader.Load(currentPack, level);
        }
    }
}
