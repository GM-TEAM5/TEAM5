using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SimpleSound", menuName = "SO/Sound/Simple", order = int.MaxValue)]
public class SimpleSoundSO : SoundSO 
{
    public AudioClip file;
    
    public override void Play(Transform t)
    {
        // file재생
        SoundManager.Instance.EnqueueSound(file, type, rank, t.position);
    }
}
