using System.Collections.Generic;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController),
                 typeof(SpriteEntity))]
public class Player : Singleton<Player>     // ui 등에서 플레이어 컴포넌트에 접근하기 쉽도록 싱글톤
{
    // [SerializeField] Sprite playerSprite;

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

    // 붓칠
    BrushAttack brushAttack;
    bool isDrawing = false;



    // [SerializeField] Vector3 playerVelocity;
    [SerializeField] Vector3 lastMoveDir;


    public bool isAlive => status.hp >0;

    public int reinforcementLevel;


    //====================================================================================

    private void Start()
    {

        InitPlayer();

        // t_camera = Camera.main.transform;
    }

    void Update()
    {
        //controller.Move(playerVelocity * Time.deltaTime);
        if (isAlive==false)
        {
            return;
        }


        Move();
        TryUseSkills();
        Drawing();


        UpdateSpriteDir();
        // Rotate(playerInput.mouseDir);
        //shoot
        // if (playerInput.leftClick)
        // {
        //     Debug.Log("좌클 중");
        // }
        // if (playerInput.rightClick)
        // {
        //     Debug.Log("우클 중");
        // }

    }

    //============================================================================
    void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("EnemyProjectile"))
        {
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

        brushAttack = GetComponent<BrushAttack>();

        reinforcementLevel = status.level;
    }

    //========================================================================

    void Move()
    {
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

        PoolManager.Instance.GetDamageText(transform.position, amount);
    }


    public void GetHealed(float amount)
    {
        status.hp += amount;

        // ui
        stateUI.UpdateCurrHp(status.hp);

        PoolManager.Instance.GetDamageText(transform.position, amount);
    }

    void Die()
    {
        brushAttack.drawArea.gameObject.SetActive(false);
        
        playerCollider.enabled = false;        // 이게 brush collider 는 true로 세팅하네??


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

    public void TryUseSkills()
    {
        if (GamePlayManager.isGamePlaying==false)
        {
            return;
        }
        
        //
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].isAvailable)
            {
                skills[i].Use();
            }
        }
    }

    #endregion

    #region ===== Drawing =====
    void Drawing()
    {
        // 잉크 충전
        ChargeInk();

        // UI 업데이트
        stateUI.UpdateCurrInk(status.currInk);

        // 그림 그리기 여부에 따라 처리
        if (playerInput.drawAction.ReadValue<float>() > 0 && status.currInk > 0)
        {
            // 잉크 사용
            UseInk();

            // 공격 실행
            Vector3 mouseWorldPos = playerInput.mouseWorldPos;
            brushAttack.Brushing(mouseWorldPos);

            if (!isDrawing)
            {
                isDrawing = true;
                brushAttack.StartBrushing();
            }
        }
        else if (isDrawing)
        {
            isDrawing = false;
            brushAttack.StopBrushing();
        }
    }

    void ChargeInk()
    {
        // 그리지 않고 있을 때만 충전
        if (!isDrawing && status.currInk < status.maxInk)
        {
            status.currInk += status.inkChargeRate * Time.deltaTime;
            status.currInk = Mathf.Min(status.currInk, status.maxInk);
        }
    }

    void UseInk()
    {
        // 붓칠 게이지가 0이 되지 않도록 소모
        status.currInk -= status.inkUseRate * Time.deltaTime;
        status.currInk = Mathf.Max(status.currInk, 0f);
    }
    #endregion
}