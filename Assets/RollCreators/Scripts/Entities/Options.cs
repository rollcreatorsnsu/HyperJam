using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using UnityEngine;

public static class Options
{
    private static string fileName = "fastConnection.data";
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();
    private static bool _IsSound;

    public static bool IsSound
    {
        get => _IsSound;
        set
        {
            if (value)
            {
                AudioListener.volume = 1;
            }
            else
            {
                AudioListener.volume = 0;
            }

            _IsSound = value;
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
            IsSound = true;
            Save();
        }
    }

    public static void Save()
    {
        using (FileStream stream = new FileStream(fileName, FileMode.CreateNew))
        {
            binaryFormatter.Serialize(stream, IsSound);
        }
    }

    private static void Load()
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Open))
        {
            IsSound = (bool)binaryFormatter.Deserialize(stream);
        }
    }
    
}
