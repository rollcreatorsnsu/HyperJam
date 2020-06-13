using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    
    public delegate void GenerationCallback(GameObject emptyElementField, LevelData levelData, int x, int y);

    public static void GenerateField(LevelData levelData, GameObject emptyElementField, GenerationCallback callback)
    {
        Vector2 beginCamera = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 endCamera = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Rect cameraRect = new Rect(beginCamera, endCamera - beginCamera);
        float width = cameraRect.width / (levelData.levelWidth + 1);
        float height = cameraRect.height / (levelData.levelHeight + 1);
        float offsetX = cameraRect.x + width;
        float offsetY = cameraRect.y + height;
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
                Rect r = element.GetComponent<RectTransform>().rect;
                element.transform.localScale = new Vector3(width / r.width, height / r.height);
                callback(element, levelData, x, levelData.levelHeight - y - 1);
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
