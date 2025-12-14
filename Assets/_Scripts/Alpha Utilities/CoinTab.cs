using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinTab : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    private int coins;
    private GameData gameData;

    private void OnEnable()
    {
        if (gameData == null)
        {
            gameData = GameStats.gameData;
        }

        AlphaEvents.StartListening("UpdateCurrency", OnUpdateCurrency);
        OnUpdateCurrency();
    }

    private void OnDisable()
    {
        coins = 0;
        AlphaEvents.StopListening(AlphaEvents.UpdateCurrency, OnUpdateCurrency);
    }

    private void OnUpdateCurrency()
    {
        float updateDuration = 0.5f;

        _ = DOTween.To(() => coins, x => coins = x, gameData.points, updateDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() => { coinsText.text = $"Points {coins}"; })
            .SetUpdate(true);
    }

    public void B_OpenStore()
    {
        //if (MainMenuUI.Instance != null)
        //{
        //    MainMenuUI.Instance.mainMenuPanel.Disable();
        //}

        //PersistentCanvas.SetStorePanel(true);
    }
}
