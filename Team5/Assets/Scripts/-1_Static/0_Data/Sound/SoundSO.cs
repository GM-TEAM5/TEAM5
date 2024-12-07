using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundSO : ScriptableObject
{
    public SoundType type;     // BGM or SFX
    public int rank;            // 재생 우선순위 ( 낮을수록 우선도 높음 )

    
    
    public abstract void Play(Transform t);
}
