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

    void Start()
    {
        currentTime = LevelLoader.currentLevelData.levelTime;
        Util.GenerateField(LevelLoader.currentLevelData, emptyElementField, GenerationCallback);
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

        timeText.text = $"{currentTime}";
        resourcesText.text = $"{GameProgress.resources}";
    }

    public void UpdateField()
    {
        if (LevelLoader.currentLevelData.IsWin())
        {
            levelEndMenu.Show(true);
            return;
        }

        List<Element> condensers = LevelLoader.currentLevelData.IsCondeserDisconnected();
        if (condensers != null)
        {
            foreach (Element condenser in condensers)
            {
                StartCoroutine(CondenserOff(condenser));
            }
        }

        currentTime += LevelLoader.currentLevelData.GetAddTime();
    }

    private IEnumerator CondenserOff(Element condenser)
    {
        yield return new WaitForSeconds(5); // TODO: timing
        if (LevelLoader.currentLevelData.IsCondeserDisconnected().Contains(condenser))
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
        elementField.SetElement(levelData.levelStructure[x, y]);
    }
    
}
