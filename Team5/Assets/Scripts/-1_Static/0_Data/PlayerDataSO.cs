using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor.Rendering;
using Unity.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Audio;


/// <summary>
/// 사운드 세팅.
/// </summary>
[Serializable]
public class SoundSetting
{
    public float bgmVolume;
    public float sfxVolume;

    public SoundSetting()
    {
        bgmVolume = 1;
        sfxVolume = 1;
    }
}


[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = int.MaxValue)]
public class PlayerDataSO : ScriptableObject
{
    public InputActionAsset inputActionSO;
    // public SoundSetting soundSetting;
    public AudioMixer audioMixer;
    
    public int traitPoint;
    public int currChapter = 0;
    public int currStageNum = 0;
    public int currStagePlayCount;
    public int deathCount;


    // public bool isNewUser => deathCount==0 && currChapter ==1 && currStageNum ==1;

    [Min(0)] public int maxCount_equipment = 8;
    // [Min(0)] public int maxCount_skill = 4;

    //
    public List<StageDataSO> stages = new();

    public bool isGameclear => currStageNum >= stages.Count;



    //
    public List<EquipmentItemSO> equipments;
    // public List<SkillItemSO> skills;

    // public List<SkillItemSO> initSkills;
    public PlayerStatus savedStatus;

    [Header("Ability")]
    public List<SkillItemSO> ability_passives = new();
    public SerializableDictionary<SkillType, SkillItemSO> ability_actives = new();
    // public SkillItemSO ability_basicAttack;   // 당분간 미사용
    // public SkillItemSO ability_draw;
    // public SkillItemSO ability_util;
    // public SkillItemSO ability_scroll;

    [Header("InitData")]
    public SerializableDictionary<SkillType, SkillItemSO> init_actives = new();
    // public SkillItemSO init_basicAttack;   // 당분간 미사용
    // public SkillItemSO init_draw;
    // public SkillItemSO init_util;
    // public SkillItemSO init_scroll;



    //==================================================================================

    void OnValidate()
    {
        FixContainerSize();
        FixActiveAbility();
    }

    //==================================================================================
    /// <summary>
    /// 액티브 기술이 
    /// </summary>
    void FixActiveAbility()
    {
        // 현재 데이터
        if (ability_actives.ContainsKey(SkillType.BasicAttack)==false)
        {
            ability_actives[SkillType.BasicAttack] = null;
        }
        if (ability_actives.ContainsKey(SkillType.Draw)==false)
        {
            ability_actives[SkillType.Draw] = null;
        }
        if (ability_actives.ContainsKey(SkillType.Util)==false)
        {
            ability_actives[SkillType.Util] = null;
        }
        if (ability_actives.ContainsKey(SkillType.Scroll)==false)
        {
            ability_actives[SkillType.Scroll] = null;
        }

        // 초기 데이터 
        if (init_actives.ContainsKey(SkillType.BasicAttack)==false)
        {
            init_actives[SkillType.BasicAttack] = null;
        }
        if (init_actives.ContainsKey(SkillType.Draw)==false)
        {
            init_actives[SkillType.Draw] = null;
        }
        if (init_actives.ContainsKey(SkillType.Util)==false)
        {
            init_actives[SkillType.Util] = null;
        }
        if (init_actives.ContainsKey(SkillType.Scroll)==false)
        {
            init_actives[SkillType.Scroll] = null;
        }
    }



    /// <summary>
    /// 리스트의 최대 크기 지정 - 장비와 스킬
    /// </summary>
    void FixContainerSize()
    {
        // 장비
        equipments = equipments
        .Take(maxCount_equipment)
        .Concat(Enumerable.Repeat((EquipmentItemSO)null, Mathf.Max(0, maxCount_equipment - equipments.Count)))  // 부족한 부분을 0으로 채움
        .ToList();

        // 스킬

        // skills = skills
        // .Take(maxCount_skill)
        // .Concat(Enumerable.Repeat((SkillItemSO)null, Mathf.Max(0, maxCount_skill - skills.Count)))  // 부족한 부분을 0으로 채움
        // .ToList();
    }


    //==================================================================================
    public void InitPlayerData()
    {
        // 패시브 초기화.
        ability_passives.Clear();

        // 액티브 초기화.
        foreach(var kv in ability_actives)
        {
            SkillType key = kv.Key;
            ability_actives[key] = init_actives[key];
        }


        //
        equipments.Clear();
        FixContainerSize();

        currStageNum = 0;

        savedStatus = new();
    }

    public void OnStageClear(Player player)
    {
        currStageNum++;

        // 진행중이던 데이터 저장
        equipments = player.equipments.equipments;

        // 패시브 
        ability_passives= new();
        foreach(PlayerSkill ps in  Player.Instance.skills.passives)
        {
            ability_passives.Add( ps.skillData);
        }

        // 액티브
        foreach(var kv in  Player.Instance.skills.actives)
        {

            ability_actives[kv.Key] = kv.Value.skillData;
        }

        //
        savedStatus = player.status;
    }


    public StageDataSO GetCurrStageInfo()
    {
        if (currStageNum < stages.Count)
        {
            return stages[currStageNum];
        }
        currStageNum = 0;

        return stages[currStageNum];
    }
}
