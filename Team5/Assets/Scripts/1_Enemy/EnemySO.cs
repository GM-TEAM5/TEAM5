using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

/// <summary>
/// 공격 타입
/// </summary>
public enum EnemyAttackType
{
    None,
    Melee,
    Range,
}

/// <summary>
/// 적 등급
/// </summary>
public enum EnemyRank
{
    Normal,
    Elite
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "SO/Enemy/EnemyData", order = int.MaxValue)] 
public class EnemySO : ScriptableObject
{
    public string id;           // 시스템 구분용 식별번호
    public string entityName;    // 개체 이름
    public Sprite sprite;       // 개체의 스프라이트
    public float size;       // 기준보다 몇배가 큰지.
    //
    public EnemyRank rank;
    public EnemyAttackType attackType;


    public float maxHp = 200;
    public float armor;

    public float range  =1;         // 공격 사거리 
    public float attackSpeed;       // 평타 공격속도
    public float ad = 20;            // 평타 뎀지 
    public float ap;            // Ability power - 몬스터 특수 기술의 공격력!?
    
    [Min(0.1f)]public float moveCooltime = 0.2f;      // 연산량을 줄이고 좀 더 자연스러운 움직임을 위해 이동에 쿨타임을 적용한다. 
    public float movementSpeed = 5;    // 이동속도

    public float exp = 10;   // 주는 경험치
    
    public List<EnemySkillSO> skils;  // 스킬이 있을 수도 있음.
}
