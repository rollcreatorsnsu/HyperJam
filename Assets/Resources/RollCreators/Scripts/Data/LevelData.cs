using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
public class LevelData
{
    public int levelWidth;
    public int levelHeight;
    public float levelTime;
    public Element[,] levelStructure;

    [NonSerialized] private List<Vector2Int> generatorsCoords = new List<Vector2Int>();
    [NonSerialized] private List<Vector2Int> condensersCoords = new List<Vector2Int>();
    [NonSerialized] private List<Vector2Int> inductionsCoords = new List<Vector2Int>();
    [NonSerialized] private List<Vector2Int> lampsCoords = new List<Vector2Int>();
    [NonSerialized] private bool calculated = false;

    public LevelData(int levelWidth, int levelHeight)
    {
        this.levelWidth = levelWidth;
        this.levelHeight = levelHeight;
        levelStructure = new Element[levelWidth, levelHeight];
    }

    public void UpdateConnections()
    {
        if (!calculated)
        {
            generatorsCoords = new List<Vector2Int>();
            condensersCoords = new List<Vector2Int>();
            inductionsCoords = new List<Vector2Int>();
            lampsCoords = new List<Vector2Int>();
            Calculate();
        }

        Clear();
        foreach (Vector2Int coords in generatorsCoords)
        {
            BFSFromGenerator(coords);
        }

        foreach (Vector2Int coords in condensersCoords)
        {
            Element condenser = levelStructure[coords.x, coords.y];
            bool connected = condenser.connected;
            if (connected)
            {
                condenser.type = ElementType.CONDENSER_ON;
            }
            if (condenser.type == ElementType.CONDENSER_ON)
            {
                BFSFromGenerator(coords);
            }

            condenser.connected = connected;
        }
    }

    private void Calculate()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                switch (levelStructure[x, y].type)
                {
                    case ElementType.GENERATOR:
                        generatorsCoords.Add(new Vector2Int(x, y));
                        break;
                    case ElementType.CONDENSER_ON:
                    case ElementType.CONDENSER_OFF:   
                        condensersCoords.Add(new Vector2Int(x, y));
                        break;
                    case ElementType.INDUCTION:
                    case ElementType.INDUCTION_USED:
                        inductionsCoords.Add(new Vector2Int(x, y));
                        break;
                    case ElementType.LAMP:
                        lampsCoords.Add(new Vector2Int(x, y));
                        break;
                }
            }
        }

        calculated = true;
    }

    private void Clear()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                levelStructure[x, y].connected = false;
            }
        }
    }

    private void BFSFromGenerator(Vector2Int coords)
    {
        Queue<Tuple<Element, Vector2Int>> queue = new Queue<Tuple<Element, Vector2Int>>();
        queue.Enqueue(new Tuple<Element, Vector2Int>(levelStructure[coords.x, coords.y], coords));
        while (queue.Count > 0)
        {
            Tuple<Element, Vector2Int> q = queue.Dequeue();
            q.Item1.connected = true;
            int connection = q.Item1.connection;
            if ((connection & 1) != 0 && q.Item2.x > 0)
            {
                Element element = levelStructure[q.Item2.x - 1, q.Item2.y];
                if (!element.connected && (element.connection & 4) != 0 && element.type != ElementType.BROKEN_RESISTOR && element.type != ElementType.COLD_RESISTOR)
                {
                    queue.Enqueue(new Tuple<Element, Vector2Int>(element, new Vector2Int(q.Item2.x - 1, q.Item2.y)));
                }
            }
            if ((connection & 2) != 0 && q.Item2.y < levelHeight - 1)
            {
                Element element = levelStructure[q.Item2.x, q.Item2.y + 1];
                if (!element.connected && (element.connection & 8) != 0 && element.type != ElementType.BROKEN_RESISTOR && element.type != ElementType.COLD_RESISTOR)
                {
                    queue.Enqueue(new Tuple<Element, Vector2Int>(element, new Vector2Int(q.Item2.x, q.Item2.y + 1)));
                }
            }
            if ((connection & 4) != 0 && q.Item2.x < levelWidth - 1)
            {
                Element element = levelStructure[q.Item2.x + 1, q.Item2.y];
                if (!element.connected && (element.connection & 1) != 0 && element.type != ElementType.BROKEN_RESISTOR && element.type != ElementType.COLD_RESISTOR)
                {
                    queue.Enqueue(new Tuple<Element, Vector2Int>(element, new Vector2Int(q.Item2.x + 1, q.Item2.y)));
                }
            }
            if ((connection & 8) != 0 && q.Item2.y > 0)
            {
                Element element = levelStructure[q.Item2.x, q.Item2.y - 1];
                if (!element.connected && (element.connection & 2) != 0 && element.type != ElementType.BROKEN_RESISTOR && element.type != ElementType.COLD_RESISTOR)
                {
                    queue.Enqueue(new Tuple<Element, Vector2Int>(element, new Vector2Int(q.Item2.x, q.Item2.y - 1)));
                }
            }
        }
    }

    public bool IsWin()
    {
        foreach (Vector2Int coords in lampsCoords)
        {
            if (!levelStructure[coords.x, coords.y].connected)
            {
                return false;
            }
        }
        return true;
    }

    public List<Element> GetDisconnectedCondensers()
    {
        List<Element> list = null;
        foreach (Vector2Int coords in condensersCoords)
        {
            Element condenser = levelStructure[coords.x, coords.y];
            if (condenser.type == ElementType.CONDENSER_ON && !condenser.connected)
            {
                if (list == null)
                {
                    list = new List<Element>();
                }
                list.Add(condenser);
            }
        }
        return list;
    }

    public int GetAddTime()
    {
        int result = 0;
        foreach (Vector2Int coords in inductionsCoords)
        {
            Element induction = levelStructure[coords.x, coords.y];
            if (induction.type == ElementType.INDUCTION && induction.connected)
            {
                induction.type = ElementType.INDUCTION_USED;
                result += 5; //TODO: balance
            }
        }

        return result;
    }

    public void GenerateRandom()
    {
        //TODO
    }
}
