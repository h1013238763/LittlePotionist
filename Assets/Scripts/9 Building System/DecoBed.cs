using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoBed : Decoration
{
    public override void Action(string name)
    {
        GUIController.Controller().ShowPanel<ConfirmPanel>("ConfirmPanel", 3, PanelAction);
    }

    private void PanelAction(ConfirmPanel panel)
    {
        
    }
}
