using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音效控制模块
/// </summary>
public class SoundController : BaseController<SoundController>
{
    private GameObject music_obj;
    
    private AudioSource bgm_player = null;
    private float bgm_volume = 1;
    
    private GameObject sound_obj;
    private List<AudioSource> sound_list;
    private List<AudioSource> inactive_list;
    private float sound_volume = 1;

    public SoundController()
    {
        MonoController.GetController().AddUpdateListener(Update);
    }

    private void Update()
    {
        for(int i = 0; i < sound_list.Count; i ++)
        {
            if(!sound_list[i].isPlaying)
                StopSound(sound_list[i]);
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">音乐名称</param>
    public void PlayBGM(string name)
    {
        // 创建bgm挂载物体
        if(bgm_player == null)
        {
            music_obj = new GameObject("Music Player");
            GameObject obj = new GameObject("BGM");
            obj.transform.SetParent(music_obj.transform);
            bgm_player = obj.AddComponent<AudioSource>();
        }

        // 异步加载bgm， 加载完后播放
        ResourceController.GetController().LoadAsync<AudioClip>("Music/BGM/" + name, (m) =>
        {
            bgm_player.clip = m;
            bgm_player.loop = true;
            bgm_player.volume = bgm_volume;
            bgm_player.Play();
        });
    }

    /// <summary>
    /// 改变背景音乐音量
    /// </summary>
    /// <param name="v">改变至数值音量</param>
    public void ChangeBGMVolume(float v)
    {
        bgm_volume = v;
        if(bgm_player != null)
            bgm_player.volume = bgm_volume;
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBGM()
    {
        if(bgm_player != null)
            bgm_player.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM()
    {
        if(bgm_player != null)
            bgm_player.Stop();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名称</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="callback">调用方法</param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        // 创建音效挂载物体
        if(sound_obj == null)
        {
            sound_obj = new GameObject("Sound");
        }

        // 拿取一个未激活的音效组件或创建一个新的
        AudioSource source;
        if(inactive_list.Count > 0)
            source = inactive_list[0]; 
        else
            source = sound_obj.AddComponent<AudioSource>();

        // 异步加载音效并添加至音效列表
        ResourceController.GetController().LoadAsync<AudioClip>("Music/Sound/" + name, (m) =>
        {   
            source.enabled = true;
            inactive_list.Remove(source);
            source.clip = m;
            source.volume = sound_volume;
            source.loop = isLoop;
            source.Play();
            sound_list.Add(source);
            if(callback != null)
                callback(source);
        });
    }

    /// <summary>
    /// 改变音效音量
    /// </summary>
    /// <param name="v">改变至数值</param>
    public void ChangeSoundVolume(float v)
    {
        sound_volume = v;
        for(int i = 0; i < sound_list.Count; i ++)
            sound_list[i].volume = sound_volume;
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="sound">音效挂载物体</param>
    public void StopSound(AudioSource source)
    {
        if( sound_list.Contains(source))
        {
            sound_list.Remove(source);
            inactive_list.Add(source);
            source.Stop();
            source.enabled = false;
        }
    }
}
