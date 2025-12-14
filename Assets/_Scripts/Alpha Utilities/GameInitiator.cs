using System.Threading.Tasks;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    private async void Start()
    {
        try { _ = DataManager.Instance; } catch { }
        await Task.Delay(500);
        try { _ = AlphaEvents.Instance; } catch { }
        await Task.Delay(500);
        try { DontDestroyOnLoad(Instantiate(GameStats.audioManagerPrefab)); } catch { }
    }
}
