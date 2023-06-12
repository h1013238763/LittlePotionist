using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePanel : PanelBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnClick(string button_name)
    {
        switch(button_name)
        {
            case "BuildModeBtn":
                if(!EventController.GetController().ContainsEvent("EnterBuildingMode"))
                    BuildController.GetController().Initial();
                EventController.GetController().EventTrigger("EnterBuildingMode");
                break;
            default:
                break;
        }
    }
}
