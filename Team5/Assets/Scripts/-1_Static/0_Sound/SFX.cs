using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SFX : MonoBehaviour, IPoolObject
{
    AudioSource audioSource;

    static int defaultPriority=100;
    
    //======================================
    public void OnCreatedInPool()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnGettingFromPool()
    {
        
    }

    //======================================
    public void Play(SoundData soundData)
    {
        // 세팅
        transform.position = soundData.pos;
        audioSource.clip = soundData.audioClip;
        audioSource.priority = defaultPriority + soundData.rank;
        if(soundData.type == SoundType.SFX_UI)
        {
            audioSource.dopplerLevel = 0;
            audioSource.spatialBlend = 0;
        }

        //After Setting
        audioSource.Play();
        StartCoroutine( DelayedDestroy( soundData.audioClip.length+ 0.1f)); 
    }

    IEnumerator DelayedDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        SoundManager.Instance.DestroySFX(this);
    }



}
