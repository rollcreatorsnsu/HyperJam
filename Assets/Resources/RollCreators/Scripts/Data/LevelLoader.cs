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
        TextAsset textAsset = Resources.Load($"RollCreators/Data/Levels/{packName}/{levelNumber}") as TextAsset;
        using (MemoryStream stream = new MemoryStream(textAsset.bytes))
        {
            currentLevelData = formatter.Deserialize(stream) as LevelData;;
        }
        currentPackName = packName;
        currentLevelNumber = levelNumber;
        SceneManager.LoadScene("Game");
    }

    public static void GenerateRandomLevel()
    {
        currentLevelData = new LevelData(4, 7);
        currentLevelData.GenerateRandom();
        SceneManager.LoadScene("Game");
    }

    public static void Save(string packName, int levelNumber, LevelData levelData) // for internal use only
    {
        if (!Directory.Exists($"{packName}"))
        {
            Directory.CreateDirectory($"{packName}");
        }
        using (FileStream stream = new FileStream($"{packName}/{levelNumber}.bytes", FileMode.Create))
        {
            formatter.Serialize(stream, levelData);
        }
    }
}
