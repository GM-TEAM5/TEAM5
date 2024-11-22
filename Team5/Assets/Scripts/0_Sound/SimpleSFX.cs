using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleSFX : MonoBehaviour
{
    AudioSource audioSource;

    public void PlaySFX(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();


        Destroy(gameObject, 2f);
    }
}
