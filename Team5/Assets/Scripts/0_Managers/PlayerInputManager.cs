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
    public Vector3 mousePosition {get;private set;}
    public Vector3 mouseWorldPos {get;private set;} // 마우스가 가리키는 곳의 월드 좌표 
    public float xAxis{get;private set;}        //마우스 움직임 x축
    public float yAxis {get;private set;}       // 마우스 움직임 y축

    // TODO: 그리기 범위 수정
    private Plane drawingPlane;

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

        mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (drawingPlane.Raycast(ray, out float enter))
        {
            mouseWorldPos = ray.GetPoint(enter);
        }
        

        isMouseLeftButtonOn = mouseLeftButtonAction.ReadValue<float>()>0;




        
        // mouseMoveVector = lookAction.ReadValue<Vector2>();
        // aim = aimAction.ReadValue<float>()>0;
        // shoot = drawAction.ReadValue<float>()>0;

        //




        // Transform t_hit = null; // 히트스캔에 필요.
        // Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width *0.5f, Screen.height * 0.5f) );  //조준점 위치(화면중앙));
        // 조준점 방향 계산
        // mouseDir = ray.GetPoint(50);
        // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))    // 마우스가 가리키는 곳에 뭔가 있다면
        // {
        //     mouseWorldPos =  raycastHit.point;
        //     t_hit = raycastHit.transform;
        // }
        // else    // 마우스가 가리키는 곳에 아무것도 없으면, 
        // {
        //     mouseWorldPos = mouseDir; // 적절한 거리로 설정
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
}

