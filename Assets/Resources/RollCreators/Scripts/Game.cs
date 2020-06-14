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
    [SerializeField] private GameObject emptyElementField;
    [HideInInspector] public float currentTime;
    private ElementField[] elements;

    void Start()
    {
        currentTime = LevelLoader.currentLevelData.levelTime;
        Util.GenerateField(LevelLoader.currentLevelData, emptyElementField, GenerationCallback);
        elements = FindObjectsOfType<ElementField>();
        UpdateField();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.Show();
        }

        if (!pauseMenu.IsShown() && !levelEndMenu.IsShown() && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            levelEndMenu.Show(false);
        }

        timeText.text = $"{currentTime*100:00:00}";
        resourcesText.text = $"{GameProgress.resources}";
    }

    public void UpdateField()
    {
        LevelLoader.currentLevelData.UpdateConnections();

        foreach (ElementField element in elements)
        {
            element.UpdateElementSprite(element.element);
        }

        List<Element> condensers = LevelLoader.currentLevelData.GetDisconnectedCondensers();
        if (condensers != null)
        {
            foreach (Element condenser in condensers)
            {
                StartCoroutine(CondenserOff(condenser));
            }
        }
        
        if (LevelLoader.currentLevelData.IsWin())
        {
            if (LevelLoader.currentLevelNumber < 8)
            {
                if (GameProgress.progress[LevelLoader.currentPackName] < LevelLoader.currentLevelNumber + 1)
                {
                    GameProgress.progress[LevelLoader.currentPackName] = LevelLoader.currentLevelNumber + 1;
                }
            }
            else
            {
                if (LevelLoader.currentPackName == "Easy")
                {
                    GameProgress.progress["Medium"] = 1;
                }
                else if (LevelLoader.currentPackName == "Medium")
                {
                    GameProgress.progress["Hard"] = 1;
                }
            }
            GameProgress.resources += Mathf.FloorToInt(currentTime);
            levelEndMenu.Show(true);
            return;
        }

        currentTime += LevelLoader.currentLevelData.GetAddTime();
    }

    private IEnumerator CondenserOff(Element condenser)
    {
        yield return new WaitForSeconds(3);
        if (!condenser.connected)
        {
            condenser.type = ElementType.CONDENSER_OFF;
            UpdateField();
        }
    }

    public void ShowPause()
    {
        pauseMenu.Show();
    }

    private void GenerationCallback(GameObject element, LevelData levelData, int x, int y)
    {
        ElementField elementField = element.GetComponent<ElementField>();
        elementField.UpdateElementSprite(levelData.levelStructure[x, y]);
    }
    
}
