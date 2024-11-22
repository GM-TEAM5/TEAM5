using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using DG.Tweening;
using BW.Util;


[RequireComponent(typeof(NavMeshAgent),
    typeof(SpriteEntity))]
[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour, IPoolObject, ITimeScaleable
{
    public EnemyDataSO data; //적의 데이터
    EnemyStateUI stateUI;
    public SpriteEntity spriteEntity;


    public EnemyAI ai;

    // EnemyMove move;
    Collider enemyCollider;
    Rigidbody rb;

    public Transform t;
    public Transform t_target;
    public float targetDistSqr;


    [SerializeField] float _hp;

    public float hp // 현재체력
    {
        get => _hp;
        set { _hp = Math.Clamp(value, 0, data.maxHp); }
    }

    public bool isAlive => _hp > 0;

    //
    [SerializeField] float stopDurationRemain;

    public bool stopped => stopDurationRemain > 0;

    //
    public float stunDurationRemain;
    public bool stunned => stunDurationRemain > 0;


    [SerializeField] float rangeWeight = 1f; // 원거리의 경우 각 개체마다 사거리 보정이 있다. - 자연스러움을 위해
    public float range => data.range * rangeWeight;

    public Vector3 lastHitPoint;

    [Header("Slow Effect")]
    private float currentSlowAmount = 0f;
    private float slowDuration = 0f;
    private float originalSpeed;
    private Coroutine slowRoutine;

    private float slowAmount = 0f;
    private float slowTimer = 0f;
    private float timeScale = 1f;

    [SerializeField] private float moveSpeed = 5f;  // 기본 이동 속도

    //===============================================================

    void Update()
    {
        if (isAlive == false || GamePlayManager.isGamePlaying == false)
        {
            return;
        }

        // Debug.Log($"{targetDistSqr} {range}, {range *range} ");

        // 정지 지속시간 감소
        if (stopDurationRemain > 0)
        {
            stopDurationRemain -= Time.deltaTime;
        }

        // 스턴 지속시간 감소
        if (stunDurationRemain > 0)
        {
            stunDurationRemain -= Time.deltaTime;
        }

        // 슬로우 타이머 관리
        if (slowTimer > 0)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                // 슬로우 효과 해제
                slowAmount = 0f;
                slowDuration = 0f;
            }
        }

        // 이동 속도 계산 (슬로우와 타임스케일 모두 적용)
        float currentSpeed = moveSpeed * (1 - slowAmount) * timeScale;

        // 이동 로직
        Vector3 direction = (t_target.position - t.position).normalized;
        t.position += direction * currentSpeed * Time.deltaTime;

        // 업뎃 성공하면, 
        if (ai.TryUpdate())
        {
            UpdateSpriteDir(t_target.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        lastHitPoint = other.ClosestPoint(transform.position);
    }

    //===========================

    /// <summary>
    ///  이게 init 보다 먼저 호출됨.
    /// </summary>
    public void OnCreatedInPool()
    {
        stateUI = GetComponent<EnemyStateUI>();
        spriteEntity = GetComponent<SpriteEntity>();
        enemyCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        ai = GetComponent<EnemyAI>();

        t = transform;
    }

    public void OnGettingFromPool()
    {
    }


    //=====================================
    /// <summary>
    /// 척 스텟 초기화 - pool에서 생성되거나, 재탕될 때 호출됨. 
    /// </summary>
    /// <param name="data"></param>
    public void Init(EnemyDataSO data, Vector3 initPos)
    {
        //
        transform.position = initPos;
        enemyCollider.enabled = true;

        //
        this.data = data;
        hp = data.maxHp;
        if (data.attackType == EnemyAttackType.Range)
        {
            rangeWeight = UnityEngine.Random.Range(0.8f, 1.2f);
        }

        originalSpeed = data.movementSpeed;
        currentSlowAmount = 0f;

        // data 에 따라 radius 및 이동속도 도 세팅해야함. 
        stunDurationRemain = 0;
        stopDurationRemain = 0;


        ai.Init(this);

        //
        stateUI.Init(this);


        //
        t_target = Player.Instance.transform;

        rb.velocity = Vector3.zero;

        //
        spriteEntity.Init(data.sprite, ai.navAgent.radius, ai.navAgent.height);
    }

    //===========================================================================================

    // public void GetDamaged(Vector3 hitPoint, float damage, bool isEnhancedAttack = false)
    // {
    //     // lastHitPoint = hitPoint == Vector3.zero ? enemyCollider.ClosestPoint(t_target.position) : hitPoint;      // 플레이어와 적 개체의 콜라이더가 겹쳐있는 경우, hitPoint 가 (0,0,0)이 나옴;
    //     lastHitPoint = hitPoint == Vector3.zero ? transform.position : hitPoint;
    //     Debug.Log(lastHitPoint);
    //
    //     GetDamaged(damage, isEnhancedAttack);
    // }

    public void GetDamaged(float damage, bool isEnhancedAttack = false)
    {
        lastHitPoint = transform.position;

        float nockbackPower = 5;
        if (isEnhancedAttack)
        {
            DropInk();
            nockbackPower = 10;
        }

        GetKnockback(nockbackPower, lastHitPoint);
        //
        hp -= damage;
        if (hp <= 0)
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
    public void SetStopped(float duration)
    {
        stopDurationRemain = Math.Max(stopDurationRemain, duration);
        ai.OnStopped();
    }

    /// <summary>
    /// 기절 상태 적용 - 넉백시. or 기타 군중제어 
    /// </summary>
    /// <param name="duration"></param>
    void SetStunned(float duration)
    {
        stunDurationRemain = Math.Max(stunDurationRemain, duration);
        ai.OnStunned();
    }


    // knockBack 
    public void GetKnockback(float power, Vector3 hitPoint)
    {
        SetStunned(0.5f);

        Vector3 dir = (t.position - hitPoint).WithFloorHeight().normalized;
        rb.velocity = dir * power;

        DOTween.Sequence()
            .AppendInterval(0.2f)
            .AppendCallback(() => rb.velocity = Vector3.zero)
            .Play();
    }


    void Die()
    {
        enemyCollider.enabled = false; // 적 탐색 및 총알 충돌에 걸리지 않도록.
        ai.OnDie();

        data.OnDie(this);
        DropItem();
        //
        PlaySequence_Death(); //

        stateUI.OnDie();
        //
        TestManager.Instance.TestSFX_enemyDeath(data.type);
        GamePlayManager.Instance.killCount_currWave++;
        //
    }

    void DropItem()
    {
        // PoolManager.Instance.GetExp( enemyData.exp, transform.position);

        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            PoolManager.Instance.GetMoney(data.exp, transform.position);
        }
    }


    /// <summary>
    /// 강공격에 맞으면 잉크 떨구도록
    /// </summary>
    public void DropInk()
    {
        PoolManager.Instance.GetInk(5, transform.position);
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


    //==================================================
    /// <summary>
    /// 적 사망 애니메이션을 재생하고, 해당 애니메이션이 종료후 오브젝트를 제거한다. 
    /// </summary>
    void PlaySequence_Death()
    {
        DOTween.Sequence()
            .OnComplete(() => { PoolManager.Instance.TakeEnemy(this); })
            .AppendInterval(0.3f)
            .Append(spriteEntity.spriteRenderer.DOFade(0, 1f))
            .Play();
    }

    public void ApplySlow(float amount, float duration)
    {
        // 새로운 슬로우가 더 강하거나, 현재 슬로우가 없을 때만 적용
        if (amount > slowAmount || slowTimer <= 0)
        {
            slowAmount = amount;
            slowDuration = duration;
            slowTimer = duration;
        }
    }

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화
    }
}