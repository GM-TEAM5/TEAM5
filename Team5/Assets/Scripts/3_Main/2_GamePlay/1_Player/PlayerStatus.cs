using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    //                          0   1    2   3   4   5   6   7   8   9
    // int[] expIncrementTable = {100,0, 10, 30, 60, 60, 60, 60, 60, 60};

    // int[] lowLevelMaxExpTable = {0, 100, 110,140, 200};

    // public int level = 1;
    // public float currExp;
    // public float maxExp = 100;
    #region 기본값
    [SerializeField] float d_attackRange;           //공격 사거리
    [SerializeField] float d_attackArea;            // 공격 범위
    [SerializeField] float d_drawRange = 10;         // 그리기 영역 범위
    [SerializeField] float d_movementSpeed = 10;    // 이동속도
    [SerializeField] float d_maxHp = 500;           // 최대체력
    [SerializeField] float d_maxInk = 100;          // 최대잉크
    [SerializeField] float d_damage = 30;           // 기본 데미지
    [SerializeField] float d_attackCooltime = 1;      // 

    [SerializeField] float d_pickupRange = 4;      // 아이템 획득 범위

    #endregion
    //
    #region 수정값

    [SerializeField] float _currHp;
    public float currHp  // 현재체력
    {
        get => _currHp;
        set
        {
            _currHp = Math.Clamp(value, 0, maxHp);
        }
    }

    public float Inc_maxHp;
    [SerializeField] float d_inkChargeRate = 10f;
    [SerializeField] int inkSegments = 5;
    private float _currInk;
    public float Inc_maxInk;

    public float inkChargeRate => d_inkChargeRate;
    public int totalInkSegments => inkSegments;

    public float currInk
    {
        get => _currInk;
        set
        {
            _currInk = Math.Clamp(value, 0, GetMaxInk());
        }
    }

    private float GetMaxInk() => d_maxInk + Inc_maxInk;
    public float maxInk => GetMaxInk();

    public float armor;
    public float evasionRate;
    public float critRate;
    public float critDamageMultiplier = 1.5f;
    public float movementSpeedMultiplier = 1f;
    public float rangeModifier = 1;
    public float cooltimeModifier = 1;
    public float luck;
    public int rerollCount;
    public int selectionCount;


    public float pDmg;  // physical dmg;
    public float mDmg;  // magical dmg;

    #endregion




    #region 최종값

    public float maxHp => d_maxHp + Inc_maxHp;
    public float movementSpeed => d_movementSpeed * movementSpeedMultiplier;
    public float drawRange => d_drawRange * rangeModifier;
    public float attackRange => d_attackRange * rangeModifier;
    public float attackArea => d_attackArea * rangeModifier;
    public float attackCooltime => d_attackCooltime * cooltimeModifier;
    // public float basicAttackDamage => d_damage + pDmg;

    public float pickUpRange => d_pickupRange * rangeModifier;


    #endregion


    //     
    // 붓칠 관련 변수

    // public float inkUseRate = 20f;    // 초당 잉크 사용량
    // public float inkChargeRate = 20f; // 초당 잉크 충전량



    public PlayerStatus()
    {
        currHp = maxHp;
        currInk = GetMaxInk() * 0.3f;
        rerollCount = 3;
        selectionCount = 3;
    }

    public PlayerStatus(PlayerStatus savedStatus)
    {
        currHp = savedStatus.currHp;
        currInk = savedStatus.currInk;
        rerollCount = savedStatus.rerollCount;
        selectionCount = savedStatus.selectionCount;
    }


    #region 데이터 수정 

    //====================================
    /// <summary>
    /// 해당 exp 만큼 경험치를 획득하고, 레벨업시 레벨과 경험치 요구량을 변경한다.
    /// </summary>
    /// <param name="exp"></param> - 
    /// <returns></returns>     
    // public bool GetExp(float exp)
    // {
    //     //
    //     bool isLevelUp = false;

    //     currExp+=exp;

    //     while(currExp>=maxExp)
    //     {
    //         level++;
    //         isLevelUp = true;

    //         currExp -= maxExp;

    //         SetNextMaxExp(level);
    //     }

    //     return isLevelUp;
    // }


    // void SetNextMaxExp(int level)
    // {
    //     maxExp = maxExp + expIncrementTable[level % expIncrementTable.Length];
    // }

    //=============================================================
    public void OnInit(ref float field, float amount, bool isIncreasing)
    {
        ChangeStatus(ref field, amount, isIncreasing);
    }

    public void OnEquip(ref float field, float amount, bool isIncreasing)
    {
        ChangeStatus(ref field, amount, isIncreasing);

        //
        Player.Instance.OnUpdateStatus();
    }


    public void ChangeStatus(ref float field, float amount, bool isIncreasing)
    {
        if (isIncreasing)
        {
            field += amount;
        }
        else
        {
            field -= amount;
        }
    }

    public void ChangeRerollCount(int amount)
    {
        rerollCount += amount;
    }

    #endregion

    public (int fullSegments, float partialSegment) GetInkSegmentInfo()
    {
        float segmentValue = maxInk / totalInkSegments;
        int maxCount = (int)(currInk / segmentValue);
        float extraInk = currInk - (maxCount * segmentValue);
        float partialFill = extraInk > 0 ? extraInk / segmentValue : 0f;

        return (maxCount, partialFill);
    }
}
