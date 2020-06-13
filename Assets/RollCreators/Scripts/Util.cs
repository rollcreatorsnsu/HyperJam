using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    
    public delegate void GenerationCallback(GameObject emptyElementField, LevelData levelData, int x, int y);

    public static void GenerateField(LevelData levelData, GameObject emptyElementField, GenerationCallback callback)
    {
        Rect cameraRect = Camera.main.rect;
        float width = cameraRect.width / (levelData.levelWidth + 2);
        float height = cameraRect.height / (levelData.levelHeight + 2);
        float offsetX = width;
        float offsetY = height;
        if (width > height)
        {
            offsetX += (width - height) * levelData.levelWidth / 2;
            width = height;
        } 
        else if (width < height)
        {
            offsetY += (height - width) * levelData.levelHeight / 2;
            height = width;
        }
        for (int x = 0; x < levelData.levelWidth; x++)
        {
            for (int y = 0; y < levelData.levelHeight; y++)
            {
                GameObject element = Instantiate(emptyElementField, new Vector3(offsetX + x * width, offsetY + y * height), Quaternion.identity);
                element.transform.localScale = new Vector3(width, height);
                callback(element, levelData, x, y);
            }
        }
    }

    public static Sprite GetElementSprite(ElementType type)
    {
        //TODO
        switch (type)
        {
            case ElementType.NONE:
                return Resources.Load<Sprite>("NONE");
        }

        return null;
    }
    
}
