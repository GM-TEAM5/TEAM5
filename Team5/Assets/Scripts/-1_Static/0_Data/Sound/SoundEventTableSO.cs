using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.EditorTools;



public enum SoundType
{
    SFX_GO,         // 게임 오브젝트의 sfx - 위치 지정 필요. - go : game object
    SFX_UI,         // ui의 sfx - 위치 지정 필요 없음. 
    BGM,            // bgm
}

//
public enum SoundEventType
{
    EnemyDie,
    EnemyHit,
    PlayerMove,
    PlayerBasicAttack,
    PlayerRyoikiTenkaiOn,
    PlayerRyoikiTenkaiOff,
    UI_GamePlayStart,
    UI_ButtonClick,
}

[CreateAssetMenu(fileName = "SoundEventTable", menuName = "SO/SoundEventTable", order = int.MaxValue)]
public class SoundEventTableSO : ScriptableObject
{
    // public List<SoundEvent> list = new();
    [Tooltip("각 소리가 발생하는 상황과 재생할 소리 파일")]
    public SerializableDictionary<SoundEventType, SoundSO> table = new();
    
    [Tooltip("동시에 재생할 수 있는 sfx 수\nBGM, rank가 0인 소리는 제외\n 아직 미완성 ")]
    public int maxSFXCount = 16;

    void OnValidate()
    {

    }
}

