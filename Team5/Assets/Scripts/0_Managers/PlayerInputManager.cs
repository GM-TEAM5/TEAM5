using UnityEngine;
using UnityEngine.InputSystem;


 // 
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : Singleton<PlayerInputManager>
{
    [SerializeField] PlayerInput playerInput;
    
    public InputAction moveAction;
    public InputAction mouseLeftButtonAction;
    InputAction lookAction;


    public bool isMouseLeftButtonOn;        //마우스를 누르고 있는 중인지.



    //
    public Vector2 moveVector {get;private set;}
    public Vector2 mouseMoveVector {get;private set;}



    public Vector3 mouseDir {get;private set;}   // 마우스가 가리키는 방향 
    public Vector3 mouseWorldPos {get;private set;} // 마우스가 가리키는 곳의 월드 좌표 
    public float xAxis{get;private set;}        //마우스 움직임 x축
    public float yAxis {get;private set;}       // 마우스 움직임 y축

    // TODO: 그리기 범위 수정
    private Plane drawingPlane;

    public int pressedNumber { get; private set; } = 0; // 숫자 키 입력
    //

    // [SerializeField] LayerMask aimColliderLayerMask = new();


    //================================================================

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        
        moveAction = playerInput.actions["Move"];
        mouseLeftButtonAction = playerInput.actions["MouseLeftButton"];

        drawingPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (drawingPlane.Raycast(ray, out float enter))
        {
            mouseWorldPos = ray.GetPoint(enter);
        }


        isMouseLeftButtonOn = mouseLeftButtonAction.ReadValue<float>() > 0;

        CheckNumberKeys();
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
        if (Keyboard.current[Key.Digit1].wasPressedThisFrame) pressedNumber = 0;
        else if (Keyboard.current[Key.Digit2].wasPressedThisFrame) pressedNumber = 1;
        else if (Keyboard.current[Key.Digit3].wasPressedThisFrame) pressedNumber = 2;
        else if (Keyboard.current[Key.Digit4].wasPressedThisFrame) pressedNumber = 3;
        else if (Keyboard.current[Key.Digit5].wasPressedThisFrame) pressedNumber = 4;
    }
}

