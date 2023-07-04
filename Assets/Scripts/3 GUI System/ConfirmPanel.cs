using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmPanel : PanelBase
{
    protected override void OnButtonClick(string button_name)
    {
        AudioController.Controller().PlaySound("ButtonClick");

        if(button_name == "YesButton")
            EventController.Controller().EventTrigger("ConfirmPanelEvent");
        else
            EventController.Controller().RemoveEventKey("ConfirmPanelEvent");

        GUIController.Controller().RemovePanel("ConfirmPanel");
    }

    public override void ShowSelf()
    {
        RectTransform cover = FindComponent<Image>("Cover").gameObject.GetComponent<RectTransform>();
        cover.sizeDelta = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        cover.position = new Vector3(0, 0, 0);

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Set the panel text and trigger event
    /// </summary>
    /// <param name="text"></param>
    /// <param name="action"></param>
    public void SetPanel(string text)
    {
        FindComponent<Text>("ConfirmText").text = text;
    }
}
