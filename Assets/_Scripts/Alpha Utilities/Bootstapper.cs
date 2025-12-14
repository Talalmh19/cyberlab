using UnityEngine;

public class Bootstapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(GameStats.gameInitiatorPrefab));
    }
}
