#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class SaveManagerEditor
{
    [MenuItem("Tools/Clear Game Save Data")]
    public static void ClearSaveData()
    {
        string savePath = Application.persistentDataPath + "/save.json";

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Сохранение очищено.");
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден. Очистка не требуется.");
        }
    }
}
#endif