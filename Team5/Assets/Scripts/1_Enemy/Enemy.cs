using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

using DG.Tweening;



[RequireComponent(typeof(NavMeshAgent),
                    typeof(SpriteEntity))]
public class Enemy : MonoBehaviour, IPoolObject
{
    public EnemySO enemyData;   //적의 데이터
    EnemyStateUI stateUI;
    SpriteEntity spriteEntity;
    public NavMeshAgent navAgent;
    EnemyMove move;
    Collider enemyCollider;

    Transform t;
    public  Transform t_target;


    [SerializeField] float _hp;
    public float hp  // 현재체력
    {
        get => _hp;
        set 
        {
            _hp = Math.Clamp(value,0,enemyData.maxHp);
        }        
    }  

    public bool isAlive => _hp>0;    
    [SerializeField] float  rangeWeight = 1f ;     // 원거리의 경우 각 개체마다 사거리 보정이 있다. - 자연스러움을 위해
    public float range => enemyData.range * rangeWeight;

    public float targetDistSqr;


    //
    List<EnemySkill> skills = new();    // 공격을 스킬로 대신함. 

    //===============================================================

    void Update()
    {
        if (isAlive == false)
        {
            return;
        }
        
        targetDistSqr = Vector3.SqrMagnitude(t_target.position - transform.position);
        // Debug.Log($"{targetDistSqr} {range}, {range *range} ");
        TryUseSkills();
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = other.ClosestPoint(transform.position);
        //
        if (other.CompareTag("Projectile"))
        {
            GetDamaged(hitPoint, 10);
        }
        //
        else if (other.CompareTag("Brush"))
        {
            GetDamaged(hitPoint,100);
        }
    }

    //===========================
     
    /// <summary>
    ///  이게 init 보다 먼저 호출됨.
    /// </summary>
    public void OnCreatedInPool()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stateUI = GetComponent<EnemyStateUI>();
        spriteEntity = GetComponent<SpriteEntity>();
        enemyCollider = GetComponent<Collider>();
        move = GetComponent<EnemyMove>();

        t = transform;
        
    }

    public void OnGettingFromPool()
    {
        
    }


   //=====================================
    /// <summary>
    /// 척 스텟 초기화 - pool에서 생성되거나, 재탕될 때 호출됨. 
    /// </summary>
    /// <param name="enemyData"></param>
    public void Init(EnemySO enemyData, Vector3 initPos)
    {
        //
        transform.position = initPos;
        enemyCollider.enabled = true;
        navAgent.isStopped = false;
        
        
        //
        this.enemyData = enemyData;
        hp = enemyData.maxHp;
        if (enemyData.attackType == EnemyAttackType.Range)
        {
            rangeWeight = UnityEngine.Random.Range(0.8f,1.2f);
        }
        navAgent.speed = enemyData.movementSpeed;
        // data 에 따라 radius 및 이동속도 도 세팅해야함. 


        InitSkill();
        
        //
        stateUI.Init(this);
        spriteEntity.Init(enemyData.sprite, navAgent.radius, navAgent.height);

        //
        t_target = Player.Instance.transform;
        move.StartMoveRoutine(this);    
    }

    public void GetDamaged(Vector3 hitPoint, float damage)
    {
        hp -= damage;

        if (hp <=0)
        {
            Die();
            
        }

        // ui
        stateUI.UpdateCurrHp(hp);


        // 데미지 텍스트 생성
        PoolManager.Instance.GetDamageText(hitPoint, damage); 
    }

    public void GetHealed(float heal)
    {
        hp += heal;

        stateUI.UpdateCurrHp(hp);
    }

    void Die()
    {
        enemyCollider.enabled = false;       // 적 탐색 및 총알 충돌에 걸리지 않도록.
        navAgent.isStopped = true;          // 이동중지

        DropItem();
        //
        PlaySequence_Death();   //

        stateUI.OnDie();
        //
        TestManager.Instance.TestSFX_enemyDeath();
    }

    void DropItem()
    {
        PoolManager.Instance.GetExp( enemyData.exp, transform.position);

        if ( UnityEngine.Random.Range(0,100) < 50  )
        {
            PoolManager.Instance.GetMoney( enemyData.exp, transform.position);
        }
            
    }

    //=============================================================
    #region SKill
    public void InitSkill()
    {
        skills.Clear();
        foreach(var skillData in enemyData.skils)
        {
            EnemySkill skill = new(skillData);
            skills.Add(skill);
        }
    }


    public void TryUseSkills()
    {  
        for (int i = 0; i < skills.Count; i++)
        {
            if (  CanUse( skills[i] ))
            {
                skills[i].Use(this,t_target.position);
            }
        }
    }

    bool CanUse(EnemySkill skill)
    {
        bool targetInRange = targetDistSqr <= range * range *1.1f;
        
        return targetInRange && skill.isCooltimeOk ;
    }

    #endregion

    //==================================================
    /// <summary>
    /// 적 사망 애니메이션을 재생하고, 해당 애니메이션이 종료후 오브젝트를 제거한다. 
    /// </summary>
    void PlaySequence_Death()
    {
        DOTween.Sequence()
        .OnComplete( ()=> PoolManager.Instance.TakeToPool<Enemy>(this) )
        .Append(spriteEntity.spriteRenderer.DOFade(0,1f))
        .Play();
    }

}
