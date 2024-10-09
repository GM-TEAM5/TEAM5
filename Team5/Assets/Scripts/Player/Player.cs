using System.Net.Mail;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;



    [RequireComponent(typeof(CharacterController))]
    public class Player : Singleton<Player>     // ui 등에서 플레이어 컴포넌트에 접근하기 쉽도록 싱글톤
    {
        public PlayerStatus status;     // 플레이어의 능력치 정보 
        
        PlayerStateUI stateUI;
        
        PlayerInputManager playerInput;
        CharacterController controller;
        

        
        
        #region Move
        // [SerializeField] Vector3 playerVelocity;
        [SerializeField] Vector3 lastMoveDir;
        #endregion



        //====================================================================================

        private void Start()
        {

            InitPlayer();
            
            // t_camera = Camera.main.transform;
        }

        void Update()
        {
            //controller.Move(playerVelocity * Time.deltaTime);

            Move();
            

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

        /// <summary>
        /// 플레이어 초기화시 호출. 
        /// </summary>
        public void InitPlayer()
        {
            controller = GetComponent<CharacterController>();
            playerInput = PlayerInputManager.Instance;
            
            status = new PlayerStatus();      // 플레이어 스탯 초기화.

            stateUI = GetComponent<PlayerStateUI>();
            stateUI.Init();     // 상태 ui 초기화
        }

        //========================================================================

        void Move()
        {
            // 땅위의 경우
            Vector2 moveVector = playerInput.moveVector;

            // Debug.Log(moveVector);
            lastMoveDir = transform.right* moveVector.x + transform.forward * moveVector.y;
            lastMoveDir.y = 0;      // 방향 조절에 필요 없기떄문.
            controller.Move(lastMoveDir.normalized * Time.deltaTime * status.movementSpeed);
        }


        //========================================================================
        public void GetDamaged(float damage)
        {
            status.hp -= damage;

            if (status.hp <=0)
            {
                Debug.LogError("플레이어 사망");
            }

            // ui
            stateUI.UpdateCurrHp();
        }


        public void GetHealed(float heal)
        {
            status.hp += heal;

            // ui
            stateUI.UpdateCurrHp();
        }


        //=====================================================
        public void GetExp(float exp)
        {
            status.currExp += exp;
            
            // 레벨업 체크
            if (status.currExp >= status.maxExp)
            {
                LevelUp();
            }

            stateUI.UpdateCurrExp();
        }


        public void LevelUp()
        {
            status.level ++;
            status.currExp -= status.maxExp;    // 현재 경험치 감소
            //그 다음으로  status.maxExp 를 공식에 따라 증가시키던지 해야함. 

            stateUI.UpdateLevelText();
            stateUI.UpdateMaxExp();

            Debug.Log("플레이어 레벨업!");
        }
    }