using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoBed : Decoration
{
    public override void Action(string name)
    {
        GUIController.GetController().ShowPanel<ConfirmPanel>("ConfirmPanel", 3, PanelAction);
    }

    private void PanelAction(ConfirmPanel panel)
    {
        panel.SetPanel("Sure go to sleep?", () => 
        {
            GUIController.GetController().ShowPanel<LoadingPanel>("LoadingPanel", 3);
            GUIController.GetController().GetPanel<LoadingPanel>("LoadingPanel").AssignAction(WorldController.GetController().NextDay);
        });
    }
}
