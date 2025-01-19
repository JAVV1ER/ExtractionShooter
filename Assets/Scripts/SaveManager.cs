using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public static class SaveManager
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void SaveEnemies(List<EnemyData> enemyDataList)
    {
        string json = JsonUtility.ToJson(new SaveData { enemies = enemyDataList }, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Данные сохранены в {SavePath}");
    }

    public static List<EnemyData> LoadEnemies()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Файл сохранения не найден.");
            return new List<EnemyData>();
        }

        string json = File.ReadAllText(SavePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        return saveData.enemies ?? new List<EnemyData>();
    }

    [Serializable]
    private class SaveData
    {
        public List<EnemyData> enemies;
    }
    
    [Serializable]
    public class EnemyData
    {
        public string Id; // Уникальный идентификатор
        public string EnemyType;
        public Vector3 Position;
        public bool IsAlive;

        public EnemyData(string id, string enemyType, Vector3 position, bool isAlive)
        {
            Id = id;
            EnemyType = enemyType;
            Position = position;
            IsAlive = isAlive;
        }
    }
}