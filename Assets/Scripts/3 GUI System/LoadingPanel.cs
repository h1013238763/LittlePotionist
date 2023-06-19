using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadingPanel : PanelBase
{
    private Transform mask;
    private UnityAction next_action;

    public override void ShowSelf()
    {   
        // trigger animation
        mask = FindComponent<Image>("Mask").transform;
        TweenController.GetController().ChangeSizeTo(mask, new Vector3(0, 0, 0), 0.5f);        
    }

    public override void HideSelf()
    {
        TweenController.GetController().ChangeSizeTo(mask, new Vector3(1, 1, 0), 0.5f);
        MonoController.GetController().StartCoroutine(WaitForAnimation(0.5f, false));
    }

    public void AssignAction(UnityAction action)
    {
        next_action = action;
        MonoController.GetController().StartCoroutine(WaitForAnimation(0.5f, true)); 
    }

    private IEnumerator WaitForAnimation(float second, bool show_stage)
    {
        // finish wait for animation
        yield return new WaitForSeconds(second * 1.15f);

        // invoke loading action
        if(show_stage)
        {
            if(next_action != null)
                next_action.Invoke();
            HideSelf();
        }
        else
        {
            next_action = null;
            GUIController.GetController().HidePanel("LoadingPanel");
        }
    }
}
