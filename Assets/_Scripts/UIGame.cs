using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGame : MonoBehaviour, IUpdateObserver
{
    public static UIGame Instance { get; private set; }

    public GameObject allPanels;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject wonPanel;
    public GameObject losePanel;
    public GameObject puzzlePanel;

    public GameObject rightQuizFrame;
    public GameObject wrongQuizFrame;
    public GameObject timerOverQuizFrame;
    public GameObject level4ObjectivesFrame;
    public GameObject logEntryFrame;

    public GameObject objectiveLevel4ButtonGO;

    public GameObject emailTabGO;
    public Transform pcScreenTrans;
    public Transform closeMainTrans;
    public Transform finalTrans;
    public Transform sandBoxTrans;
    public Transform safeBoxTrans;
    public Transform localRuleZoneTrans;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI frameTitleText;
    public TextMeshProUGUI personNameText;
    public TextMeshProUGUI emailAddressText;
    public TextMeshProUGUI emailContentText;
    public TextMeshProUGUI emailCounterText;

    public TextMeshProUGUI quizNumberText;
    public TextMeshProUGUI quizQustionText;
    public TextMeshProUGUI quizOptionOneText;
    public TextMeshProUGUI quizOptionTwoText;
    public TextMeshProUGUI quizTimerText;

    public TextMeshProUGUI socialTimerText;

    public TextMeshProUGUI logEntryPromptText;

    public TextMeshProUGUI pointsText;

    public TextMeshProUGUI levelObjectiveText;

    public Transform initalShowEmailTrans;
    public Transform finalShowEmailTrans;
    public Button showEmailButton;

    public GameObject phishing_ButtonsTab;

    public GameObject se_ButtonsTab;

    public GameObject scan_ButtonsTab;

    public ResultFrame resultFrame;

    public GameObject fileAnalyzeFrame;
    public GameObject level4PuzzleFrame;
    public GameObject puzzleDeatailFrame;
    public TextMeshProUGUI deatilText;
    public Image ruleBoxGlow;

    public GameObject level4ApplayToolFrame;
    public GameObject level4AnalysisFrame;
    public GameObject level4OutComesFrame;
    public TextMeshProUGUI serverRiskText;
    public Button[] level4ToolButtons;
    public Image[] level4ToolButtonTicks;

    public GameObject loadingFrame;
    public Slider loadingSlider;
    public Slider serverRiskSlider;
    public TextMeshProUGUI loadingText;
    public GameObject mitigationFrame;

    public UILogEntry[] uILogEntries;
    public UIRuleButton[] uIRuleButtons;

    public readonly List<string> globalRules = new()
    {
        "Block IP",
        "Rate Limit",
        "Allow IP Range",
        "Deny All Except Trusted",
        "Geo-Block",
        "Disable Unused Ports",
        "Enable Protocol Filtering",
        "Allow All Traffic",
        "Block Legitimate Traffic",
        "Disable Firewall",
        "Increase Rate Limit",
        "Enable Telnet Access",
        "Open All Ports",
        "Disable HTTPS Only",
        "Whitelist Suspicious IP",
    };
    public readonly List<string> globalRuleDetails = new()
    {
        "Blocks specific IPs identified as malicious from earlier traffic analysis.",
        "Restricts the number of requests a source can make within a given timeframe to prevent overload.",
        "Ensures legitimate traffic from internal networks or trusted sources isn't blocked.",
        "Restricts access to only trusted subnets, reducing attack surface.",
        "Blocks traffic from regions identified as attack origins.",
        "Shuts down unnecessary ports that could be exploited during an attack.",
        "Ensures only secure, encrypted traffic is allowed.",
        "Completely opens up the network, allowing malicious traffic to overwhelm it.",
        "Blocks an internal IP or a trusted source, disrupting legitimate services.",
        "Removes all protection, leaving the network exposed to attacks.",
        "Increases the traffic threshold, worsening the attack's impact.",
        "Activates an outdated and insecure protocol, increasing vulnerability.",
        "Leaves every port open, making the system a target for other attacks.",
        "Allows unencrypted traffic, which attackers can intercept and exploit.",
        "Permits an IP flagged as malicious, potentially exacerbating the attack.",
    };
    public readonly List<bool> rulesCorrectness = new()
    {
        true, true, true, true, true, true, true, false, false, false, false, false, false, false, false,
    };
    private readonly List<int> ruleSortOrder = new()
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
    };
    private int LevelOneEmailTarget = 10;
    private int levelOneEmailCurrent;
    private const int QuizQustionTarget = 5;
    private int quizQustionCurrent;
    private Coroutine quizTimerRoutine;
    private Coroutine seTimerRoutine;
    [SerializeField]
    private int points;
    private int requestRateThresh;

    private void Awake()
    {
        Instance = this;
        AlphaUpdate.Instance.RegisterObserver(this);
    }

    private void OnDestroy()
    {
        AlphaUpdate.Instance.UnregisterObserver(this);
    }

    private void Start()
    {
        Time.timeScale = 1;
        //GameStats.level = 3;

        if (GameStats.level is 0 or 1)
        {
            LevelOneEmailTarget = 10;
        }
        else if (GameStats.level == 2)
        {
            LevelOneEmailTarget = 5;
        }
        else if (GameStats.level == 3)
        {
            SetLogEntry();
            SetRuleList();
        }

        levelText.text = $"Level {GameStats.level + 1}";

        _ = allPanels.transform.DOMove(closeMainTrans.position, 0).SetUpdate(true)
            .OnComplete(() =>
            {
                allPanels.transform.DOMove(finalTrans.position, 1).SetUpdate(true)
                .OnComplete(() =>
                {
                    _ = showEmailButton.transform.DOMove(initalShowEmailTrans.position, 0).SetUpdate(true)
                    .OnComplete(() =>
                    {
                        _ = showEmailButton.transform.DOMove(finalShowEmailTrans.position, 1).SetUpdate(true);
                    });
                });
            });
    }

    int ruleListCount;
    private void SetRuleList()
    {
        Helper.Shuffle(ruleSortOrder);

        ruleListCount = UnityEngine.Random.Range(7, uIRuleButtons.Length - 5);

        for (int i = 0; i < ruleListCount; i++)
        {
            int j = ruleSortOrder[i];

            uIRuleButtons[i].SetUIRuleButton(j, globalRules[j], rulesCorrectness[j]);
        }
    }

    public void OpenRuleDetail(int index)
    {
        deatilText.text = globalRuleDetails[index];
        puzzleDeatailFrame.Enable();
    }

    int entryLogCount;
    private void SetLogEntry()
    {
        requestRateThresh = UnityEngine.Random.Range(500, 1501);

        logEntryPromptText.text = $"You must identify suspicious IPs based on unusually high request rates or patterns (e.g., if request rate is greater then {requestRateThresh} then it is warning sign).";

        entryLogCount = 5;// UnityEngine.Random.Range(7, uILogEntries.Length);

        for (int i = 0; i < entryLogCount; i++)
        {
            GenerateLogEntry.LogEntry logEntry = GenerateLogEntry.GenerateRandomLogEntry();
            uILogEntries[i].SetLogEntry(logEntry);
        }
    }

    public void AnalyzeTraffic(string ip, int requestRate, bool isSafe)
    {
        if (!isSafe)
        {
            if (requestRate > requestRateThresh)
            {
                points += 10;
                resultFrame.ShowResult($"IP {ip} marked as malicious. Traffic blocked!", "+10", Color.green);
            }
            else
            {
                points -= 5;
                resultFrame.ShowResult($"IP {ip} marked incorrectly. Double-check your logs.", "-5", Color.red);
            }
        }
        else
        {
            if (requestRate <= 1000)
            {
                points += 10;
                resultFrame.ShowResult($"IP {ip} marked as Safe.", "+10", Color.green);
            }
            else
            {
                points -= 5;
                resultFrame.ShowResult($"IP {ip} marked incorrectly. Double-check your logs.", "-5", Color.red);
            }
        }

        entryLogCount--;

        if (entryLogCount <= 0)
        {
            logEntryFrame.Disable();
            level4PuzzleFrame.Enable();
        }
    }

    public void B_GamePause()
    {
        ClickSound();
        Time.timeScale = 0;
        pausePanel.Enable();
    }

    public void B_GameResume()
    {
        ClickSound();
        Invoke(nameof(SetTimeScaleOne), 0.5f);
    }

    private void SetTimeScaleOne()
    {
        Time.timeScale = 1;
    }

    public void B_GameRestart()
    {
        ClickSound();
        MovePanelAndLoadScene(GameStats.SceneGameplay);
    }

    public void B_GameHome()
    {
        ClickSound();
        MovePanelAndLoadScene(GameStats.SceneMainMenu);
    }

    private void MovePanelAndLoadScene(string sceneName)
    {
        _ = allPanels.transform.DOMove(closeMainTrans.position, 1)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                GameStats.LoadDirectScene(sceneName);
            });
    }

    public void B_GamePuzzle()
    {
        ClickSound();
        Time.timeScale = 1;
        puzzlePanel.Enable();
        SetQuizAndTimer();
    }

    public void B_ShowEmail()
    {
        ClickSound();
        _ = showEmailButton.transform.DOMove(finalShowEmailTrans.position, 0).SetUpdate(true)
            .OnComplete(() =>
            {
                showEmailButton.transform.DOMove(initalShowEmailTrans.position, 1).SetUpdate(true);
            });

        showEmailButton.interactable = false;
        if (GameStats.level == 0)
        {
            frameTitleText.text = "New Email Received";
            se_ButtonsTab.Disable();
            phishing_ButtonsTab.Enable();
            B_NextEmail();
        }
        else if (GameStats.level == 1)
        {
            frameTitleText.text = "Social Message Received";
            ShowSocialEngineeringMail();
        }
        else if (GameStats.level == 2)
        {
            frameTitleText.text = "Scan Email Header and Links";
            ShowEmailToScan();
        }
        else if (GameStats.level == 3)
        {
            frameTitleText.text = "DDoS attack";

            personNameText.text = "Administration";
            emailAddressText.text = "administration@example.com";

            emailContentText.text = "The company’s website is under a sudden surge of traffic, disrupting services. You as the network administrator, must identify and mitigate the Distributed Denial of Service (DDoS) attack.";

            emailTabGO.Enable();
            phishing_ButtonsTab.Disable();
            Invoke(nameof(EnableObjectiveButton), 3f);
        }
        else if (GameStats.level == 4)
        {
            frameTitleText.text = "MitM attack";

            personNameText.text = "Administration";
            emailAddressText.text = "administration@example.com";

            emailContentText.text = "While employees are working remotely, an attacker intercepts their communication over an unsecured Wi-Fi network. You must identify and secure the communication channel.";

            levelObjectiveText.text = "Detect and neutralize the MitM (Main-in-the-Middle) attack while educating employees on safe network practices.";

            emailTabGO.Enable();
            phishing_ButtonsTab.Disable();
            Invoke(nameof(EnableObjectiveButton), 3f);
        }
    }

    private void EnableObjectiveButton()
    {
        objectiveLevel4ButtonGO.Enable();
    }

    public void B_Level4Objectives()
    {
        emailTabGO.Disable();
        level4ObjectivesFrame.Enable();
    }

    public void B_OpenLogEntryFrame()
    {
        level4ObjectivesFrame.Disable();
        logEntryFrame.Enable();
    }

    public void ShowEmailToScan()
    {
        scan_ButtonsTab.Enable();
        phishing_ButtonsTab.Disable();
        emailTabGO.Enable();
        SetEmail();
    }

    public void ShowSocialEngineeringMail()
    {
        se_ButtonsTab.Enable();
        phishing_ButtonsTab.Disable();
        emailTabGO.Enable();

        SetSocialEngineeringMail();
    }

    public void B_NextEmail()
    {
        emailTabGO.Enable();
        SetEmail();
    }

    private bool isRealEamil;
    public void SetEmail()
    {
        if (GameStats.level == 2)
        {
            allowSocailButtonPress = false;
            if (levelOneEmailCurrent >= LevelOneEmailTarget)
            {
                // next Network Map: Quarantine System
                levelThreePahse++;
                fileAnalyzeFrame.Enable();
                return;
            }
        }

        levelOneEmailCurrent++;

        emailContentText.text = $"{levelOneEmailCurrent}/{LevelOneEmailTarget}";

        isRealEamil = false;

        personNameText.text = NameGenerator.GetRandomMaleName;
        emailAddressText.text = EmailAddressGenerator.GetRandomEmail;

        isRealEamil = Helper.Chance50;

        emailContentText.text = isRealEamil ? GenerateEmails.GetRandomGenuineEmail :
            GenerateEmails.GetRandomPhishingEmail;
    }

    private GenerateEmails.EmailScenario currentScenarioPoints;
    public void SetSocialEngineeringMail()
    {
        allowSocailButtonPress = false;
        if (levelOneEmailCurrent >= LevelOneEmailTarget)
        {
            Time.timeScale = 0;
            GameWon();
            return;
        }

        levelOneEmailCurrent++;

        emailContentText.text = $"{levelOneEmailCurrent}/{LevelOneEmailTarget}";
        personNameText.text = NameGenerator.GetRandomMaleName;
        emailAddressText.text = EmailAddressGenerator.GetRandomEmail;

        currentScenarioPoints = GenerateEmails.GetRandomScenario;

        emailContentText.text = currentScenarioPoints.Content;

        RandomizeButtonOrder();
        StartSETimer();
    }

    [SerializeField] private RectTransform[] buttons;
    private void RandomizeButtonOrder()
    {
        //foreach (var button in buttons)
        //{
        //    button.SetSiblingIndex(UnityEngine.Random.Range(0, buttons.Length));
        //}

        for (int i = buttons.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            buttons[i].SetSiblingIndex(randomIndex);
            buttons[randomIndex].SetSiblingIndex(i);
        }
    }

    private bool seHaveTime;
    private void StartSETimer(float initialDealy = 0)
    {
        seHaveTime = true;
        seTimerRoutine = StartCoroutine(SE_TimerRoutine(initialDealy));
    }

    private IEnumerator SE_TimerRoutine(float initialDealy = 0)
    {
        const float totalTimeAmount = 10;
        float timeAmount = totalTimeAmount;
        SetSETimerText(timeAmount);

        if (initialDealy > 0)
        {
            yield return Helper.GetWait(initialDealy);
        }

        while (seHaveTime)
        {
            yield return Helper.GetWait(1.00f);
            timeAmount--;
            SetSETimerText(timeAmount);

            if (timeAmount <= 0)
            {
                allowSocailButtonPress = true;
                seHaveTime = false;
                points -= 5;
                resultFrame.ShowResult("No decision made in time! Penalty applied.", $"-{5} points", Color.red);
            }
        }
    }

    private void SetSETimerText(float timeAmount)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeAmount);
        socialTimerText.text = timeSpan.ToString(@"mm\:ss");
    }

    bool allowSocailButtonPress;
    public void B_SE_AllowAccess()
    {
        ClickSound();
        if (allowSocailButtonPress) { return; }
        StopSocialTimerRoutine();

        points += currentScenarioPoints.AllowAccessPoints;
        resultFrame.ShowResult(
            currentScenarioPoints.AllowAccessPoints < 0 ? "Incorrect! You’ve allowed a potential attacker." : "Good! Access was granted correctly.",
            $"{currentScenarioPoints.AllowAccessPoints} points",
            currentScenarioPoints.AllowAccessPoints < 0 ? Color.red : Color.green
        );
    }

    public void B_SE_AskForID()
    {
        ClickSound();
        if (allowSocailButtonPress) { return; }
        StopSocialTimerRoutine();

        points += currentScenarioPoints.AskForIDPoints;
        resultFrame.ShowResult(
            currentScenarioPoints.AskForIDPoints > 0 ? "Good! Always verify identity before granting access." : "Not ideal, but you verified.",
            $"{currentScenarioPoints.AskForIDPoints} points",
            currentScenarioPoints.AskForIDPoints > 0 ? Color.green : Color.yellow
        );
    }

    public void B_SE_DenyAccess()
    {
        ClickSound();
        if (allowSocailButtonPress) { return; }
        StopSocialTimerRoutine();

        points += currentScenarioPoints.DenyAccessPoints;
        resultFrame.ShowResult(
            currentScenarioPoints.DenyAccessPoints > 0 ? "Correct! Denying access was the safest choice." : "Not always the best decision.",
            $"{currentScenarioPoints.DenyAccessPoints} points",
            currentScenarioPoints.DenyAccessPoints > 0 ? Color.green : Color.yellow
        );
    }

    private void StopSocialTimerRoutine()
    {
        allowSocailButtonPress = true;

        if (seTimerRoutine != null)
        {
            StopCoroutine(seTimerRoutine);
        }
    }

    public void B_Scan_ScanEmail()
    {
        if (!isRealEamil)
        {
            points += 10;
            resultFrame.ShowResult("Correct! The email is malicious.", "+10 points", Color.green);
        }
        else
        {
            points -= 5;
            resultFrame.ShowResult("Incorrect! The email is safe.", "-5 points", Color.red);
        }
    }

    public void B_Scan_AnalyzeLink()
    {
        if (!isRealEamil)
        {
            points += 10;
            resultFrame.ShowResult("Correct! The link is malicious.", "+10 points", Color.green);
        }
        else
        {
            points -= 5;
            resultFrame.ShowResult("Incorrect! The link is safe.", "-5 points", Color.red);
        }
    }

    public void B_Scan_Ignore()
    {
        if (isRealEamil)
        {
            points += 10;
            resultFrame.ShowResult("Correct! The Mail is safe.", "+10 points", Color.green);
        }
        else
        {
            points -= 5;
            resultFrame.ShowResult("Incorrect! The Mail is malicious.", "-5 points", Color.red);
        }
    }

    public void B_RealEmail()
    {
        HandleEmail(true);
    }

    public void B_PhishingEmail()
    {
        HandleEmail(false);
    }

    private void HandleEmail(bool isReal)
    {
        ClickSound();
        Time.timeScale = 0;
        emailTabGO.Disable();

        if (isReal == isRealEamil)
        {
            points += 20;
            SetEmailOrWinPanel();
        }
        else
        {
            points -= 10;
            if (points < 0) { points = 0; }
            levelOneEmailCurrent--;
            losePanel.Enable();
        }
    }

    private void SetQuizAndTimer()
    {
        rightQuizFrame.Disable();
        wrongQuizFrame.Disable();
        timerOverQuizFrame.Disable();

        if (quizQustionCurrent >= QuizQustionTarget)
        {
            quizQustionCurrent = 0;
            puzzlePanel.Disable();
            losePanel.Disable();
            SetEmailOrWinPanel();
            return;
        }

        quizQustionCurrent++;

        SetPuzzleQustion();
        StartQuizTimer();
    }

    private void SetEmailOrWinPanel()
    {
        if (levelOneEmailCurrent >= LevelOneEmailTarget)
        {
            GameWon();
        }
        else
        {
            B_NextEmail();
        }
    }

    private void GameWon()
    {
        if (points < 0) { points = 0; }

        pointsText.text = $"+{points} points";

        if (!GameStats.gameData.playedLevel[GameStats.level])
        {
            GameStats.gameData.playedLevel[GameStats.level] = true;
        }

        GameStats.gameData.points += points;
        DataManager.SaveData();
        wonPanel.Enable();
    }

    private int correctOption;
    private int quizNumber;
    private void SetPuzzleQustion()
    {
        quizNumber++;
        quizNumberText.text = $"Quiz {quizNumber}";

        var quiz = CyberSecurityQuiz.GetRandomQuizQuestion;

        quizQustionText.text = quiz.Question;

        correctOption = Helper.Chance50 ? 1 : 2;

        if (correctOption == 1)
        {
            quizOptionOneText.text = quiz.Option1;
            quizOptionTwoText.text = quiz.Option2;
        }
        else
        {
            quizOptionOneText.text = quiz.Option2;
            quizOptionTwoText.text = quiz.Option1;
        }
    }

    public void B_PuzzleOption(int option)
    {
        ClickSound();
        option++;

        if (quizTimerRoutine != null)
        {
            StopCoroutine(quizTimerRoutine);
        }

        bool isCorrect = option == correctOption;

        rightQuizFrame.SetActive(isCorrect);
        wrongQuizFrame.SetActive(option != correctOption);

        Invoke(nameof(SetQuizAndTimer), 2);
    }

    private bool quizHaveTime;
    private void StartQuizTimer(float initialDealy = 0)
    {
        quizHaveTime = true;
        quizTimerRoutine = StartCoroutine(TimerRoutine(initialDealy));
    }

    private IEnumerator TimerRoutine(float initialDealy = 0)
    {
        const float totalTimeAmount = 10;
        float timeAmount = totalTimeAmount;
        SetTimerText(timeAmount);

        if (initialDealy > 0)
        {
            yield return Helper.GetWait(initialDealy);
        }

        while (quizHaveTime)
        {
            yield return Helper.GetWait(1.00f);
            timeAmount--;
            SetTimerText(timeAmount);

            if (timeAmount <= 0)
            {
                quizHaveTime = false;
                timerOverQuizFrame.Enable();
            }
        }

        yield return Helper.GetWait(2);
        SetQuizAndTimer();
    }

    private void SetTimerText(float timeAmount)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeAmount);
        quizTimerText.text = timeSpan.ToString(@"mm\:ss");
    }

    public void ClickSound()
    {
        AudioManager.ClickSound();
    }

    #region Drag Drop
    bool ruleAllow;
    [SerializeField]
    private UIRuleButton dragableUIRuleButton;
    public void PickRule(UIRuleButton uIRule)
    {
        if (inAnimation) { return; }

        dragableUIRuleButton = uIRule;
        ruleAllow = uIRule.apply;
        initialPos = uIRule.transform.position;
    }

    public void DropRule()
    {
        if (dragableUIRuleButton == null) { return; }

        if (dropable)
        {
            inAnimation = true;
            dragableUIRuleButton.transform.position = localRuleZoneTrans.position;

            dragableUIRuleButton.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                dragableUIRuleButton.parentGO.Disable();
                dragableUIRuleButton = null;
                inAnimation = false;

                if (!ruleAllow)
                {
                    points -= 5;
                    resultFrame.ShowResult("Incorrect Rule Apply!", "-5 points", Color.red);
                }
                else
                {
                    points += 5;
                    resultFrame.ShowResult("Rule Apply Correctly!", "+5 points", Color.green);
                }
                ruleAllow = false;
            });

            return;
        }

        dragableUIRuleButton.transform.position = initialPos;
        dragableUIRuleButton = null;
    }

    public void B_Rule_ApplyRule()
    {
        resultFrame.ShowResult("Rule Applied Successfully!", "", Color.white);
        level4PuzzleFrame.Disable();

        Invoke(nameof(EnableToolsLevel4), 3f);
    }

    int serverLoad = 100;
    int serverToolCount = 0;
    public void B_ApplyToolLevel4(int index)
    {
        level4ToolButtons[index].interactable = false;
        level4ToolButtonTicks[index].enabled = true;

        if (index == 0)
        {
            serverLoad -= 30;
            serverToolCount++;
        }
        else if (index == 1)
        {
            serverLoad -= 20;
            serverToolCount++;
        }
        else
        {
            serverLoad -= 40;
            serverToolCount++;
        }

        _ = serverRiskSlider.DOValue(serverToolCount <= 1 ? 60 : serverToolCount <= 2 ? 50 : 40
            , 0.5f).OnComplete(() =>
        {
            serverRiskSlider.fillRect.GetComponent<Image>().color = serverToolCount <= 1 ?
            Color.yellow : serverToolCount <= 2 ? Color.blue : Color.green;

            int riskSliderValur = serverToolCount <= 1 ? 60 : serverToolCount <= 2 ? 50 : 40;
            serverRiskText.text = $"Server Risk {riskSliderValur}%";

            if (serverLoad <= 11)
            {
                points += 30;
                Invoke(nameof(EnableResultForToolApplay), 0.5f);
            }
        });
    }

    private void EnableResultForToolApplay()
    {
        level4ApplayToolFrame.Disable();
        resultFrame.ShowResult("Server is stable! DDoS mitigated.!", "+30 points", Color.green);
        Invoke(nameof(EnableLevel4AnalysisFrame), 3.0f);
    }

    private void EnableLevel4AnalysisFrame()
    {
        level4AnalysisFrame.Enable();
    }

    public void B_Level4ContinueToEducationalOutcomes()
    {
        level4OutComesFrame.Enable();
    }

    public void B_Level4ReadOutComes()
    {
        GameWon();
    }

    private void EnableToolsLevel4()
    {
        level4ApplayToolFrame.Enable();
    }

    private Vector3 initialPos;
    [SerializeField]
    private GameObject dragableFile;
    public void B_FileAnalyze_Pick(GameObject file)
    {
        if (inAnimation) { return; }

        dragableFile = file;
        initialPos = dragableFile.transform.position;
    }

    public void OnUpdate()
    {
        if (inAnimation) { return; }

        if (GameStats.level == 2)
        {
            if (dragableFile != null)
            {
                Vector3 mousePos = Input.mousePosition;

                mousePos.z = dragableFile.transform.position.z;

                dragableFile.transform.position = mousePos;

                float distance = Vector3.Distance(sandBoxTrans.position, mousePos);
                float distance1 = Vector3.Distance(safeBoxTrans.position, mousePos);

                if (distance < 200)
                {
                    dropable = true;
                    sandBoxTrans.localScale = Vector3.one * 1.1f;
                }
                else
                {
                    dropable = false;
                    sandBoxTrans.localScale = Vector3.one;
                }

                if (distance1 < 200)
                {
                    safeable = true;
                    safeBoxTrans.localScale = Vector3.one * 1.1f;
                }
                else
                {
                    safeable = false;
                    safeBoxTrans.localScale = Vector3.one;
                }
            }
            else
            {
                dropable = false;
                safeable = false;
                sandBoxTrans.localScale = Vector3.one;
                safeBoxTrans.localScale = Vector3.one;
            }
        }

        if (GameStats.level == 3)
        {
            if (dragableUIRuleButton != null)
            {
                Vector3 mousePos = Input.mousePosition;

                mousePos.z = dragableUIRuleButton.transform.position.z;

                dragableUIRuleButton.transform.position = mousePos;

                float distance = Vector3.Distance(localRuleZoneTrans.position, mousePos);

                if (distance < 150)
                {
                    dropable = true;
                    ruleBoxGlow.color = dragableUIRuleButton.apply ? Color.green : Color.red;
                    ruleBoxGlow.enabled = true;
                }
                else
                {
                    dropable = false;
                    ruleBoxGlow.enabled = false;
                }
            }
            else
            {
                ruleBoxGlow.enabled = false;
            }
        }
    }

    public int levelThreePahse;
    int fileAlalyzeCount = 4;
    bool dropable;
    bool safeable;
    bool inAnimation;
    AnalyzeFile analyzeFile;
    public void B_FileAnalyze_Drop(BaseEventData eventData)
    {
        if (dragableFile == null) { return; }

        if (dropable)
        {
            inAnimation = true;
            dragableFile.transform.position = sandBoxTrans.position;

            dragableFile.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                dragableFile.Disable();
                analyzeFile = dragableFile.GetComponent<AnalyzeFile>();
                dragableFile = null;
                DoUploadingAndAnalyzing();
            });

            return;
        }

        if (safeable)
        {
            inAnimation = true;
            dragableFile.transform.position = safeBoxTrans.position;

            dragableFile.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                dragableFile.Disable();
                analyzeFile = dragableFile.GetComponent<AnalyzeFile>();
                dragableFile = null;
                DoUploadingAndAnalyzing(true);
            });

            return;
        }

        dragableFile.transform.position = initialPos;
        dragableFile = null;
    }

    private void DoUploadingAndAnalyzing(bool safeable = false)
    {
        loadingSlider.value = 0.05f;
        loadingText.text = "Uploading...";
        loadingFrame.Enable();

        _ = loadingSlider.DOValue(1, 3)
            .OnComplete(() =>
            {
                if (safeable)
                {
                    loadingFrame.Disable();

                    if (analyzeFile.isMalwhere)
                    {
                        points -= 20;
                        resultFrame.ShowResult("File was a malware!", "-20 points", Color.red);
                    }
                    else
                    {
                        points += 5;
                        resultFrame.ShowResult("This is safe.", "+5 points", Color.green);
                    }

                    fileAlalyzeCount--;
                    if (fileAlalyzeCount <= 0)
                    {
                        mitigationFrame.Enable();
                    }
                    inAnimation = false;
                    return;
                }

                loadingSlider.value = 0.05f;
                loadingText.text = "Analyzing...";

                _ = loadingSlider.DOValue(1, 4)
                .OnComplete(() =>
                {
                    loadingFrame.Disable();

                    if (analyzeFile.isMalwhere)
                    {
                        points += 20;
                        resultFrame.ShowResult("File is confirmed as malware!", "+20 points", Color.green);
                    }
                    else
                    {
                        points -= 5;
                        resultFrame.ShowResult("This file is clean.", "-5 points", Color.red);
                    }

                    fileAlalyzeCount--;
                    if (fileAlalyzeCount <= 0)
                    {
                        mitigationFrame.Enable();
                    }
                    inAnimation = false;
                });
            });
    }
    #endregion

    #region Mitigation
    int mitigationCount;
    public void B_Mitigation_UpdateAntivirus(MitigateButton mitigateButton)
    {
        DoMitigation("Update Antivirus", 5, mitigateButton);
    }

    public void B_Mitigation_RestoreBackups(MitigateButton mitigateButton)
    {
        DoMitigation("Restore Backups", 5, mitigateButton);
    }

    public void B_Mitigation_EducateEmployees(MitigateButton mitigateButton)
    {
        DoMitigation("Educate Employees", 2, mitigateButton);
    }

    private void DoMitigation(string loadingString, float loadingTime, MitigateButton mitigateButton)
    {
        mitigationCount++;
        loadingSlider.value = 0.05f;
        loadingText.text = loadingString;
        loadingFrame.Enable();

        _ = loadingSlider.DOValue(1, loadingTime)
            .OnComplete(() =>
            {
                loadingFrame.Disable();

                switch (loadingString)
                {
                    case "Update Antivirus":
                        points += 10;
                        resultFrame.ShowResult("Antivirus software has been updated to detect and block known malware. However, it may not fix existing infections.\\nThis action helps in future prevention but doesn’t address the current situation.", "+5 points", Color.green, 6);
                        break;
                    case "Restore Backups":
                        points += 20;
                        resultFrame.ShowResult("Infected systems have been restored using clean backups. This ensures the systems are malware-free.\nEffective for recovery, but ensure proper scanning to avoid reinfection.", "+20 points.", Color.green, 5f);
                        break;
                    case "Educate Employees":
                        points += 15;
                        resultFrame.ShowResult("Employees are now trained to recognize phishing and malware tactics. Awareness reduces risks of future infections.\nAn informed workforce strengthens your organization’s defenses but doesn’t directly fix infected systems.", "+15 points.", Color.green, 7f);
                        break;
                    default:
                        points -= 5;
                        resultFrame.ShowResult("Invalid action.", "-5 points.", Color.red);
                        break;
                }

                mitigateButton.DoneMitigation();

                if (mitigationCount >= 3)
                {
                    GameWon();
                }
            });
    }
    #endregion
}
