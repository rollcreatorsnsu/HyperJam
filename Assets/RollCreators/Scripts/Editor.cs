using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    private static ElementType[] ELEMENT_TYPES_EDITOR =
    {
        ElementType.NONE,
        ElementType.GENERATOR,
        ElementType.WIRES,
        ElementType.WIRES,
        ElementType.WIRES,
        ElementType.RESISTOR,
        ElementType.INDUCTION,
        ElementType.LAMP,
        ElementType.CONDENSER_OFF
    };

    private static int[] ELEMENT_CONNECTIONS_EDITOR =
    {
        0, 15, 5, 3, 11, 5, 5, 1, 1
    };

    [HideInInspector] public EditorCell selectedCell;
    [SerializeField] private GameObject emptyElement;
    [SerializeField] private InputField packName;
    [SerializeField] private InputField levelNumber;
    [SerializeField] private InputField levelTime;
    private LevelData levelData;

    // Start is called before the first frame update
    void Start()
    {
        levelData = new LevelData(4, 7);
        Util.GenerateField(levelData, emptyElement, GenerationCallback);
    }

    public void Save()
    {
        levelData.levelTime = Int32.Parse(levelTime.text);
        LevelLoader.Save(packName.text, Int32.Parse(levelNumber.text), levelData);
    }

    public void SetSelectedCell(int index)
    {
        if (selectedCell == null) return;
        selectedCell.element.type = ELEMENT_TYPES_EDITOR[index];
        selectedCell.element.connection = ELEMENT_CONNECTIONS_EDITOR[index];
        selectedCell.element.rotation = 0;
        selectedCell.transform.rotation = Quaternion.identity;
        selectedCell.SetElement(selectedCell.element);
    }

    private void GenerationCallback(GameObject element, LevelData levelData, int x, int y)
    {
        levelData.levelStructure[x, y] = new Element();
        EditorCell editorCell = element.GetComponent<EditorCell>();
        editorCell.SetElement(levelData.levelStructure[x, y]);
    }
}
