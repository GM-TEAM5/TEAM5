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
    PlayerSkillsUI skillsUI;


    //
    PlayerInputManager playerInput;
    CharacterController controller;
    [SerializeField] Collider playerCollider;


    List<PlayerSkill> skills = new();
    int maxSkillNum = 5;
    int currentSkill = 0;

    // 붓칠
    // bool isDrawingMode = false;

    // [SerializeField] Vector3 playerVelocity;
    [SerializeField] Vector3 lastMoveDir;


    public bool isAlive => status.hp > 0;

    public int reinforcementLevel;


    // -- melee attack ---
    float lastMeleeAttackTime;
    bool meleeAttackOk => Time.time >= lastMeleeAttackTime + status.attackSpeed;
    int combo=0;
    bool canMoveAfterMeleeAttack => Time.time >= lastMeleeAttackTime + focusTime;
    float focusTime = 0.1f;
    

    //------- after hit--------
    bool isInvincible => Time.time < lastInvincibleTime + invincibleDuration;
    float lastInvincibleTime = -2f;
    float invincibleDuration = 1f;

    Sequence onHitSeq;


    //====================================================================================

    private void Start()
    {

        InitPlayer();
        skills[currentSkill].On();
        // t_camera = Camera.main.transform;
    }

    void Update()
    {
        //controller.Move(playerVelocity * Time.deltaTime);
        if (isAlive==false || GamePlayManager.isGamePlaying == false )
        {
            return;
        }


        Move();
        TryUseSkills();

        UpdateSpriteDir();

        // 스킬 변경
        if (playerInput.pressedNumber != currentSkill)
        {
            ChangeSkill(playerInput.pressedNumber);
        }
    }

    //============================================================================
    void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("EnemyProjectile"))
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
        else if ( other.CompareTag("DropItem"))
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

        status = new PlayerStatus();      // 플레이어 스탯 초기화.
        foreach (var skill in TestManager.Instance.initSkillData)
        {
            GetSkill(skill);
        }

        

        stateUI = GetComponent<PlayerStateUI>();
        stateUI.Init(this);     // 상태 ui 초기화

        skillsUI = FindObjectOfType<PlayerSkillsUI>();
        if(skillsUI != null)
        {
            skillsUI.Init(skills);
        }
        

        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(controller.radius, controller.height);

        // brushAttack = GetComponent<BrushAttack>();

        //
        reinforcementLevel = status.level;
    }

    //========================================================================

    /// <summary>
    ///  좌클릭시 근접공격 - 1,2타 : 찌르기, 3타 베기 
    /// </summary>
    void MeleeAttack()
    {        
        if (meleeAttackOk == false)
        {
            return;
        }

        //
        lastMeleeAttackTime = Time.time;
        bool isEnhancedAttack = ++combo==3;
        //
        if( isEnhancedAttack )
        {
            combo = 0;
            lastMeleeAttackTime += status.attackSpeed*2;    // 강화 후엔 딜레이 좀 두려고

            MeleeAttack_Enhanced();
        }
        else
        {
            MeleeAttack_Normal();
        }               
    }


    private Vector3 lastCastDirection;  // 마지막으로 캐스팅한 방향 저장
    private bool debug_normalAttack = false;     // 현재 캐스팅 중인지 여부

    /// <summary>
    /// 일반공격 - 좁은 범위를 찌른다.
    /// </summary>
    void MeleeAttack_Normal()
    {
        Debug.Log("일반공격");
        Vector3 mouseWorldPos = playerInput.mouseWorldPos;

        Vector3 dir = (mouseWorldPos- t_player.position).WithFloorHeight().normalized;
        float radius = 1;
        float maxDist = 5;

        RaycastHit[] hits = Physics.SphereCastAll(t_player.position.WithStandardHeight(), radius, dir, maxDist, GameConstants.enemyLayer);

        // 충돌된 오브젝트들에 대해 반복 실행
        for(int i=0;i<hits.Length;i++)
        {
            RaycastHit hit = hits[i];
            
            // 적에게 피해를 입히는 로직
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamaged(hit.point, status.ad);
            }
        }

        TestManager.Instance.TestSFX_NormalAttack();
        // for debug.
        lastCastDirection = dir;
        debug_normalAttack = true;
    }



    // Gizmos를 사용해 SphereCast 범위를 그리기
    void OnDrawGizmos()
    {
        if (debug_normalAttack)
        {
            // 캐스팅 시작점
            Vector3 start = t_player.position.WithStandardHeight();

            // 캐스팅 끝점
            Vector3 end = start + lastCastDirection * 5;

            // 구형의 시작 지점과 끝 지점에 대한 와이어 스피어 그리기
            Gizmos.color = Color.red;  // 시작점
            Gizmos.DrawWireSphere(start, 1);

            Gizmos.color = Color.green; // 끝점
            Gizmos.DrawWireSphere(end, 1);

            // 시작점과 끝점을 연결하는 선 그리기
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, end);
        }
    }
    /// <summary>
    /// 강화 공격 - 플레어가 보는 방향의 180도를 휩쓸기 공격을 한다.
    /// </summary>
    void MeleeAttack_Enhanced()
    {
        Debug.Log("강화공격!!!!!");
        
        Vector3 mouseWorldPos = playerInput.mouseWorldPos;
        Vector3 mouseDir = (mouseWorldPos- t_player.position).WithFloorHeight().normalized;

        // OverlapSphere를 사용해 모든 적을 반경 내에서 감지
        float maxDist = 8;
        Collider[] hitColliders = Physics.OverlapSphere(t_player.position.WithStandardHeight(), maxDist, GameConstants.enemyLayer);

        for(int i=0;i<hitColliders.Length;i++)
        {
            Collider hitCollider = hitColliders[i];


            // 방향 벡터 계산 (origin에서 적으로)
            Vector3 enemyDir = (hitCollider.transform.position - t_player.position).normalized;
            float angleWithEnemy = Vector3.Angle(mouseDir, enemyDir);

            // 각도가 설정된 범위 내에 있는지 확인 (90도 이하만 허용 = 반구)
            if (angleWithEnemy <= 90)
            {
                // 적에게 피해를 입힘
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetDamaged( hitCollider.ClosestPoint( t_player.position ), status.ad  *1.5f, true);
                }
            }
        }
        lastCastDirection = mouseDir;
        debug_normalAttack = true;


        TestManager.Instance.TestSFX_EnhancedAttack();
    }

    /// <summary>
    /// 움직임
    /// </summary>
    void Move()
    {        
        if( canMoveAfterMeleeAttack == false)
        {
            return;
        }
        
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

        PoolManager.Instance.GetDamageText(transform.position, amount , DamageType.DMG_PLAYER);
    }


    public void GetHealed(float amount)
    {
        status.hp += amount;

        // ui
        stateUI.UpdateCurrHp(status.hp);

        PoolManager.Instance.GetDamageText(transform.position, amount , DamageType.HEAL_PLAYER);
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
        if(status.GetExp(exp))
        {
            OnLevelUp();
        }

        stateUI.UpdateCurrExp(status.currExp);
    }


    public void OnLevelUp()
    {
        GameEventManager.Instance.onLevelUp.Invoke();

        stateUI.UpdateLevelText(status.level);
        stateUI.UpdateMaxExp(status.maxExp);

        // Debug.Log("플레이어 레벨업!");
    }


    #region ===== Skill =====
    public void GetSkill(PlayerSkillSO skillData)
    {
        if (skills.Count < maxSkillNum)
        {
            PlayerSkill skill = new(skillData);
            skills.Add(skill);
        }

        // 그리고 ui 업뎃
    }

    public void ChangeSkill(int index)
    {
        skills[currentSkill].Off();
        currentSkill = index;
        skills[currentSkill].On();
    }

    public void TryUseSkills()
    {
        if (!GamePlayManager.isGamePlaying)
        {
            return;
        }

        // 공격 시
        if (playerInput.isMouseLeftButtonOn)
        {
            // TODO SO로 옮기기 / 기본공격
            if (currentSkill == 0)
            {
                MeleeAttack();
            }
            else if (status.currInk > 0)
            {
                // 잉크 사용
                UseInk();
            }
        }

        // 공격 실행
        if (status.currInk > 0)
        {
            skills[currentSkill].Use(playerInput.isMouseLeftButtonOn, playerInput.mouseWorldPos);
        }
    }

    void ChargeInk()
    {
        status.currInk += status.inkChargeRate * Time.deltaTime;
        status.currInk = Mathf.Min(status.currInk, status.maxInk);
        stateUI.UpdateCurrInk(status.currInk);
    }

    void UseInk()
    {
        // 붓칠 게이지가 0이 되지 않도록 소모
        status.currInk -= status.inkUseRate * Time.deltaTime;
        status.currInk = Mathf.Max(status.currInk, 0f);
        stateUI.UpdateCurrInk(status.currInk);
    }
    #endregion






    #region ==== 연출 ======

    
    /// <summary>
    ///  맞으면 빨간색으로 깜빡깜빡임
    /// </summary>
    public void PlayAnim_PlayerHit()
    {
        Color targetColor = new Color(1,0.2f,0.2f);
        Color originColor = Color.white;

        if(onHitSeq!=null && onHitSeq.IsActive())
        {
            onHitSeq.Kill();
        }

        onHitSeq = DOTween.Sequence()
        .OnComplete( ()=>{
            spriteEntity.spriteRenderer.color = originColor;

        } )
        .Append( spriteEntity.spriteRenderer.DOColor( targetColor, 0.05f))
        .Append( spriteEntity.spriteRenderer.DOColor( originColor, 0.05f))
        .SetLoops(4)
        .Play();
    }

    #endregion
}