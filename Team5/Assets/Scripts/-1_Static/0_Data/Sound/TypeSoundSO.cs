using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class TypeSoundSO<TEnum> : SoundSO where TEnum : System.Enum
{
    public SerializableDictionary<TEnum,AudioClip> files;
    
    
    public override void Play(Transform t)
    {
        TEnum  enumType = GetType(t);

        AudioClip file = files[enumType];

        SoundManager.Instance.EnqueueSound(file, type, rank, t.position);
    }

    // 각 enum 타입에 대한 키를 가져오는 메서드
    protected abstract TEnum GetType(Transform t);
}
