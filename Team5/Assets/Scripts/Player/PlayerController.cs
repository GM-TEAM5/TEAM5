using UnityEngine;



    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        PlayerInputManager playerInput;
        CharacterController controller;
        

        [SerializeField]
        private Vector3 playerVelocity;
        
        #region Move
        [SerializeField] private Vector3 lastMoveDir;
        [SerializeField] private float playerSpeed = 5f;
        #endregion



        //====================================================================================

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            playerInput = PlayerInputManager.Instance;
            
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


        void Move()
        {
            // 땅위의 경우
            Vector2 moveVector = playerInput.moveVector;

            // Debug.Log(moveVector);
            lastMoveDir = transform.right* moveVector.x + transform.forward * moveVector.y;
            lastMoveDir.y = 0;      // 방향 조절에 필요 없기떄문.
            controller.Move(lastMoveDir.normalized * Time.deltaTime * playerSpeed);
        }
    }