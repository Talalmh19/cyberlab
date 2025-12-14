using UnityEngine;
using UnityEngine.UI;

public class MitigateButton : MonoBehaviour
{
    public Image tickImage;
    public Button button;

    public void DoneMitigation()
    {
        button.interactable = false;
        tickImage.enabled = true;
    }
}
