using System.Collections;
using System.Collections.Generic;
using BW.Util;

using UnityEngine;
using DG.Tweening;



[RequireComponent(typeof(CharacterController), typeof(SpriteEntity))]
[RequireComponent(typeof(PlayerEquipments), typeof(PlayerSkills), typeof(PlayerInteraction))]
public class Player : Singleton<Player>, ITimeScaleable     // ui 등에서 플레이어 컴포넌트에 접근하기 쉽도록 싱글톤
{
    public Transform t;

    public PlayerStatus status;     // 플레이어의 능력치 정보 
    SpriteEntity spriteEntity;

    //======= ui ========
    public PlayerStateUI stateUI;
    public PlayerDraw playerDraw;
    //
    PlayerInputManager playerInput;
    CharacterController controller;
    [SerializeField] Collider playerCollider;
    [SerializeField] Canvas playerCanvas;
    [SerializeField] Vector3 lastMoveDir;

    public PlayerAnimator animator;

    public bool isAlive => status.currHp > 0;

    public int reinforcementLevel;

    //------- after hit--------
    bool isInvincible => Time.time < lastInvincibleTime + invincibleDuration;
    float lastInvincibleTime = -2f;
    float invincibleDuration = 1f;

    Sequence onHitSeq;


    //-------- skills ------------
    public SerializableDictionary<KeyCode, PlayerSkill> _skills;


    //
    public PlayerSkills skills;
    //
    public PlayerEquipments equipments;

    //
    PlayerInteraction playerInteraction;
    public PlayerBasicAttack basicAttack;

    // 스턴
    public bool isStunned => stunDurationRemain > 0;


    // 넉백
    [SerializeField] Vector3 knockbackVelocity;
    [SerializeField] float knockbackDamping = 10f; // 넉백 감소 속도
    [SerializeField] bool isNockbackOn => knockbackVelocity.magnitude > 0.3f;

    //
    public float stunDurationRemain;
    public bool stunned => stunDurationRemain > 0;

    //====================================================================================

    private Coroutine inkChargeRoutine;

    [Header("Ink Charge Settings")]
    [SerializeField] float inkChargeInterval = 0.1f;  // 잉크 충전 간격

    private float timeScale = 1f;

    // public PlayerDraw playerDraw { get; private set; }

    private void Start()
    {
        // t_camera = Camera.main.transform;
        StartInkChargeRoutine();
        GameManager.Instance.RegisterTimeScaleable(this);
    }

    private void StartInkChargeRoutine()
    {
        if (inkChargeRoutine != null)
        {
            StopCoroutine(inkChargeRoutine);
        }
        inkChargeRoutine = StartCoroutine(AutoChargeInkRoutine());
    }

    private IEnumerator AutoChargeInkRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(inkChargeInterval);

        while (true)
        {
            if (!playerDraw.isDrawing && status.currInk < status.maxInk)
            {
                float chargeAmount = status.inkChargeRate * inkChargeInterval;
                status.currInk += chargeAmount;
                OnUpdateInk();
            }
            yield return wait;
        }
    }

    public void UseInk(float amount)
    {
        status.currInk -= amount;
        OnUpdateInk();
    }

    public bool HasEnoughInk(float amount)
    {
        return status.currInk >= amount;
    }

    private void OnUpdateInk()
    {
        stateUI.UpdateCurrInk(status.currInk);
        OnUpdateStatus();
    }

    void OnDisable()
    {
        if (inkChargeRoutine != null)
        {
            StopCoroutine(inkChargeRoutine);
            inkChargeRoutine = null;
        }
    }

    void Update()
    {
        if (isAlive == false || GamePlayManager.isGamePlaying == false)
        {
            return;
        }

        // 스턴 지속시간 감소
        if (stunDurationRemain > 0)
        {
            stunDurationRemain -= Time.unscaledDeltaTime * timeScale;
        }
        else if (isNockbackOn)
        {
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDamping * Time.unscaledDeltaTime * timeScale);
        }

        skills.OnUpdate();

        Move();
        UpdateSpriteDir();

        playerInteraction.OnUpdate();
        playerDraw.OnUpdate();
    }


    //============================================================================
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            //
            // Debug.Log($"{Time.time} {lastInvincibleTime} {invincibleDuration}");
            if (isInvincible)
            {
                return;
            }
            lastInvincibleTime = Time.time;
            PlayAnim_PlayerHit();

            //
            EnemyProjectile ep = other.GetComponent<EnemyProjectile>();
            GetDamaged(ep.damage);
        }

        if (other.CompareTag("DropItem"))
        {
            DropItem di = other.GetComponent<DropItem>();
            di.PickUp();
        }

    }


    /// <summary>
    /// 플레이어 초기화시 호출. 
    /// </summary>
    public void InitPlayer(PlayerDataSO playerData)
    {
        t = transform;
        //
        t.position = StageManager.Instance.currStage.playerInitPos;  // 플레이어 위치 지정 

        controller = GetComponent<CharacterController>();
        playerInput = PlayerInputManager.Instance;
        playerCollider = GetComponent<Collider>();
        playerCollider.enabled = true;

        // 플레이어 스탯 초기화.
        if (playerData.savedStatus != null)
        {
            status = new(playerData.savedStatus);
        }
        else
        {
            status = new();
        }

        //--------- after init status --------------
        basicAttack = GetComponentInChildren<PlayerBasicAttack>();

        playerDraw = GetComponentInChildren<PlayerDraw>();
        playerDraw.Init();

        playerInteraction = GetComponent<PlayerInteraction>();
        equipments = GetComponent<PlayerEquipments>();
        equipments.InitEquipments();                      // 스텟을 조정하기 때문에, 스탯 초기화 이후에 진행해야함. 

        skills = GetComponent<PlayerSkills>();
        skills.Init();



        animator = GetComponentInChildren<PlayerAnimator>();
        //----------- after init finished ---------------------

        stateUI = GetComponent<PlayerStateUI>();
        stateUI.Init(this);     // 

        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(controller.radius, controller.height);

        // reinforcementLevel = status.level;

        playerCanvas.gameObject.SetActive(false);



        //
        GameEventManager.Instance.onInitPlayer.Invoke();    // 플레이어 초기화가 필요한 ui 작업을 하기 위함. 
    }

    #region Equipment 
    /// <summary>
    ///  자동으로 빈칸에 아이템 장착 
    /// </summary>
    /// <param name="equipmentData"></param>
    public void EquipAutomatically(EquipmentItemSO equipmentData)
    {
        if (equipments.TryEquip(equipmentData) == false)
        {
            Debug.LogError("그럴리가 없는데...?");   // 이거 나오면 로직 잘못짠거임;
        }
    }

    #endregion


    //========================================================================

    /// <summary>
    /// 움직임
    /// </summary>
    void Move()
    {
        // if (canMoveAfterMeleeAttack == false)
        // {
        //     return;
        // }
        Vector3 moveVector = Vector3.zero;

        // 넉백 처리
        if (isNockbackOn)
        {
            moveVector = knockbackVelocity; // 넉백 벡터 추가
        }
        else
        {
            // 땅위의 경우
            Vector2 inputVector = playerInput.moveVector;

            // Debug.Log(moveVector);
            lastMoveDir = transform.right * inputVector.x + transform.forward * inputVector.y;
            lastMoveDir.y = 0;      // 방향 조절에 필요 없기떄문.

            moveVector = lastMoveDir.normalized * status.movementSpeed;
            animator.OnMove(moveVector.magnitude);
        }

        controller.Move(moveVector * Time.unscaledDeltaTime * timeScale);
    }

    /// <summary>
    /// 마지막 이동한 방향을 보도록함. 
    /// </summary>
    void UpdateSpriteDir()
    {
        spriteEntity.Flip(lastMoveDir.x);
    }


    //========================================================================
    public void GetImpulsiveDamaged(float dmg, Vector3 enemyPos, Vector3 hitPoint, float impulse)
    {
        GetNockback(hitPoint, enemyPos, impulse);
        GetDamaged(dmg);
    }

    public void GetDamaged(float amount)
    {
        status.currHp -= amount;

        if (status.currHp <= 0)
        {
            Die();
        }

        // ui
        stateUI.UpdateCurrHp(status.currHp);

        PoolManager.Instance.GetDamageText(transform.position, amount, DamageType.DMG_PLAYER);
    }

    //==============================================================
    void GetNockback(Vector3 enemyPos, Vector3 hitPoint, float impulse)
    {
        Vector3 dir = t.position - hitPoint;
        if (dir == Vector3.zero)
        {
            dir = t.position - enemyPos;
        }
        dir = dir.WithFloorHeight().normalized;

        knockbackVelocity = dir * impulse;
        float stunDuration = impulse * 0.02f;
        SetStunned(stunDuration);
    }

    void SetStunned(float duration)
    {
        stunDurationRemain = Mathf.Max(stunDurationRemain, duration);
    }


    public void GetHealed(float amount)
    {
        status.currHp += amount;

        // ui
        stateUI.UpdateCurrHp(status.currHp);

        PoolManager.Instance.GetDamageText(transform.position, amount, DamageType.HEAL_PLAYER);
    }

    public void GetInk(float amount)
    {
        status.currInk += amount;

        stateUI.UpdateCurrInk(status.currInk);


    }


    void Die()
    {
        // brushAttack.drawArea.gameObject.SetActive(false);

        // playerCollider.enabled = false;        // 이게 brush collider 는 true로 세팅하네??


        GamePlayManager.Instance.GameOver();
    }



    //=====================================================
    // public void GetExp(float exp)
    // {
    //     //
    //     if (status.GetExp(exp))
    //     {
    //         OnLevelUp();
    //     }

    //     stateUI.UpdateCurrExp(status.currExp);
    // }


    // public void OnLevelUp()
    // {
    //     GameEventManager.Instance.onLevelUp.Invoke();

    //     GetHealed(50);

    //     // stateUI.UpdateLevelText(status.level);
    //     // stateUI.UpdateMaxExp(status.maxExp);

    //     // Debug.Log("플레이어 레벨업!");
    // }

    #region  UI

    public void OnUpdateStatus()
    {
        stateUI.UpdateMaxHp(status.maxHp);
        stateUI.UpdateMaxInk(status.maxInk);
    }

    #endregion
    //
    #region ==== 연출 ======


    /// <summary>
    ///  맞으면 빨간색으로 깜빡깜빡임
    /// </summary>
    public void PlayAnim_PlayerHit()
    {
        Color targetColor = new Color(1, 0.2f, 0.2f);
        Color originColor = Color.white;

        if (onHitSeq != null && onHitSeq.IsActive())
        {
            onHitSeq.Kill();
        }

        onHitSeq = DOTween.Sequence()
        .OnComplete(() =>
        {
            spriteEntity.spriteRenderer.color = originColor;

        })
        .Append(spriteEntity.spriteRenderer.DOColor(targetColor, 0.05f))
        .Append(spriteEntity.spriteRenderer.DOColor(originColor, 0.05f))
        .SetLoops(4)
        .Play();
    }

    /// <summary>
    /// 포탈에서 나오거나/들어가는 애니메이션 - 플레이어 페이드 인/아웃
    /// </summary>
    /// <param name="isEnter"></param>
    public Sequence GetSequence_EnterPortal(bool isEnter, float playTime)
    {

        float startValue = isEnter ? 1 : 0;
        float targetValue = isEnter ? 0 : 1;

        spriteEntity.spriteRenderer.color = new Color(1, 1, 1, startValue);
        return DOTween.Sequence()
        .Append(spriteEntity.spriteRenderer.DOFade(targetValue, playTime));
    }

    public void OnStartGamePlay()
    {
        // playerCanvas.gameObject.SetActive(true);
    }

    #endregion

    // 또는 새로운 public 메서드 추가
    public void UpdateInk(float amount)
    {
        status.currInk = amount;
        stateUI.UpdateCurrInk(status.currInk);
    }

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
        // 필요한 경우 자식 컴포넌트들의 timeScale도 설정
        // if (basicAttack != null) basicAttack.SetTimeScale(scale);
        if (playerDraw != null) playerDraw.SetTimeScale(scale);
        // ... 다른 타임스케일이 필요한 컴포넌트들
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterTimeScaleable(this);
        }
    }
}

public interface ITimeScaleable
{
    void SetTimeScale(float scale);
}