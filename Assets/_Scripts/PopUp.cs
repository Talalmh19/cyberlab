using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public bool doPanelFade = false;
    public bool doPanelFullFade = false;
    public GameObject panel;
    private Image panelBG;

    private const float duration = 0.5f;

    private void OnEnable()
    {
        gameObject.transform.LocalScaleZero();
        _ = gameObject.transform.DOScale(1, duration)
            .SetUpdate(true);
        PanelFade(true);
    }

    public void B_ClosePopUp()
    {
        if (AudioManager.am != null)
        {
            AudioManager.am.Play(AudioManager.SoundClick);
        }

        ClosePopUp();
    }

    public void ClosePopUp()
    {
        gameObject.transform.LocalScaleOne();
        _ = gameObject.transform.DOScale(0, duration)
            .OnComplete(() =>
            {
                if (panel != null)
                {
                    panel.Disable();
                }
            })
            .SetUpdate(true);

        PanelFade(false);
    }

    private void PanelFade(bool fadeIn)
    {
        if (!doPanelFade) { return; }

        float fadeValue = fadeIn ? (doPanelFullFade ? 1 : 0.9f) : 0;
        float initValue = fadeIn ? 0 : (doPanelFullFade ? 1 : 0.9f);

        if (panel != null)
        {
            if (panelBG == null)
            {
                panelBG = panel.GetComponent<Image>();
            }

            panelBG.Fade(initValue);

            _ = panelBG.DOFade(fadeValue, duration)
            .SetUpdate(true);
        }
    }
}
