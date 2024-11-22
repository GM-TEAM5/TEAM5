using System.Collections;
using System.Collections.Generic;
using BW.Util;

using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;


[RequireComponent(typeof(CharacterController), typeof(SpriteEntity))]
[RequireComponent(typeof(PlayerEquipments), typeof(PlayerInteraction))]
public class Player : Singleton<Player>     // ui 등에서 플레이어 컴포넌트에 접근하기 쉽도록 싱글톤
{
    public Transform t;

    public PlayerStatus status;     // 플레이어의 능력치 정보 
    SpriteEntity spriteEntity;

    //======= ui ========
    public PlayerStateUI stateUI;
    PlayerDraw playerDraw;
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
    public SerializableDictionary<KeyCode, PlayerSkill> skills;



    //
    public PlayerEquipments playerEquipments;

    //
    PlayerInteraction playerInteraction;
    PlayerBasicAttack playerBasicAttack;

    // 스턴
    public bool isStunned => isNockbackOn;


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

    private void Start()
    {
        // t_camera = Camera.main.transform;
        StartInkChargeRoutine();
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

        // 스킬 입력 체크 추가
        CheckSkillInput();

        playerBasicAttack.OnUpdate();

        Move();
        UpdateSpriteDir();

        playerInteraction.OnUpdate();
        playerDraw.OnUpdate();
    }

    // 새로운 메서드 추가
    void CheckSkillInput()
    {
        if (playerInput.pressedNumber != 0)
        {
            KeyCode keyCode = playerInput.skillKeys[playerInput.pressedNumber - 1];
            if (skills.ContainsKey(keyCode))
            {
                skills[keyCode].Use();
            }
            playerInput.pressedNumber = 0;
        }
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
    public void InitPlayer()
    {
        t = transform;

        controller = GetComponent<CharacterController>();
        playerInput = PlayerInputManager.Instance;
        playerCollider = GetComponent<Collider>();
        playerCollider.enabled = true;

        status = new PlayerStatus();      // 플레이어 스탯 초기화.
        //--------- after init status --------------
        playerDraw = GetComponentInChildren<PlayerDraw>();
        playerDraw.Init();

        playerInteraction = GetComponent<PlayerInteraction>();
        playerEquipments = GetComponent<PlayerEquipments>();
        playerEquipments.InitEquipments();                      // 스텟을 조정하기 때문에, 스탯 초기화 이후에 진행해야함. 

        playerBasicAttack = GetComponentInChildren<PlayerBasicAttack>();

        animator = GetComponentInChildren<PlayerAnimator>();
        //----------- after init finished ---------------------

        stateUI = GetComponent<PlayerStateUI>();
        stateUI.Init(this);     // 

        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(controller.radius, controller.height);

        // reinforcementLevel = status.level;

        playerCanvas.gameObject.SetActive(false);

        InitSkills();

        //
        GameEventManager.Instance.onInitPlayer.Invoke();    // 플레이어 초기화가 필요한 ui 작업을 하기 위함. 
    }

    // 데이터 상의 모든 스킬장착
    public void InitSkills()
    {
        if (GameManager.Instance == null || GameManager.Instance.playerData == null)
        {
            Debug.LogError("GameManager or playerData is null");
            return;
        }

        List<SkillItemSO> skillsData = GameManager.Instance.playerData.skills;
        if (skillsData == null)
        {
            Debug.LogError("skillsData is null");
            return;
        }

        skills = new();

        // PlayerInputManager가 초기화되었는지 확인
        if (PlayerInputManager.Instance != null)
        {
            for (int i = 0; i < skillsData.Count; i++)
            {
                if (skillsData[i] != null)
                {
                    ChangeSkill(i, skillsData[i], false);
                }
            }
        }
        else
        {
            Debug.LogError("PlayerInputManager is not initialized");
        }
    }

    // 개별 스킬 장착
    public void ChangeSkill(int idx, SkillItemSO skillData, bool eventCall = true)
    {
        if (PlayerInputManager.Instance == null)
        {
            Debug.LogError("PlayerInputManager is null");
            return;
        }

        if (idx >= PlayerInputManager.Instance.skillKeys.Count)
        {
            Debug.LogError($"Skill index {idx} is out of range");
            return;
        }

        KeyCode keyCode = PlayerInputManager.Instance.skillKeys[idx];
        PlayerSkill playerSkill = new PlayerSkill(skillData);

        if (skills != null && skills.ContainsKey(keyCode))
        {
            skills[keyCode].skillData.OnUnEquip();
        }

        skills[keyCode] = playerSkill;
        skillData.OnEquip();

        if (eventCall && GameEventManager.Instance != null)
        {
            GameEventManager.Instance.onChangeSkill.Invoke(keyCode, playerSkill);
        }
    }




    #region Equipment 
    /// <summary>
    ///  자동으로 빈칸에 아이템 장착 
    /// </summary>
    /// <param name="equipmentData"></param>
    public void EquipAutomatically(EquipmentItemSO equipmentData)
    {
        if (playerEquipments.TryEquip(equipmentData) == false)
        {
            Debug.LogError("그럴리가 없는데...?");   // 이거 나오면 로직 잘못짠거임;
        }
    }

    /// <summary>
    /// 직접 해당 칸에 아이템 장착
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="equipmentData"></param>
    public void Equip(int idx, EquipmentItemSO equipmentData)
    {
        playerEquipments.Equip(idx, equipmentData);
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
        }

        controller.Move(moveVector * Time.unscaledDeltaTime * timeScale);

        animator.OnMove(moveVector.magnitude);
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
        SetStunned(0.2f);
    }

    void SetStunned(float duration)
    {
        stunDurationRemain = System.Math.Max(stunDurationRemain, duration);
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
        playerCanvas.gameObject.SetActive(true);
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

        // 플레이어의 모든 ITimeScaleable 컴포넌트에 적용
        foreach (var component in GetComponentsInChildren<ITimeScaleable>())
        {
            component.SetTimeScale(scale);
        }

        // 애니메이터도 정상 속도 유지
        if (animator != null)
        {
            animator.SetTimeScale(scale);
        }
    }
}

public interface ITimeScaleable
{
    void SetTimeScale(float scale);
}