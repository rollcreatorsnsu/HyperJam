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
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource looseSound;
    [SerializeField] private List<Image> backImages;
    [SerializeField] private SpriteRenderer background;
    [HideInInspector] public float currentTime;
    private ElementField[] elements;
    private bool begin = false;
    [HideInInspector] public bool end = false;

    void Start()
    {
        BackgroundMusic.SetBackgroundMusic(music);
        Color color = Color.white;
        switch (LevelLoader.currentPackName)
        {
            case "Easy":
                color = new Color(0.667f, 0.988f, 0.027f);
                break;
            case "Hard":
                color = new Color(0.976f, 0.275f, 0.275f);
                break;
        }
        foreach (Image image in backImages)
        {
            image.color = color;
        }
        background.color = color;
        currentTime = LevelLoader.currentLevelData.levelTime;
        Util.GenerateField(LevelLoader.currentLevelData, emptyElementField, GenerationCallback);
        elements = FindObjectsOfType<ElementField>();
        UpdateField();
        begin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (end) return;
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.Show();
        }
        
        if (!pauseMenu.IsShown() && !levelEndMenu.IsShown() && currentTime > 0 && begin)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            if (!end)
            {
                looseSound.Play();
            }
            end = true;
            levelEndMenu.Show(false);
        }

        timeText.text = $"{currentTime*100:00:00}";
        resourcesText.text = $"{GameProgress.resources}";
    }

    public void UpdateField()
    {
        begin = true;

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
            if (!end)
            {
                winSound.Play();
            }
            end = true;
            StartCoroutine(Win());
            return;
        }

        currentTime += LevelLoader.currentLevelData.GetAddTime();
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(2);
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
    }    

    private IEnumerator CondenserOff(Element condenser)
    {
        yield return new WaitForSeconds(3);
        if (!condenser.connected && !end)
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
