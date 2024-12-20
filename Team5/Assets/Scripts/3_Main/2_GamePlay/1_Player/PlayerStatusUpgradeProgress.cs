using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using JetBrains.Annotations;

[Serializable]
public class PlayerStatusUpgradeProgress
{
    public SerializableDictionary<string, PlayerStatusUpgradeProgressData> list;


    public PlayerStatusUpgradeProgress(PlayerStatus playerStatus)
    {
        Dictionary<string, string> fieldNames = new() { {nameof(playerStatus.Inc_maxHp),"생명력"},
                                            {nameof(playerStatus.inkSegments), "먹물량"},
                                            {nameof(playerStatus.pDmg)          ,"공격력"},
                                            {nameof(playerStatus.mDmg)          ,"도력"},
                                            {nameof(playerStatus.movementSpeedMultiplier) ,"이동속도"} };

        Dictionary<string, float> fieldValues = new() { {nameof(playerStatus.Inc_maxHp), 200},
                                                        {nameof(playerStatus.inkSegments), 4},
                                                        {nameof(playerStatus.pDmg)          , 5},
                                                        {nameof(playerStatus.mDmg)          , 5},   //
                                                        {nameof(playerStatus.movementSpeedMultiplier) ,0.2f} }; // 20 %p

        Dictionary<string, string> effectStrings = new() { {nameof(playerStatus.Inc_maxHp), "증가"},
                                                            {nameof(playerStatus.inkSegments), "개 증가"},
                                                            {nameof(playerStatus.pDmg)          , "증가"},
                                                            {nameof(playerStatus.mDmg)          , "증가"},   //
                                                            {nameof(playerStatus.movementSpeedMultiplier) ,$"증가"} }; // 20 %p

        Dictionary<string, int> initCosts = new() { {nameof(playerStatus.Inc_maxHp), 1},
                                                        {nameof(playerStatus.inkSegments), 1},
                                                        {nameof(playerStatus.pDmg)          , 1},
                                                        {nameof(playerStatus.mDmg)          , 1},   //
                                                        {nameof(playerStatus.movementSpeedMultiplier) ,1} }; // 20 %p

        Dictionary<string, int> costIncrements = new() { {nameof(playerStatus.Inc_maxHp), 1},
                                                {nameof(playerStatus.inkSegments), 1},
                                                {nameof(playerStatus.pDmg)          , 1},
                                                {nameof(playerStatus.mDmg)          , 1},   //
                                                {nameof(playerStatus.movementSpeedMultiplier) ,1} }; // 20 %p\

        Dictionary<string, Action> upgradeCallbackEvents = new() { {nameof(playerStatus.Inc_maxHp), ()=>{
                                                                                                    GameEventManager.Instance.onChangePlayerStatus_maxHp.Invoke();
                                                                                                    Player.Instance.GetHealed(fieldValues[nameof(playerStatus.Inc_maxHp)]);
                                                                                                    }
                                                                                                },
                                                            {nameof(playerStatus.inkSegments), ()=> {
                                                                GameEventManager.Instance.onChangePlayerStatus_inkSegments.Invoke();
                                                            }},
                                                            {nameof(playerStatus.pDmg)         ,null},
                                                            {nameof(playerStatus.mDmg)         ,null},   //
                                                            {nameof(playerStatus.movementSpeedMultiplier) ,null} }; // 20 %p



        list = new();
        foreach (var kv in fieldNames)
        {
            string fieldName = kv.Key;
            string displayedName = kv.Value;

            list[fieldName] = new PlayerStatusUpgradeProgressData(playerStatus,
                                                                fieldName,
                                                                displayedName,
                                                                fieldValues[fieldName],
                                                                effectStrings[fieldName],
                                                                initCosts[fieldName],
                                                                costIncrements[fieldName],
                                                                upgradeCallbackEvents[fieldName]);
        }

    }
}

[Serializable]
public class PlayerStatusUpgradeProgressData
{
    public int level;
    public int initCost;
    public int costIncrement;
    public int cost => initCost + costIncrement * level;
    public float valuePerLevel;     // 레벨당 증가 수치 
    public int maxLevel = 5; // 최대 업그레이드 값 

    public bool canUpgrade => level < maxLevel && playerStatus.statusUpgradePoint >= cost;


    private PlayerStatus playerStatus; // 참조할 객체 (PlayerStatus 인스턴스)
    private FieldInfo targetField; // 참조할 필드 정보

    public string fieldName;// 필드 이름
    public string displayedName;

    public string effectInfo => $"단계x{valuePerLevel}{effectText}";
    public string effectText;

    public Action upgradeCallback;

    //====================================================================

    public PlayerStatusUpgradeProgressData(PlayerStatus playerStatus,
                                            string fieldName,
                                            string displayedName,
                                            float valuePerLevel,
                                            string effectText,
                                            int initCost, int costIncrement,
                                            Action upgradeCallback)
    {
        this.playerStatus = playerStatus;

        // 필드 정보 가져오기
        Type targetType = playerStatus.GetType();
        targetField = targetType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

        if (targetField == null || targetField.FieldType != typeof(float))
        {

        }

        this.fieldName = fieldName;
        this.displayedName = displayedName;
        this.valuePerLevel = valuePerLevel;
        this.effectText = effectText;
        this.initCost = initCost;
        this.costIncrement = costIncrement;
        this.upgradeCallback = upgradeCallback;

    }

    //==============================================================================

    public void Upgrade()
    {
        if (level >= maxLevel)
        {
            Debug.Log("이미 최대 레벨입니다.");
            return;
        }
        playerStatus.UseStatusUpgradePoint(cost);

        level++;

        // 필드 타입에 따라 다르게 처리
        if (targetField.FieldType == typeof(float))
        {
            float currValue = (float)targetField.GetValue(playerStatus);
            targetField.SetValue(playerStatus, currValue + valuePerLevel);
        }
        else if (targetField.FieldType == typeof(int))
        {
            int currValue = (int)targetField.GetValue(playerStatus);
            targetField.SetValue(playerStatus, currValue + (int)valuePerLevel);
        }

        if (upgradeCallback != null)
        {
            upgradeCallback();
        }
    }

    public void Upgrade(int amount)
    {
        int maxAmount = maxLevel - level;
        amount = Math.Min(amount, maxAmount);
        playerStatus.UseStatusUpgradePoint(cost);

        level += amount;

        // 필드 타입에 따라 다르게 처리
        if (targetField.FieldType == typeof(float))
        {
            float currValue = (float)targetField.GetValue(playerStatus);
            targetField.SetValue(playerStatus, currValue + valuePerLevel * amount);
        }
        else if (targetField.FieldType == typeof(int))
        {
            int currValue = (int)targetField.GetValue(playerStatus);
            targetField.SetValue(playerStatus, currValue + (int)(valuePerLevel * amount));
        }
    }

    //=======================================================================

    void IncreaseCost(int amount)
    {
        // cost += amount;
    }
}