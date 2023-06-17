using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : PanelBase
{
    private Transform mask;

    public override void ShowSelf()
    {
        mask = FindComponent<Image>("Mask").transform;

        TweenController.GetController().ChangeSizeTo(mask, new Vector3(0, 0, 0), 0.5f);
    }

    public override void HideSelf()
    {
        TweenController.GetController().ChangeSizeTo(mask, new Vector3(1, 1, 0), 0.5f);
    }
}
