using UnityEngine;
using System.IO;
using static Marisa;

public class SaveSystem
{
    private static SaveData saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
    }

    public static int slotNumber = 1;

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + $"/Save Files/{slotNumber}" + ".absgaming";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }

    public static void HandleSaveData()
    {
        GameManager.Instance.player.Save(ref saveData.PlayerData);
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    public static void HandleLoadData()
    {
        GameManager.Instance.player.Load(saveData.PlayerData);
    }
}
