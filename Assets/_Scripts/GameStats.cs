using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameStats
{
    public static int level;
    public static string nextScene;
    public static float loadingDuration;

    public static GameData gameData = Resources.Load<GameData>("Bootstapper/Game Data");
    public static GameObject audioManagerPrefab = Resources.Load<GameObject>("Bootstapper/Audio Manager");
    public static GameObject gameInitiatorPrefab = Resources.Load<GameObject>("Bootstapper/Game Initiator");

    public const string SceneLoading = "Loading";
    public const string SceneMainMenu = "Main Menu";
    public const string SceneGameplay = "Gameplay";

    public const string TagPeople = "People";
    public const string TagCivilian = "Civilian";
    public const string TagWallRestriction = "WallRestriction";
    public const string TagArrest = "Arrest";

    public static void LoadScene(string scene, float loadingTime = 3.5f)
    {
        nextScene = scene;
        loadingDuration = loadingTime;
        SceneManager.LoadScene(SceneLoading);
    }

    public static void LoadDirectScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void PrivacyPolicy()
    {
        string url = "";
        Application.OpenURL(url);
    }

    public static void MoreGames()
    {
        string url = "";
        Application.OpenURL(url);
    }

    public static void RateUs()
    {
        string url = "";
        Application.OpenURL(url);
    }
}
