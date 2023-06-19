using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmPanel : PanelBase
{
    public UnityAction action;

    protected override void OnClick(string button_name)
    {
        switch(button_name)
        {
            case "YesButton":
                action.Invoke();
                break;
            case "NoButton":
                HideSelf();
                break;
            default:
                break;
        }
    }

    public override void HideSelf()
    {
        action = null;
        GUIController.GetController().HidePanel("ConfirmPanel");
    }

    public void SetPanel(string text, UnityAction action)
    {
        FindComponent<Text>("ConfirmText").text = text;
        this.action = action;
    }
}
