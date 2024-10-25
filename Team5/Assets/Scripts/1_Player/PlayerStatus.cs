using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    //                          0   1    2   3   4   5   6   7   8   9
    int[] expIncrementTable = {100,0, 10, 30, 60, 60, 60, 60, 60, 60};

    // int[] lowLevelMaxExpTable = {0, 100, 110,140, 200};

    public int level = 1;
    public float currExp;
    public float maxExp = 100;
    
    //
    [SerializeField] float _hp;
    public float hp  // 현재체력
    {
        get => _hp;
        set 
        {
            _hp = Math.Clamp(value,0,maxHp);
        }        
    }       
    public float maxHp = 500;     // 최대체력
    public float ad = 10;     // 공격력
    public float attackSpeed = 0.3f;
    public float armor;     // 방어력
    public float movementSpeed = 10;    // 이동속도
    public float range = 4f;
    // 붓칠 관련 변수
    public float drawRange = 10f; // 그리기 사용 가능 범위  
    public float maxInk = 100f;   // 최대 잉크 양
    [SerializeField]float _currInk;   // 현재 잉크 양
    public float currInk  // 현재체력
    {
        get => _currInk;
        set 
        {
            _currInk = Math.Clamp(value,0,maxInk);
        }        
    }     


    public float inkUseRate = 20f;    // 초당 잉크 사용량
    public float inkChargeRate = 20f; // 초당 잉크 충전량

    public PlayerStatus()
    {
        hp = maxHp;     
        currInk = maxInk *0.3f;
    }

    //====================================
    /// <summary>
    /// 해당 exp 만큼 경험치를 획득하고, 레벨업시 레벨과 경험치 요구량을 변경한다.
    /// </summary>
    /// <param name="exp"></param> - 
    /// <returns></returns>     
    public bool GetExp(float exp)
    {
        //
        bool isLevelUp = false;

        currExp+=exp;

        while(currExp>=maxExp)
        {
            level++;
            isLevelUp = true;
            
            currExp -= maxExp;
            
            SetNextMaxExp(level);
        }

        return isLevelUp;
    }

    
    void SetNextMaxExp(int level)
    {
        maxExp = maxExp + expIncrementTable[level % expIncrementTable.Length];
    }
}
