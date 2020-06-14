using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public static class GameProgress
{
    private static string fileName = $"{Application.persistentDataPath}/fastConnection.sav";
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();
    public static Dictionary<string, int> progress = new Dictionary<string, int>();
    private static int _resources;

    public static int resources
    {
        get => _resources;
        set
        {
            _resources = value;
            Save();
        }
    }

    public static void Init()
    {
        if (File.Exists(fileName))
        {
            Load();
        }
        else
        {
            progress.Add("Easy", 1);
            progress.Add("Medium", 0);
            progress.Add("Hard", 0);
            resources = 10;
            Save();
        }
    }

    public static void Save()
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            binaryFormatter.Serialize(stream, progress);
            binaryFormatter.Serialize(stream, resources);
        }
    }

    private static void Load()
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Open))
        {
            progress = (Dictionary<string, int>)binaryFormatter.Deserialize(stream);
            _resources = (int) binaryFormatter.Deserialize(stream);
        }
    }
}
