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
        Debug.Log($"begin {beginCamera.x},{beginCamera.y} end {endCamera.x},{endCamera.y}");
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

    private static int GetDirections(int connections)
    {
        int dirs = 0;
        while (connections != 0)
        {
            if ((connections & 1) == 1)
            {
                dirs++;
            }
            connections >>= 1;
        }

        return dirs;
    }

    public static Sprite GetElementSprite(Element element)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("RollCreators/Sprites/Elements/Elements");
        switch (element.type)
        {
            case ElementType.GENERATOR:
                switch (GetDirections(element.connection))
                {
                    case 4:
                        return sprites[14];
                    case 3:
                        return sprites[15];
                    case 2:
                        if (element.connection == 10 || element.connection == 5)
                        {
                            return sprites[17];
                        }
                        else
                        {
                            return sprites[16];
                        }
                    case 1:
                        return sprites[18];
                }

                break;
            case ElementType.WIRES:
                switch (GetDirections(element.connection))
                {
                    case 4:
                        return sprites[19];
                    case 3:
                        return sprites[11];
                    case 2:
                        if (element.connection == 10 || element.connection == 5)
                        {
                            return sprites[0];
                        }
                        else
                        {
                            return sprites[7];
                        }
                }

                break;
            case ElementType.RESISTOR:
                return sprites[1];
            case ElementType.BROKEN_RESISTOR:
                return sprites[2];
            case ElementType.COLD_RESISTOR:
                return sprites[3];
            case ElementType.INDUCTION:
            case ElementType.INDUCTION_USED:
                return sprites[10];
            case ElementType.LAMP:
                return sprites[8];
            case ElementType.CONDENSER_ON:
            case ElementType.CONDENSER_OFF:
                return sprites[28];
        }

        return null;
    }

    public static Sprite GetElementStaticSprite(Element element)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("RollCreators/Sprites/Elements/Elements");
        switch (element.type)
        {
            case ElementType.GENERATOR:
                return sprites[12];
            case ElementType.RESISTOR:
                return sprites[4];
            case ElementType.BROKEN_RESISTOR:
                return sprites[5];
            case ElementType.COLD_RESISTOR:
                return sprites[6];
        }
        return null;
    }
    
    public static Sprite GetElementLightSprite(Element element)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("RollCreators/Sprites/Elements/Elements");
        switch (element.type)
        {
            case ElementType.BROKEN_RESISTOR:
                return sprites[20];
            case ElementType.COLD_RESISTOR:
                return sprites[13];
            case ElementType.LAMP:
                return element.connected ? sprites[9] : null;
        }
        return null;
    }

    public static List<Sprite> GetElementElectricitySprites(Element element)
    {
        if ((!element.connected && element.type != ElementType.CONDENSER_ON) || element.type == ElementType.NONE || element.type == ElementType.CONDENSER_OFF) return null;
        List<Sprite> result = new List<Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("RollCreators/Sprites/Elements/Elements");
        switch (element.type)
        {
            case ElementType.WIRES:
                switch (GetDirections(element.connection))
                {
                    case 4:
                        result.Add(sprites[21]);
                        result.Add(sprites[27]);
                        break;
                    case 3:
                        result.Add(sprites[21]);
                        result.Add(sprites[26]);
                        break;
                    case 2:
                        if (element.connection == 10 || element.connection == 5)
                        {
                            result.Add(sprites[21]);
                            break;
                        }
                        else
                        {
                            result.Add(sprites[23]);
                            break;
                        }
                }
                break;
            case ElementType.RESISTOR:
            case ElementType.BROKEN_RESISTOR:
                result.Add(sprites[22]);
                break;
            case ElementType.INDUCTION:
            case ElementType.INDUCTION_USED:
                result.Add(sprites[25]);
                break;
            case ElementType.LAMP:
                result.Add(sprites[24]);
                break;
            case ElementType.CONDENSER_ON:
                result.Add(sprites[21]);
                result.Add(sprites[29]);
                break;
        }
        return result;
    }
}
