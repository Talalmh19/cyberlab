using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILogEntry : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI ipAddressText;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI buttonText1;
    public Button button;
    public Button button1;
    private int requestRate;
    private string ipAddress;

    public void SetLogEntry(GenerateLogEntry.LogEntry logEntry)
    {
        ipAddress = logEntry.IPAddress;
        requestRate = logEntry.RequestRate;

        //Log: IP - 10.0.0.6, Rate - 1800, Type - GET, Time - 12:20 PM
        ipAddressText.text = $"IP - {ipAddress}, Rate - {requestRate}, Type - {logEntry.RequestType}, Time - {logEntry.TimeStamp}";

        gameObject.Enable();
    }

    public void B_IsSafe(bool safe)
    {
        button.interactable = false;
        button1.interactable = false;

        if (safe)
        {
            buttonText.text = "Marked Safe";
            buttonText1.text = "Marked Safe";
        }
        else
        {
            buttonText.text = "Marked Malicious";
            buttonText1.text = "Marked Malicious";
        }

        UIGame.Instance.AnalyzeTraffic(ipAddress, requestRate, safe);
    }
}
