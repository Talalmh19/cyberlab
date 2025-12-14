using System.Collections;
using TMPro;
using UnityEngine;

public class ResultFrame : MonoBehaviour
{
    [SerializeField]
    private PopUp popUp;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI pointsText;

    public void ShowResult(string resultText, string point, Color color, float closePanelTime = 2.5f)
    {
        pointsText.text = point;
        this.resultText.text = resultText;
        this.resultText.color = color;
        gameObject.Enable();
        _ = StartCoroutine(ClosePopUp(closePanelTime));
    }

    private IEnumerator ClosePopUp(float closePanelTime)
    {
        yield return Helper.GetRealWait(closePanelTime);
        if (GameStats.level == 1)
        {
            UIGame.Instance.SetSocialEngineeringMail();
        }
        else if (GameStats.level == 2)
        {
            if (UIGame.Instance.levelThreePahse == 0)
            {
                UIGame.Instance.SetEmail();
            }
        }
        popUp.panel = popUp.gameObject;
        popUp.ClosePopUp();
    }
}
