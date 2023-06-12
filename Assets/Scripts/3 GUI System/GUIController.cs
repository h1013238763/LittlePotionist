using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// GUI Controller Module
/// </summary>
public class GUIController : BaseController<GUIController>
{
    private Dictionary<string, PanelBase> panel_dic = new Dictionary<string, PanelBase>();

    public Transform canvas;
    private int canvas_layer_count = 0;

    public GUIController()
    {
        // Create Canvas Object
        GameObject obj = ResourceController.GetController().Load<GameObject>("GUI/General/Canvas");
        canvas = obj.transform;
        GameObject.DontDestroyOnLoad(obj);
        
        // Create Event Listener
        obj = ResourceController.GetController().Load<GameObject>("GUI/General/EventSystem");
        GameObject.DontDestroyOnLoad(obj);

        foreach( Transform child in canvas)
            canvas_layer_count ++;
    }

    /// <summary>
    /// Show Panel
    /// </summary>
    /// <param name="panel_name">name of panel</param>
    /// <param name="layer">to which layer</param>
    /// <param name="callback">calling function after panel shows</param>
    /// <typeparam name="T">type of panel</typeparam>
    public void ShowPanel<T>(string panel_name, int layer, UnityAction<T> callback) where T : PanelBase
    {
        // Avoid Panel repeat create
        if(panel_dic.ContainsKey(panel_name))
        {
            panel_dic[panel_name].ShowSelf();
            if(callback != null)
                callback(panel_dic[panel_name] as T);
            return;
        }

        // Avoid layer out of bound
        if(layer >= canvas_layer_count){
            return;
        }

        // LoadAsync panel, set layer and position
        ResourceController.GetController().LoadAsync<GameObject>("UI/Panels/" + panel_name, (obj) =>
        {
            obj.transform.SetParent(canvas.GetChild(layer));
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(1, 1, 1);

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = obj.GetComponent<T>();

            // 处理并储存面板
            if(callback != null)
                callback(panel);
            panel.ShowSelf();            
            panel_dic.Add(panel_name, panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panel_name">面板名称</param>
    public void HidePanel(string panel_name)
    {
        if(panel_dic.ContainsKey(panel_name))
        {
            GameObject.Destroy(panel_dic[panel_name].gameObject);
            panel_dic.Remove(panel_name);
        }
    }

    /// <summary>
    ///  获取一个已存在面板
    /// </summary>
    /// <param name="panel_name">面板名称</param>
    /// <returns>对应面板</returns>
    public T GetPanel<T>(string panel_name) where T : PanelBase
    {
        if(panel_dic.ContainsKey(panel_name))
            return panel_dic[panel_name] as T;
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件
    /// </summary>
    /// <param name="control">对应控件</param>
    /// <param name="type">事件类型</param>
    /// <param name="callback">响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}
