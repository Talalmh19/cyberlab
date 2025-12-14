using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public static UIMain Instance { get; private set; }
    public GameObject allPanels;
    public GameObject mainPanel;
    public GameObject levelsPanle;
    public GameObject optionsPanle;
    public GameObject howToPlayPanle;
    public GameObject exitPanle;
    public GameObject notEnoughPointsTab;
    public PopUp notEnoughPointsTabPopUp;
    public GameObject shuttingDownScreen;
    public GameObject shuttingDownBlackImageGO;
    public Transform pcScreenTrans;
    public Transform closeMainTrans;
    public Transform finalTrans;
    public LevelButton[] levelButtons;
    public Sprite[] levelSprites;
    public int[] levelPrice;
    [Space]
    [Header("Options")]
    public AudioMixer mixer;
    public Slider soundSlider;
    public Slider musicSlider;
    public Image soundSliderBG;
    public Image musicSliderBG;
    public Sprite sliderBGSpriteOn;
    public Sprite sliderBGSpriteOff;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
        //_ = DataManager.Instance;
    }

    private void Start()
    {
        GameStats.level = GameStats.gameData.level;
        InitLevelButtons();
        SetLevelButtons();
        soundSlider.value = (int)GameStats.gameData.sound;
        musicSlider.value = (int)GameStats.gameData.music;
        SetSoundMixer(GameStats.gameData.sound == 1);
        SetMixer(AudioManager.MixerBGM, GameStats.gameData.music == 1);

        _ = allPanels.transform.DOMove(closeMainTrans.position, 0).SetUpdate(true)
            .OnComplete(() =>
            {
                _ = allPanels.transform.DOMove(finalTrans.position, 1).SetUpdate(true);
            });
    }

    private void InitLevelButtons()
    {
        for (int j = 0; j < levelButtons.Length; j++)
        {
            levelButtons[j].index = j;
            levelButtons[j].lockButtonGO.Enable();
            levelButtons[j].startButtonGO.Disable();
            levelButtons[j].replayButtonGO.Disable();
            levelButtons[j].levelBG.sprite = levelSprites[j];
            levelButtons[j].levelPriceText.text = $"points {levelPrice[j]}";
        }
    }

    private void SetLevelButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            if (GameStats.gameData.haveLevel[i])
            {
                levelButtons[i].lockButtonGO.Disable();
                levelButtons[i].startButtonGO.SetActive(!GameStats.gameData.playedLevel[i]);
                levelButtons[i].replayButtonGO.SetActive(GameStats.gameData.playedLevel[i]);
            }
        }
    }

    public void BuyLevel(int level)
    {
        if (levelPrice[level] > GameStats.gameData.points)
        {
            notEnoughPointsTab.Enable();
            Invoke(nameof(DisableNotEnoughPointsTab), 2.5f);
        }
        else
        {
            GameStats.gameData.points -= levelPrice[level];
            GameStats.gameData.haveLevel[level] = true;
            DataManager.SaveData();
            SetLevelButtons();
            AlphaEvents.TriggerEvent(AlphaEvents.UpdateCurrency);
        }
    }

    private void DisableNotEnoughPointsTab()
    {
        notEnoughPointsTabPopUp.ClosePopUp();
    }

    #region Panels
    public void B_ExitPanel()
    {
        ClickSound();
        exitPanle.Enable();
    }

    public void B_CancelExit()
    {
        ClickSound();
        exitPanle.Disable();
    }

    public void B_Levels()
    {
        ClickSound();
        levelsPanle.Enable();
    }

    public void B_CancelLevels()
    {
        ClickSound();
        levelsPanle.Disable();
    }

    public void B_Options()
    {
        ClickSound();
        optionsPanle.Enable();
    }

    public void B_CalcelOptions()
    {
        ClickSound();
        optionsPanle.Disable();
    }

    public void B_HowToPlayPanle()
    {
        ClickSound();
        howToPlayPanle.Enable();
    }

    public void B_CalcelhowToPlayPanle()
    {
        ClickSound();
        howToPlayPanle.Disable();
    }
    #endregion

    public void B_SoundSFX(float amount)
    {
        bool value = amount == 1;

        soundSliderBG.sprite = value ? sliderBGSpriteOn : sliderBGSpriteOff;

        SetSoundMixer(value);

        GameStats.gameData.sound = amount;
        DataManager.SaveData();
    }

    private void SetSoundMixer(bool value)
    {
        SetMixer(AudioManager.MixerUI, value);
        SetMixer(AudioManager.MixerSFX, value);
    }

    public void B_MusicSFX(float amount)
    {
        bool value = amount == 1;

        musicSliderBG.sprite = value ? sliderBGSpriteOn : sliderBGSpriteOff;

        SetMixer(AudioManager.MixerBGM, value);

        GameStats.gameData.music = amount;
        DataManager.SaveData();
    }

    private void SetMixer(string type, bool value)
    {
        _ = mixer.SetFloat(type, value ? -3 : -80);
    }

    public void B_ResetLevels()
    {
        ClickSound();
        DataManager.ResetLevelData();
        SetLevelButtons();
    }

    public void B_StartGame()
    {
        ClickSound();
        _ = allPanels.transform.DOMove(closeMainTrans.position, 1).SetUpdate(true)
            .OnComplete(() =>
            {
                GameStats.LoadDirectScene(GameStats.SceneGameplay);
            });
    }

    public void StartLevel(int level)
    {
        GameStats.level = level;
        B_StartGame();
    }

    public void B_ExitGame()
    {
        ClickSound();
        _ = StartCoroutine(ExitGameRoutine());
    }

    private IEnumerator ExitGameRoutine()
    {
        shuttingDownScreen.Enable();
        mainPanel.Disable();
        yield return Helper.GetRealWait(1);
        shuttingDownBlackImageGO.Enable();
        yield return Helper.GetRealWait(1);
        Helper.QuitGame();
    }

    public void ClickSound()
    {
        AudioManager.ClickSound();
    }
}
