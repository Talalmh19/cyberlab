using TMPro;
using UnityEngine;

public class UIRuleButton : MonoBehaviour
{
    public int index;
    public bool apply;
    public GameObject parentGO;
    public TextMeshProUGUI buttonNameText;

    public void SetUIRuleButton(int num, string nameText, bool ruleTrue)
    {
        index = num;
        apply = ruleTrue;
        buttonNameText.text = nameText;
        parentGO.Enable();
    }

    public void B_ClickInfo()
    {
        UIGame.Instance.OpenRuleDetail(index);
    }

    public void B_Pick()
    {
        UIGame.Instance.PickRule(this);
    }

    public void B_Drop()
    {
        UIGame.Instance.DropRule();
    }
}
