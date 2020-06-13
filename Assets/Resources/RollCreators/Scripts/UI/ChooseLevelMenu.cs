using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelMenu : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    private string currentPack;

    void Start()
    {
        for (int i = 0; i < buttons.Capacity; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void ChoosePack(string packName)
    {
        int currentLevel = GameProgress.progress[packName];
        for (int i = 0; i < currentLevel; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }

        for (int i = currentLevel; i < buttons.Capacity; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        currentPack = packName;
    }

    public void ChooseLevel(int level)
    {
        LevelLoader.Load(currentPack, level);
    }
}
