using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new("Data Manager");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<DataManager>();

                instance.InitDataManager();
            }

            return instance;
        }
    }

    public static bool DataLoaded;

    private GameData gameData;
    private const string Key = "GameData";

    private async void InitDataManager()
    {
        gameData = GameStats.gameData;

        while (UnityServices.Instance.State != ServicesInitializationState.Initialized)
        {
            await System.Threading.Tasks.Task.Delay(1000);
        }

        while (!AuthenticationService.Instance.IsSignedIn)
        {
            await System.Threading.Tasks.Task.Delay(1000);
        }

        LoadData();
    }

    private void OnApplicationQuit() { SaveData(); }

    public static void SaveData()
    {
        Instance.Save();
        //string json = JsonConvert.SerializeObject(instance.gameData);
        //PlayerPrefs.SetString(Key, json);
        //PlayerPrefs.Save();
    }

    public async void Save()
    {
        if (!AuthenticationService.Instance.IsSignedIn) { return; }

        try
        {
            string json = JsonConvert.SerializeObject(instance.gameData);
            var data = new Dictionary<string, object> { { Key, json } };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            Debug.Log("Game data saved to cloud.");

            DataLoaded = true;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to save data: {ex.Message}");
        }
    }

    private async void LoadData()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { Key });

            if (data.TryGetValue(Key, out Unity.Services.CloudSave.Models.Item item))
            {
                string json = item.Value.GetAs<string>();

                GameData loadedData = ScriptableObject.CreateInstance<GameData>();

                JsonUtility.FromJsonOverwrite(json, loadedData);

                //GameData loadedData = JsonConvert.DeserializeObject<GameData>(json);

                gameData.level = loadedData.level;
                gameData.sound = loadedData.sound;
                gameData.music = loadedData.music;
                gameData.points = loadedData.points;
                gameData.haveLevel = loadedData.haveLevel;
                gameData.playedLevel = loadedData.playedLevel;

                DataLoaded = true;

                Debug.Log("Game data loaded from cloud.");
            }
            else
            {
                Debug.Log("No saved game data found. Resetting data.");
                ResetData();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to load data: {ex.Message}");
            ResetData();
        }

        //if (!PlayerPrefs.HasKey(Key))
        //{
        //    ResetData();
        //    return;
        //}

        //GameData loadedData = ScriptableObject.CreateInstance<GameData>();

        //string json = PlayerPrefs.GetString(Key);

        //JsonUtility.FromJsonOverwrite(json, loadedData);

        //gameData.level = loadedData.level;
        //gameData.sound = loadedData.sound;
        //gameData.music = loadedData.music;
        //gameData.points = loadedData.points;
        //gameData.haveLevel = loadedData.haveLevel;
        //gameData.playedLevel = loadedData.playedLevel;
    }

    private void ResetGameData()
    {
        gameData.level = 0;
        gameData.points = 0;
        gameData.sound = 0.5f;
        gameData.music = 0.5f;
        Array.Fill(gameData.playedLevel, false);
        Array.Fill(gameData.haveLevel, false);
        gameData.haveLevel[0] = true;
        SaveData();
    }

    private void ResetLevelGameData()
    {
        gameData.level = 0;
        Array.Fill(gameData.playedLevel, false);
        Array.Fill(gameData.haveLevel, false);
        gameData.haveLevel[0] = true;
        SaveData();
    }

    public static void ResetData()
    {
        instance.ResetGameData();
    }

    public static void ResetLevelData()
    {
        instance.ResetLevelGameData();
    }
}
