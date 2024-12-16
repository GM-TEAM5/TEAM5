using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// 
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : Singleton<PlayerInputManager>
{
    // [SerializeField] PlayerInput playerInput;

    public InputAction moveAction;      // WASD
    public InputAction mouseLeftButtonAction;       // 클릭
    public InputAction drawAction;          // 그리기
    public InputAction utilAction;          // 기본 대쉬
    public InputAction scrollAction;        // 스크롤 ( 즉발형 스킬 )
    public InputAction interactAction;      //상호작용            // - 이벤트 형식으로 빼
    public InputAction flowControlAction;         // 게임 일시정지, 씬별로 다른 기능. 

    // public KeyCode secondaryInteractAction = KeyCode.R;         // - 이벤트 형식으로 빼

    // public KeyCode pauseAction = KeyCode.Escape;                // - 이벤트 형식으로 빼
    InputAction lookAction;


    public bool interact;
    // public bool secondaryInteract;
    public bool flowControl;
    public bool isMouseLeftButtonOn; //마우스를 누르고 있는 중인지.

    public bool basicAttack;
    public bool draw;
    public bool scroll;
    public bool util;

    public bool ability;


    //
    public Vector2 moveVector { get; private set; }
    public Vector2 mouseMoveVector { get; private set; }


    public Vector3 mouseDir { get; private set; } // 마우스가 가리키는 방향 
    public Vector3 mousePosition { get; private set; }
    public Vector3 mouseWorldPos { get; private set; } // 마우스가 가리키는 곳의 월드 좌표 
    public float xAxis { get; private set; } //마우스 움직임 x축
    public float yAxis { get; private set; } // 마우스 움직임 y축

    // TODO: 그리기 범위 수정
    private Plane drawingPlane;

    public int pressedNumber { get; set; } = 0; // 숫자 키 입력
    //

    // [SerializeField] LayerMask aimColliderLayerMask = new();
    public List<KeyCode> skillKeys = new() { KeyCode.Q, KeyCode.E, KeyCode.LeftShift, KeyCode.Alpha4 };



    //
    // public KeyCode skillKey_draw = KeyCode.Q;               // 이거 이벤트로,
    // public KeyCode skillKey_scroll = KeyCode.E;             // 이거 이벤트로, 
    // public KeyCode skillKey_dash = KeyCode.LeftShift;       //



    //================================================================
    public void Init()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.actions = GameManager.Instance.playerData.inputActionSO;

        moveAction = playerInput.actions["Move"];
        mouseLeftButtonAction = playerInput.actions["MouseLeftButton"];
        interactAction = playerInput.actions["Interact"];
        drawAction = playerInput.actions["Draw"];
        utilAction = playerInput.actions["Util"];
        scrollAction = playerInput.actions["Scroll"];
        flowControlAction = playerInput.actions["FlowControl"];

        // mouseLeftButtonAction = playerInput.actions["MouseLeftButton"];

        drawingPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        flowControl = flowControlAction.triggered;




        moveVector = moveAction.ReadValue<Vector2>();

        mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (drawingPlane.Raycast(ray, out float enter))
        {
            mouseWorldPos = ray.GetPoint(enter);
        }




        isMouseLeftButtonOn = mouseLeftButtonAction.ReadValue<float>() > 0;
        interact = interactAction.ReadValue<float>() > 0;
        // secondaryInteract = Input.GetKeyDown(secondaryInteractAction);


        //
        basicAttack = mouseLeftButtonAction?.ReadValue<float>() > 0 &&
                     Player.Instance?.playerDraw?.isInDrawMode != true;
        draw = drawAction.triggered;
        scroll = scrollAction.triggered;
        util = utilAction.triggered;
        //
        // CheckNumberKeys();


        // hidden command 
        // if (Input.GetKey(KeyCode.Alpha9) && Input.GetKey(KeyCode.Alpha0))
        // {
        //     SceneLoadManager.Instance.Load_Lobby();
        // }
    }

    // 커서고정
    private void OnApplicationFocus(bool hasFocus)
    {
        // SetCursorState(hasFocus);
    }

    //=========================================================
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // private void CheckNumberKeys()
    // {
    //     if (Keyboard.current[Key.Q].wasPressedThisFrame)
    //     {
    //         pressedNumber = 1;
    //     }
    //     else if (Keyboard.current[Key.E].wasPressedThisFrame)
    //     {
    //         pressedNumber = 2;
    //     }
    //     else if (Keyboard.current[Key.LeftShift].wasPressedThisFrame)
    //     {
    //         pressedNumber = 3;
    //     }
    // }
}