using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using BW.Util;

using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController),
                 typeof(SpriteEntity))]
public class Player : Singleton<Player>     // ui 등에서 플레이어 컴포넌트에 접근하기 쉽도록 싱글톤
{
    public Transform t_player;

    public PlayerStatus status;     // 플레이어의 능력치 정보 
    SpriteEntity spriteEntity;

    //======= ui ========
    PlayerStateUI stateUI;

    //
    PlayerInputManager playerInput;
    CharacterController controller;
    [SerializeField] Collider playerCollider;
    [SerializeField] Canvas playerCanvas;
    [SerializeField] Vector3 lastMoveDir;


    public bool isAlive => status.hp > 0;

    public int reinforcementLevel;

    //------- after hit--------
    bool isInvincible => Time.time < lastInvincibleTime + invincibleDuration;
    float lastInvincibleTime = -2f;
    float invincibleDuration = 1f;

    Sequence onHitSeq;


    //-------- skills ------------
    public SerializableDictionary<KeyCode,PlayerSkill> skills;

    //
    PlayerInteraction playerInteraction;

    //====================================================================================

    private void Start()
    {
        // t_camera = Camera.main.transform;
    }

    void Update()
    {
        if (isAlive == false || GamePlayManager.isGamePlaying == false)
        {
            return;
        }

        Move();
        UpdateSpriteDir();

        playerInteraction.OnUpdate();
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
        t_player = transform;

        controller = GetComponent<CharacterController>();
        playerInput = PlayerInputManager.Instance;
        playerCollider = GetComponent<Collider>();
        playerCollider.enabled = true;

        playerInteraction = GetComponent<PlayerInteraction>();

        status = new PlayerStatus();      // 플레이어 스탯 초기화.
        stateUI = GetComponent<PlayerStateUI>();
        stateUI.Init(this);     // 상태 ui 초기화

        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(controller.radius, controller.height);

        reinforcementLevel = status.level;

        playerCanvas.gameObject.SetActive(false);

        InitSkills();

        //
        GameEventManager.Instance.onInitPlayer.Invoke();    // 플레이어 초기화가 필요한 ui 작업을 하기 위함. 
    }

    // 데이터 상의 모든 스킬장착
    public void InitSkills()
    {
        List<SkillItemSO> skillsData = GameManager.Instance.playerData.skills;

        skills = new();
        for(int i=0;i< skillsData.Count;i++)
        {
            ChangeSkill( i, skillsData[i], false);
        }
    }

    // 개별 스킬 장착
    public void ChangeSkill(int idx, SkillItemSO skillData, bool eventCall = true)
    {
        KeyCode keyCode = playerInput.skillKeys[idx];
        PlayerSkill playerSkill =  new PlayerSkill( skillData); 
        skills[ keyCode] = playerSkill;

        if (eventCall)
        {
            GameEventManager.Instance.onChangeSkill.Invoke( keyCode, playerSkill );
        }

    }

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

        // 땅위의 경우
        Vector2 moveVector = playerInput.moveVector;

        // Debug.Log(moveVector);
        lastMoveDir = transform.right * moveVector.x + transform.forward * moveVector.y;
        lastMoveDir.y = 0;      // 방향 조절에 필요 없기떄문.
        controller.Move(lastMoveDir.normalized * Time.deltaTime * status.movementSpeed);
    }

    /// <summary>
    /// 마지막 이동한 방향을 보도록함. 
    /// </summary>
    void UpdateSpriteDir()
    {
        spriteEntity.Flip(lastMoveDir.x);
    }


    //========================================================================
    public void GetDamaged(float amount)
    {
        status.hp -= amount;

        if (status.hp <= 0)
        {
            Die();
        }

        // ui
        stateUI.UpdateCurrHp(status.hp);

        PoolManager.Instance.GetDamageText(transform.position, amount, DamageType.DMG_PLAYER);
    }


    public void GetHealed(float amount)
    {
        status.hp += amount;

        // ui
        stateUI.UpdateCurrHp(status.hp);

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
    public void GetExp(float exp)
    {
        //
        if (status.GetExp(exp))
        {
            OnLevelUp();
        }

        stateUI.UpdateCurrExp(status.currExp);
    }


    public void OnLevelUp()
    {
        GameEventManager.Instance.onLevelUp.Invoke();

        GetHealed(50);

        stateUI.UpdateLevelText(status.level);
        stateUI.UpdateMaxExp(status.maxExp);

        // Debug.Log("플레이어 레벨업!");
    }







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
}