using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int levelWidth;
    public int levelHeight;
    public float levelTime;
    public Element[,] levelStructure;

    public LevelData(int levelWidth, int levelHeight)
    {
        this.levelWidth = levelWidth;
        this.levelHeight = levelHeight;
        levelStructure = new Element[levelWidth, levelHeight];
    }

    public bool IsWin()
    {
        return false;
        // TODO
    }

    public List<Element> IsCondeserDisconnected()
    {
        return null;
        //TODO
    }

    public int GetAddTime()
    {
        return 0;
        //TODO
    }

    public void GenerateRandom()
    {
        //TODO
    }
}
