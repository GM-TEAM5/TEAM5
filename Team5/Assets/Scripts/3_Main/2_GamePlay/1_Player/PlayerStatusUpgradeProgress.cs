using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[Serializable]
public class PlayerStatusUpgradeProgress
{
    public SerializableDictionary<string, PlayerStatusUpgradeProgressField > list;


    public PlayerStatusUpgradeProgress(PlayerStatus playerStatus)
    {
        List<string> fieldNames = new () { nameof(playerStatus.Inc_maxHp),
                                            nameof(playerStatus.Inc_maxInk),
                                            nameof(playerStatus.pDmg),
                                            nameof(playerStatus.mDmg),
                                            nameof(playerStatus.movementSpeed) };

        list = new();
        foreach(string fieldName in fieldNames)
        {
            // Debug.Log($" 잉  {fieldName}");
            list[fieldName] = new PlayerStatusUpgradeProgressField(playerStatus, fieldName);
        }

    }    
}

[Serializable]
public class PlayerStatusUpgradeProgressField 
{
   public int level;
    public int cost;
    public float valuePerLevel;     // 레벨당 증가 수치 
    public int maxLevel = 5; // 최대 업그레이드 값 

    public bool canUpgrade => level< maxLevel;


    private PlayerStatus playerStatus; // 참조할 객체 (PlayerStatus 인스턴스)
    private FieldInfo targetField; // 참조할 필드 정보

    public string fieldName;// 필드 이름
    
    public PlayerStatusUpgradeProgressField(PlayerStatus playerStatus, string fieldName)
    {
        this.playerStatus = playerStatus;

        // 필드 정보 가져오기
        Type targetType = playerStatus.GetType();
        targetField = targetType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

        if (targetField == null || targetField.FieldType != typeof(float))
        {
            
        }

        this.fieldName = fieldName;
    }

    //==============================================================================

    public void Upgrade()
    {
        if (level>= maxLevel)
        {
            Debug.Log("이미 최대 레벨입니다.");
            return;
        }
        level ++;
        IncreaseCost(1);
        float currValue = (float)targetField.GetValue(playerStatus);
        targetField.SetValue( playerStatus, currValue + valuePerLevel );
    }

    public void Upgrade(int amount)
    {
        int maxAmount = maxLevel - level;
        amount = Math.Min(amount,maxAmount);

        level += amount;
        IncreaseCost(amount);
        float currValue = (float)targetField.GetValue(playerStatus);
        targetField.SetValue( playerStatus, currValue + valuePerLevel * amount );
    }

    //=======================================================================

    void IncreaseCost(int amount)
    {
        cost += amount;
    }
}