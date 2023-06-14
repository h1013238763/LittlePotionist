using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tween Controller module
/// </summary>
public class TweenController : BaseControllerMono<TweenController>
{   
    private Dictionary<string, TweenAction>              info_list   = new Dictionary<string, TweenAction>();
    private Dictionary<string, UnityAction<TweenAction>> action_list = new Dictionary<string, UnityAction<TweenAction>>();
    private List<TweenAction> remove_action = new List<TweenAction>();

    private Dictionary<TweenAction, UnityAction<TweenAction>> wait_list = new Dictionary<TweenAction, UnityAction<TweenAction>>();
    private List<TweenAction> remove_wait = new List<TweenAction>();

    private List<TweenAction> tween_pool = new List<TweenAction>();

    void Update()
    {
        Animating();
    }

    /// <summary>
    /// Animating
    /// </summary>
    private void Animating()
    {
        // animating
        foreach(var pair in info_list)
        {
            // assign infomation
            TweenAction current_action = pair.Value; 
            // use infomation for animating
            action_list[current_action.id].Invoke(current_action);
            // check if animation finished and remove animation
            if(current_action.Finish())
                remove_action.Add(current_action);
        }

        // remove finished animation
        foreach(TweenAction action in remove_action)
        {
            action_list.Remove(action.id);
            info_list.Remove(action.id); 
            tween_pool.Add(action); 
        }
        remove_action.Clear();

        
        // add animation into action list
        foreach(var pair in wait_list)
        {
            if(!info_list.ContainsKey(pair.Key.id))
            {
                info_list.Add(pair.Key.id, pair.Key);
                action_list.Add(pair.Key.id, pair.Value);
                remove_wait.Add(pair.Key);
            }
        }

        foreach(TweenAction action in remove_wait)
        {
            wait_list.Remove(action);
        }
        remove_wait.Clear();
    }

    public void MoveToPosition(Transform transform, Vector3 pos, float time, TweenType type = TweenType.Normal)
    {
        // assign infomation
        TweenAction action = GetTweenAction();
        action.id = transform.gameObject.name;
        action.target = transform;
        action.start_pos = transform.position;
        action.end_pos = pos;
        action.current_time = 0;
        action.total_time = time;
        action.type = type;

        // assign to dictionary
        wait_list.Add(action, IMoveToPosition);
    }
    private void IMoveToPosition(TweenAction action)
    {
        float percent = action.current_time/action.total_time;
        if(percent > 1)
            percent = 1;

        switch(action.type)
        {
            case TweenType.Normal:
                action.target.position = Vector3.Lerp(action.start_pos, action.end_pos, percent);
                break;
            case TweenType.Smooth:
                break;
            default:
                break;
        }
    }

    public TweenAction GetTweenAction()
    {  
        if(tween_pool.Count < 1)
            tween_pool.Add(new TweenAction());

        TweenAction temp = tween_pool[tween_pool.Count-1];
        tween_pool.RemoveAt(tween_pool.Count-1);
        return temp;
    }
    
    public class TweenAction
    {
        public string id;
        public Transform target;
        public Vector3 start_pos;
        public Vector3 end_pos;
        public float current_time;
        public float total_time;
        public TweenType type;

        /// <summary>
        /// Constructor
        /// </summary>
        public TweenAction(){}

        /// <summary>
        /// check if this animation is finished
        /// </summary>
        /// <returns>true if finished</returns>
        public bool Finish()
        {
            current_time += Time.deltaTime;
            return current_time >= total_time;
        }

        // the animation type of tween
        
    }
}

public enum TweenType
{
    Normal,
    Smooth
}