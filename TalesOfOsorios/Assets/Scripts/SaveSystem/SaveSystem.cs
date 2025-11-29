using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveSystem
    {
        private const string SAVE_FILE_NAME = "gamesave.json";
        
        private static string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        
        public static bool ShouldLoadOnStart { get; set; } = false;

        public static void SaveGame(GameSaveData data)
        {
            try
            {
                data.saveDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SaveFilePath, json);
                
                //Debug.Log($"Game saved successfully to: {SaveFilePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save game: {e.Message}");
            }
        }

        public static GameSaveData LoadGame()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    //Debug.LogWarning("No save file found. Returning new save data.");
                    return new GameSaveData();
                }

                string json = File.ReadAllText(SaveFilePath);
                GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
                
                //Debug.Log($"Game loaded successfully from: {SaveFilePath}");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}");
                return new GameSaveData();
            }
        }

        public static bool SaveExists()
        {
            return File.Exists(SaveFilePath);
        }
    }
}
