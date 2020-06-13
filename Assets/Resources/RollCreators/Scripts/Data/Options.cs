using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Options
{
    private static string fileName = "fastConnection.data";
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();
    private static bool _isSound;

    public static bool isSound
    {
        get => _isSound;
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

            _isSound = value;
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
            isSound = true;
            Save();
        }
    }

    public static void Save()
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            binaryFormatter.Serialize(stream, isSound);
        }
    }

    private static void Load()
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Open))
        {
            _isSound = (bool)binaryFormatter.Deserialize(stream);
        }
    }
    
}
