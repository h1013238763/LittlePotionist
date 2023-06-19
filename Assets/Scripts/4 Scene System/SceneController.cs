using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene Changing module
/// </summary>
public class SceneController : BaseController<SceneController>
{
    /// <summary>
    /// Change Scene
    /// </summary>
    /// <param name="name">name of scene</param>
    /// <param name="action">The action after finish loading</param>
    public void LoadScene(string name, UnityAction action)
    {

        SceneManager.LoadScene(name);

        action.Invoke();
    }

    /// <summary>
    /// Change Scene Async
    /// </summary>
    /// <param name="name">场景名称</param>
    /// <param name="fun">加载函数</param>
    public void LoadSceneAsync(string name, UnityAction action)
    {
        MonoController.GetController().StartCoroutine(ILoadSceneAsync(name, action));
    }

    /// <summary>
    /// The Coroutine function
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator ILoadSceneAsync(string name, UnityAction action)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        while(operation.isDone)
        {
            EventController.GetController().EventTrigger("Refresh Progress", operation.progress);
            yield return operation.progress;
        }

        action();
    }

}
