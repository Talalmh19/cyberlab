using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int index;
    public Image levelBG;
    public GameObject replayButtonGO;
    public GameObject startButtonGO;
    public GameObject lockButtonGO;
    public TextMeshProUGUI levelPriceText;

    public void B_StartGame()
    {
        B_StartLevel();
    }

    public void B_StartLevel()
    {
        GameStats.level = index;
        GameStats.gameData.level = index;
        DataManager.SaveData();
        UIMain.Instance.B_StartGame();
    }

    public void B_BuyLevel()
    {
        UIMain.Instance.ClickSound();
        UIMain.Instance.BuyLevel(index);
    }
}
