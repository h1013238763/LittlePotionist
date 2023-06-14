using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event System Module
/// </summary>
public class EventController : BaseController<EventController>
{
    // Create Event Dictionary
    private Dictionary<string, IEventInfo> event_dic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// Add Event
    /// </summary>
    /// <param name="name"> name of trigger </param>
    /// <param name="action"> functions </param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // Find Target Event
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo<T>).actions += action;
        else
            event_dic.Add(name, new EventInfo<T>( action ));
    }

    /// <summary>
    /// Add no-parameter Event
    /// </summary>
    /// <param name="name"> name of trigger </param>
    /// <param name="action"> functions </param>
    public void AddEventListener(string name, UnityAction action)
    {
        // Find Target Event
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo).actions += action;
        else
            event_dic.Add(name, new EventInfo( action ));
    }

    /// <summary>
    /// Trigger Event
    /// </summary>
    /// <param name="name">name of trigger</param>
    public void EventTrigger<T>(string name, T info)
    {
        // Find Target Event
        if(event_dic.ContainsKey(name))
        {
            if((event_dic[name] as EventInfo<T>).actions != null)
                (event_dic[name] as EventInfo<T>).actions.Invoke(info);
        }
    }

    /// <summary>
    /// Trigger no-parameter Event
    /// </summary>
    /// <param name="name">name of trigger</param>
    public void EventTrigger(string name)
    {
        // Find Target Event
        if(event_dic.ContainsKey(name))
        {
            if((event_dic[name] as EventInfo).actions != null)
                (event_dic[name] as EventInfo).actions.Invoke();
        }
    }

    /// <summary>
    /// Remove Event
    /// </summary>
    /// <param name="name"> name of trigger </param>
    /// <param name="action"> functions </param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo<T>).actions-= action;
    }

    /// <summary>
    /// Remove no-parameter Event
    /// </summary>
    /// <param name="name"> name of trigger </param>
    /// <param name="action"> functions </param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if(event_dic.ContainsKey(name))
            (event_dic[name] as EventInfo).actions-= action;
    }

    /// <summary>
    /// Remove all Events inside this key
    /// </summary>
    /// <param name="name">name of trigger</param>
    public void RemoveEventKey(string name)
    {
        if(event_dic.ContainsKey(name))
            event_dic.Remove(name);
    }

    /// <summary>
    /// Check if event dictionary contains target event
    /// </summary>
    /// <returns>true if contains</returns>
    public bool ContainsEvent(string name)
    {
        return event_dic.ContainsKey(name);
    }

    /// <summary>
    /// Clear All Events
    /// </summary>
    public void Clear()
    {
        event_dic.Clear();
    }

    /// <summary>
    /// Interface for encapsulation
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
}

