using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public enum SoundType
{
    SFX,
    BGM,
}

//
public enum SoundEventType
{
    EnemyDie,
    PlayerMove,


}

[System.Serializable]
public class SoundEvent
{
    // public SoundEventType eventType;    // 발생시킬 이벤트 
    public SoundType type;     // BGM or SFX
    public AudioClip file;         // 재생할 사운드
    public int rank;            // 재생 우선순위 ( 낮을수록 우선도 높음 )
}




[CreateAssetMenu(fileName = "SoundEventTable", menuName = "SO/SoundEventTable", order = int.MaxValue)]
public class SoundEventTableSO : ScriptableObject
{
    // public List<SoundEvent> list = new();
    public SerializableDictionary<SoundEventType, SoundEvent> table = new();


    void OnValidate()
    {

    }
}

