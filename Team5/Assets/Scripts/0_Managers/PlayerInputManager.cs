using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// 
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : Singleton<PlayerInputManager>
{
    [SerializeField] PlayerInput playerInput;

    public InputAction moveAction;
    public InputAction mouseLeftButtonAction;
    public KeyCode interactAction = KeyCode.F;
    public KeyCode secondaryInteractAction = KeyCode.R;

    public KeyCode pauseAction = KeyCode.Escape;
    InputAction lookAction;


    public bool interact;
    public bool secondaryInteract;
    public bool pause;
    public bool isMouseLeftButtonOn;        //마우스를 누르고 있는 중인지.



    //
    public Vector2 moveVector { get; private set; }
    public Vector2 mouseMoveVector { get; private set; }



    public Vector3 mouseDir { get; private set; }   // 마우스가 가리키는 방향 
    public Vector3 mousePosition { get; private set; }
    public Vector3 mouseWorldPos { get; private set; } // 마우스가 가리키는 곳의 월드 좌표 
    public float xAxis { get; private set; }        //마우스 움직임 x축
    public float yAxis { get; private set; }       // 마우스 움직임 y축

    // TODO: 그리기 범위 수정
    private Plane drawingPlane;

    public int pressedNumber { get; set; } = 0; // 숫자 키 입력
    //

    // [SerializeField] LayerMask aimColliderLayerMask = new();
    public List<KeyCode> skillKeys = new() { KeyCode.Q, KeyCode.E, KeyCode.LeftShift, KeyCode.Alpha4 };


    //================================================================

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        mouseLeftButtonAction = playerInput.actions["MouseLeftButton"];
        // mouseLeftButtonAction = playerInput.actions["MouseLeftButton"];

        drawingPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();

        mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (drawingPlane.Raycast(ray, out float enter))
        {
            mouseWorldPos = ray.GetPoint(enter);
        }


        isMouseLeftButtonOn = mouseLeftButtonAction.ReadValue<float>() > 0;
        interact = Input.GetKeyDown(interactAction);
        secondaryInteract = Input.GetKeyDown(secondaryInteractAction);
        pause = Input.GetKeyDown(pauseAction);

        CheckNumberKeys();


        // hidden command 
        if (Input.GetKey(KeyCode.Alpha9) && Input.GetKey(KeyCode.Alpha0))
        {
            SceneLoadManager.Instance.Load_Lobby();
        }
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

    private void CheckNumberKeys()
    {
        if (Keyboard.current[Key.Q].wasPressedThisFrame) pressedNumber = 1;
        else if (Keyboard.current[Key.E].wasPressedThisFrame) pressedNumber = 2;
        else if (Keyboard.current[Key.LeftShift].wasPressedThisFrame) pressedNumber = 3;
        else if (Keyboard.current[Key.Digit4].wasPressedThisFrame) pressedNumber = 4;
        else if (Keyboard.current[Key.Digit5].wasPressedThisFrame) pressedNumber = 5;
    }
}

