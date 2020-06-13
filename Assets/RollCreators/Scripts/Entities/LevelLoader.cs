using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string currentPackName;

    public static int currentLevelNumber
    {
        get => _currentLevelNumber;
        set
        {
            _currentLevelNumber = value;
            if (value > GameProgress.progress[currentPackName])
            {
                GameProgress.progress[currentPackName] = value;
                GameProgress.Save();
            }
        }
    }
    public static LevelData currentLevelData;

    private static int _currentLevelNumber;
    private static BinaryFormatter formatter = new BinaryFormatter();
    
    public static void Load(string packName, int levelNumber)
    {
        using (FileStream stream = new FileStream($"{packName}/{levelNumber}", FileMode.Open))
        {
            currentLevelData = (LevelData) formatter.Deserialize(stream);
        }

        currentPackName = packName;
        currentLevelNumber = levelNumber;
        SceneManager.LoadScene("Game");
    }

    public static void GenerateRandomLevel()
    {
        currentLevelData = new LevelData();
        currentLevelData.GenerateRandom();
        SceneManager.LoadScene("Game");
    }

    public static void Save(string packName, int levelNumber) // for internal use only
    {
        using (FileStream stream = new FileStream($"{packName}/{levelNumber}", FileMode.Open))
        {
            formatter.Serialize(stream, currentLevelData);
        }
    }
}
