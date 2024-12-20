using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using BW;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SoundData
{
    public AudioClip audioClip; 
    public SoundType type;
    public Vector3 pos;
    public int rank;

    public SoundData( AudioClip audioClip, SoundType type,  int rank = 0 , Vector3 pos = default )
    {
        this. audioClip = audioClip;
        this.type = type;
        this.rank = rank;
        this.pos = pos;
    }

}


//
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] SoundEventTableSO soundEventTable;


    PriorityQueue< SoundData > waitingQ= new();
    

    AudioSource bgmSource;

    [SerializeField] int playingSFXCount = 0;

    //==============================================================

    void Update()
    {
        ClearQueue();
    }

    public void Init()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.dopplerLevel = 0;
        bgmSource.priority = 0;
        
        
        InitPool();
        InitSoundSetting();
    }

    //================================================================
    /// <summary>
    /// 큐에 등록된 소리들을 재생한다. 동시에 최대 maxSFXCount 개까지 재생.  
    /// </summary>
    void ClearQueue()
    {
        int cnt = 0;
        while(waitingQ.IsEmpty()==false)
        {
            // 소리재생
            SoundData soundData = waitingQ.Dequeue();
            PlaySFX( soundData );
            //
            if( ++ cnt >= soundEventTable.maxSFXCount)
            {
                waitingQ.Clear();
            }
        }
    }

    //==============================================================
        
    /// <summary>
    /// 사운드 이벤트 테이블에 맞는 soundSO를 발생시킨다.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="eventType"></param>
    public void Invoke(Transform t, SoundEventType eventType)
    {
        if(soundEventTable.table.TryGetValue(eventType, out var soundSO))
        {
            soundSO.Play(t);
        }
    }



    /// <summary>
    /// soundSO 에서 Play 시키면 호출됨. 
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="type"></param>
    /// <param name="rank"></param>
    /// <param name="pos"></param>
    public void EnqueueSound(  AudioClip audioClip, SoundType type, int rank, Vector3 pos)
    {
        // 브금인 경우,
        if( type == SoundType.BGM )
        {
            PlayBGM(audioClip);
        }
        // sfx인 경우, 
        else 
        {
            SoundData soundData =  new SoundData(audioClip, type, rank, pos);
            if( rank >0)
            {
                 // 큐에 등록 
                waitingQ.Enqueue( soundData , rank);
            }
            else
            {
                // 바로 재생
                PlaySFX(soundData );
            }
        }
    }

    //=======================================================================

    void PlaySFX(SoundData soundData)
    {
        if(isMute_sfx)
        {
            return;
        }
        
        playingSFXCount++;
        
        SFX sfx = GetFromPool<SFX>();
        sfx.Play(soundData);
    }

    void PlayBGM(AudioClip audioClip)
    {
        bgmSource.Stop();
        bgmSource.clip = audioClip;
        bgmSource.Play();
    }


    public void DestroySFX(SFX sfx)
    {
        TakeToPool<SFX>(sfx);
        playingSFXCount--;
    }



//===============================================


    #region 사운드 세팅 
    float minMixerValue = -40;
    float maxMixerValue = 0;
    float diff_mixerValue => maxMixerValue - minMixerValue;

    public float maxSettingValue = 10;
    public float minSettingValue = 0;
    AudioMixer mixer;

    [Range(-80,0)] public float master = 0;
    [Range(-80,0)] public float bgm = 0;
    [Range(-80,0)] public float sfx = 0;
    
    bool isMute_bgm;
    bool isMute_sfx;

    //====================================
    void InitSoundSetting()
    {
        // soundSetting = GameManager.Instance.playerData.soundSetting;
        mixer =  GameManager.Instance.userData.audioMixer;
        mixer.GetFloat(nameof(master), out master);
        mixer.GetFloat(nameof(bgm), out bgm);
        mixer.GetFloat(nameof(sfx), out sfx);
    }


    public void SetMaster(float settingValue)
    {
        float mixerValue = ChangeToMixerValue(settingValue);

        CheckMute();
        
        master = mixerValue;
        mixer.SetFloat(nameof(master), master);
    }

    public void SetBGM(float settingValue)
    {
        CheckMute();
        
        float mixerValue = ChangeToMixerValue(settingValue);

        bgm = mixerValue;
        mixer.SetFloat(nameof(bgm), bgm);
    }

    public void SetSFX(float settingValue)
    {
        CheckMute();
        
        float mixerValue = ChangeToMixerValue(settingValue);

        sfx = mixerValue;
        mixer.SetFloat(nameof(sfx), sfx);
    }

    public float GetSettingValue_Master()
    {
        return ChangeToSettingValue( master);
    }
    public float GetSettingValue_BGM()
    {
        return ChangeToSettingValue( bgm );
    }

    public float GetSettingValue_SFX()
    {
        return ChangeToSettingValue( sfx );
    }

    //=========================================================================

    float ChangeToMixerValue(float settingValue)
    {
        settingValue = System.Math.Clamp(settingValue, minSettingValue, maxSettingValue);

        float ret = minMixerValue  + diff_mixerValue * settingValue / maxSettingValue;
        return ret;        
    }

    float ChangeToSettingValue(float mixerValue)
    {
        float ratio = (mixerValue - minMixerValue) / diff_mixerValue;
        
        float ret = ratio * maxSettingValue;
        return ret;  
    }


    void CheckMute()
    {
        isMute_bgm  = master == minMixerValue || bgm == minMixerValue;
        isMute_sfx  = master == minMixerValue || sfx == minMixerValue;
    }










    #endregion






































#region pool

    [SerializeField] List<PoolData> _pools;

    readonly List<IPool<Component>> _poolsObjects = new();



    void InitPool()
    {
        var namesGroups = _pools.Select(p => p.Name).GroupBy(n => n).Where(g => g.Count() > 1);

        if (namesGroups.Count() > 0)
            throw new Exception($"Pool Manager already contains pool with name \"{namesGroups.First().Select(g => g).First()}\"");

        var poolsType = typeof(List<IPool<Component>>);
        var poolsAddMethod = poolsType.GetMethod("Add");
        var genericPoolType = typeof(Pool<>);

        foreach (var poolData in _pools)
        {
            var poolType = genericPoolType.MakeGenericType(poolData.Component.GetType());
            var createMethod = poolType.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic);

            var pool = createMethod.Invoke(null, new object[] { poolData.Component, poolData.Count, poolData.Container });

            if (poolData.NonLazy)
            {
                var nonLazyMethod = poolType.GetMethod("NonLazy");
                nonLazyMethod.Invoke(pool, null);
            }

            poolsAddMethod.Invoke(_poolsObjects, new object[] { pool });
        }
    }

    #region Get pool
    /// <summary>
    /// Find pool by <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="T">Pool's objects type.</typeparam>
    /// <param name="index">Pool index.</param>
    /// <returns>Finded pool.</returns>
    IPool<T> GetPool<T>(int index) where T : Component => (IPool<T>)_poolsObjects[index];

    /// <summary>
    /// Find pool by type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Pool's objects type.</typeparam>
    /// <returns>Finded pool.</returns>
    IPool<T> GetPool<T>() where T : Component => (IPool<T>)_poolsObjects.Find(p => p.Source is T);

    /// <summary>
    /// Find pool by <paramref name="name"/>
    /// </summary>
    /// <typeparam name="T">Pool's objects type.</typeparam>
    /// <param name="name">Pool name.</param>
    /// <returns>Finded pool.</returns>
    IPool<T> GetPool<T>(string name) where T : Component => (IPool<T>)_poolsObjects[_pools.FindIndex(p => p.Name == name)];
    #endregion

    #region Get from pool
    /// <summary>
    /// Find pool by <paramref name="index"/> and gets object from it.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="GetPool{T}"/></typeparam>
    /// <param name="index"><inheritdoc cref="GetPool{T}"/></param>
    /// <returns>Pool's object (or <see langword="null"/> if free object was not finded).</returns>
    T GetFromPool<T>(int index) where T : Component => GetPool<T>(index).Get();

    /// <summary>
    /// Find pool by type <typeparamref name="T"/> and gets object from it.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="GetPool{T}"/></typeparam>
    /// <returns>Pool's object (or <see langword="null"/> if free object was not finded).</returns>
    T GetFromPool<T>() where T : Component => GetPool<T>().Get();

    /// <summary>
    /// Find pool by name <paramref name="name"/> and gets object from it.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="GetPool{T}"/></typeparam>
    /// <param name="name"><inheritdoc cref="GetPool{T}(string)"/></param>
    /// <returns>Pool's object (or <see langword="null"/> if free object was not finded).</returns>
    T GetFromPool<T>(string name) where T : Component => GetPool<T>(name).Get();
    #endregion

    #region Take to pool
    /// <summary>
    /// Returns object back to pool and marks it as free.
    /// </summary>
    /// <param name="index"><inheritdoc cref="GetPool{T}"/></param>
    /// <param name="component">Object (its component) which returns back.</param>
    void TakeToPool(int index, Component component) => _poolsObjects[index].Take(component);

    /// <summary>
    /// Returns object back to pool and marks it as free.
    /// </summary>
    /// <typeparam name="T">Pool type.</typeparam>
    /// <param name="component">Object (its component) which returns back.</param>
    void TakeToPool<T>(Component component) where T : Component => GetPool<T>().Take(component);

    /// <summary>
    /// Returns object back to pool and marks it as free.
    /// </summary>
    /// <typeparam name="T">Pool type.</typeparam>
    /// <param name="name"><inheritdoc cref="GetPool{T}(string)"/></param>
    /// <param name="component">Object (its component) which returns back.</param>
    void TakeToPool<T>(string name, Component component) where T : Component => GetPool<T>(name).Take(component);
    #endregion



#endregion


}
