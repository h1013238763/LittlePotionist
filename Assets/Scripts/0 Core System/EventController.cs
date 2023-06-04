using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心模块
/// </summary>
public class EventController : BaseController<EventController>
{
    // 创建监听列表
    private Dictionary<string, IEventInfo> event_dic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    /// <param name="action"> 触发方法 </param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // 查找对应监听
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo<T>).actions += action;
        else
            event_dic.Add(name, new EventInfo<T>( action ));
    }

    /// <summary>
    /// 添加无参事件监听
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    /// <param name="action"> 触发方法 </param>
    public void AddEventListener(string name, UnityAction action)
    {
        // 查找对应监听
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo).actions += action;
        else
            event_dic.Add(name, new EventInfo( action ));
    }

    /// <summary>
    /// 事件被触发
    /// </summary>
    /// <param name="name">事件名称</param>
    public void EventTrigger<T>(string name, T info)
    {
        // 查找对应监听
        if(event_dic.ContainsKey(name))
        {
            if((event_dic[name] as EventInfo<T>).actions != null)
                (event_dic[name] as EventInfo<T>).actions.Invoke(info);
        }
    }

    /// <summary>
    /// 无参事件触发
    /// </summary>
    /// <param name="name">事件名称</param>
    public void EventTrigger(string name)
    {
        // 查找对应监听
        if(event_dic.ContainsKey(name))
        {
            if((event_dic[name] as EventInfo).actions != null)
                (event_dic[name] as EventInfo).actions.Invoke();
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    /// <param name="action"> 触发方法 </param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo<T>).actions-= action;
    }

    /// <summary>
    /// 移除无参事件监听
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    /// <param name="action"> 触发方法 </param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo).actions-= action;
    }

    /// <summary>
    /// 清空事件监听
    /// </summary>
    public void Clear()
    {
        event_dic.Clear();
    }
}

/// <summary>
/// 包装用接口和类
/// </summary>
public interface IEventInfo{}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo( UnityAction<T> action )
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo( UnityAction action )
    {
        actions += action;
    }
}