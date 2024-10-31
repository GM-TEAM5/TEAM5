using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

using DG.Tweening;
using BW.Util;
using Unity.VisualScripting;



[RequireComponent(typeof(NavMeshAgent),
                    typeof(SpriteEntity))]
public class Enemy : MonoBehaviour, IPoolObject
{
    public EnemyDataSO enemyData;   //적의 데이터
    EnemyStateUI stateUI;
    SpriteEntity spriteEntity;
    public NavMeshAgent navAgent;
    EnemyMove move;
    Collider enemyCollider;
    Rigidbody rb;

    public Transform t;
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
    //
    [SerializeField] float stopDurationRemain;
    public bool stopped => stopDurationRemain >0;
    //
    public float stunDurationRemain; 
    public bool stunned => stunDurationRemain >0; 
    
    

    [SerializeField] float  rangeWeight = 1f ;     // 원거리의 경우 각 개체마다 사거리 보정이 있다. - 자연스러움을 위해
    public float range => enemyData.range * rangeWeight;

    public float targetDistSqr;


    //
    List<EnemySkill> skills = new();    // 공격을 스킬로 대신함. 
    EnemySkill usingSkill;

    //
    float lastMoveTime;

    bool moveCooltimeOk => Time.time >= lastMoveTime + enemyData.moveCooltime;
    // bool canMove => stopDurationRemain<=0  && stunDurationRemain <=0;  

    public Vector3 lastHitPoint;

    //===============================================================

    void Update()
    {
        if (isAlive==false || GamePlayManager.isGamePlaying == false )
        {
            return;
        }
        
        targetDistSqr = Vector3.SqrMagnitude(t_target.position - transform.position);
        // Debug.Log($"{targetDistSqr} {range}, {range *range} ");
        
        // 정지 지속시간 감소
        if( stopDurationRemain>0)
        {
            stopDurationRemain -= Time.deltaTime;
        }
        // 스턴 지속시간 감소
        if( stunDurationRemain>0)
        {
            stunDurationRemain -= Time.deltaTime;
        }

        if(stunned)
        {
            return;
        }

        // 스턴걸리면 아래까지 안내려가게.
        TryUseSkills();
        Move(); 
        
    }

    void OnTriggerEnter(Collider other)
    {
        lastHitPoint = other.ClosestPoint(transform.position);

        // TODO 삭제 예정
        if (other.CompareTag("Projectile"))
        {
            GetDamaged(10);
        }
        //
        else if (other.CompareTag("Brush"))
        {
            GetDamaged(100);
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
        rb = GetComponent<Rigidbody>();
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
    public void Init(EnemyDataSO enemyData, Vector3 initPos)
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

        stunDurationRemain = 0;
        stopDurationRemain = 0;



        InitSkill();
        
        //
        stateUI.Init(this);
        spriteEntity.Init(enemyData.sprite, navAgent.radius, navAgent.height);

        //
        t_target = Player.Instance.transform;
        move.Init(this);   


        lastMoveTime = -999;
        rb.velocity = Vector3.zero;
    }

    //===========================================================================================

    void Move()
    {
        // Debug.Log($" move {canMove} : {lastMoveTime} +  {enemyData.moveCooltime}  <> {Time.time}");
        if (moveCooltimeOk && stopDurationRemain<=0)
        {
            navAgent.isStopped  = false;
            navAgent.velocity = navAgent.desiredVelocity;   //  원래는 velocity 가 점진적으로 가속을 받기 때문에, 즉시 원하는 이동속도 적용

            //
            move.Move(enemyData,navAgent,t_target.position );

            lastMoveTime = Time.time;


            UpdateSpriteDir(t_target.position);
        } 
    }


    
    public void GetDamaged(Vector3 hitPoint,float damage,bool isEnhancedAttack = false)
    {
        lastHitPoint = hitPoint == Vector3.zero? enemyCollider.ClosestPoint(t_target.position) :hitPoint;      // 플레이어와 적 개체의 콜라이더가 겹쳐있는 경우, hitPoint 가 (0,0,0)이 나옴;
        
        GetDamaged(damage, isEnhancedAttack );
    }

    public void GetDamaged(float damage, bool isEnhancedAttack = false)
    {
        float nockbackPower= 5;
        if (isEnhancedAttack)
        {
            DropInk();
            nockbackPower = 10;
        }
        GetKnockback(nockbackPower, lastHitPoint);
        //
        hp -= damage;
        if (hp <=0)
        {
            Die();
            
        }

        // ui
        stateUI.UpdateCurrHp(hp);

        // 데미지 텍스트 생성
        DamageType damageType = isEnhancedAttack ? DamageType.DMG_CRITICAL : DamageType.DMG_NORMAL;
        PoolManager.Instance.GetDamageText(lastHitPoint, damage, damageType); 
    }

    public void GetHealed(float heal)
    {
        hp += heal;

        stateUI.UpdateCurrHp(hp);
    }

    //=========================================================================================
    
    /// <summary>
    ///  정지 상태 적용 - 움직이지 못하게. - 스킬 사용, 피격 or 사망  등
    /// </summary>
    /// <param name="duration"></param>
    public void SetStopped( float duration )
    {
        stopDurationRemain = Math.Max(stopDurationRemain,duration );
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
    }

    /// <summary>
    /// 기절 상태 적용 - 넉백시. or 기타 군중제어 
    /// </summary>
    /// <param name="duration"></param>
    void SetStunned( float duration )
    {
        stunDurationRemain = Math.Max(stunDurationRemain, duration);
        SetStopped(duration);    // 
    }
    
    
    
    // knockBack 
    public void GetKnockback(float power, Vector3 hitPoint)
    {
        if(usingSkill!=null)
        {
            usingSkill.Interrupt(this);
            usingSkill=null;
        }

        SetStunned(0.5f);

        Vector3 dir = (t.position - hitPoint).WithFloorHeight().normalized;
        rb.velocity = dir * power;
        
        DOTween.Sequence()
        .AppendInterval(0.2f)
        .AppendCallback( ()=>rb.velocity = Vector3.zero)
        .Play();
    }


    void Die()  
    {
        enemyCollider.enabled = false;       // 적 탐색 및 총알 충돌에 걸리지 않도록.
        navAgent.isStopped = true;          // 이동중지
        
        enemyData.OnDie(this);
        DropItem();
        //
        PlaySequence_Death();   //
        
        stateUI.OnDie();
        //
        TestManager.Instance.TestSFX_enemyDeath(enemyData.type);
    }

    void DropItem()
    {
        PoolManager.Instance.GetExp( enemyData.exp, transform.position);

        if ( UnityEngine.Random.Range(0,100) < 50  )
        {
            PoolManager.Instance.GetMoney( enemyData.exp, transform.position);
        }
            
    }


    /// <summary>
    /// 강공격에 맞으면 잉크 떨구도록
    /// </summary>
    public void DropInk()
    {
        PoolManager.Instance.GetInk( 5, transform.position);
    }


    /// <summary>
    /// targetPos 방향으로 스프라이트 방향을 세팅한다. 
    /// </summary>
    /// <param name="targetPos"></param>
    void UpdateSpriteDir(Vector3 targetPos)
    {
        Vector3 dir = targetPos - t.position;
        spriteEntity.Flip(dir.x);
    }






    //=============================================================
    #region SKill
    public void InitSkill()
    {
        usingSkill = null;
        
        skills.Clear();
        foreach(var skillData in enemyData.skils)
        {
            EnemySkill skill = new(skillData);
            skills.Add(skill);
        }
    }


    public void TryUseSkills()
    {  
        if( usingSkill!=null)
        {
            return;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            EnemySkill skill = skills[i];
            
            if (  CanUse( skill ) )
            {
                usingSkill = skill;

                skill.Use(this,t_target.position);
                SetStopped(skill.skillData.delay_beforeCast + skill.skillData.delay_afterCast);

            }
        }
    }

    public void OnFinish_Skill()
    {
        usingSkill = null;
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
        .AppendInterval(0.3f)
        .Append(spriteEntity.spriteRenderer.DOFade(0,1f))
        .Play();
    }

}
