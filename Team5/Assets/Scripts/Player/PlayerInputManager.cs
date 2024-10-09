using UnityEngine;
using UnityEngine.InputSystem;


 // 
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : Singleton<PlayerInputManager>
{
    [SerializeField] PlayerInput playerInput;
    
    InputAction moveAction;
    InputAction drawAction;
    InputAction lookAction;
    // InputAction jumpAction;
    // InputAction aimAction;


    //
    public Vector2 moveVector {get;private set;}
    public Vector2 mouseMoveVector {get;private set;}

    // public bool jump {get{return jumpAction.triggered;}}

    // public bool aim { get ;private set;}

    // public bool shoot {get;private set;}

    public Vector3 mouseDir {get;private set;}   // 마우스가 가리키는 방향 
    public Vector3 mouseWorldPos {get;private set;} // 마우스가 가리키는 곳의 월드 좌표 
    public float xAxis{get;private set;}        //마우스 움직임 x축
    public float yAxis {get;private set;}       // 마우스 움직임 y축
    
    
    //
    
    // [SerializeField] LayerMask aimColliderLayerMask = new();


    //================================================================

    void Start()
    {
        DontDestroyOnLoad(gameObject);  // 얜 죽으면 안됨.
        
        playerInput = GetComponent<PlayerInput>();
        
        moveAction = playerInput.actions["Move"];
        drawAction = playerInput.actions["Draw"];

        
        // lookAction = playerInput.actions["Look"];
        // jumpAction = playerInput.actions["Jump"];
        // aimAction = playerInput.actions["Aim"];
        
    }

    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        
        
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

