using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class UnderWorldPlayer : Singleton<UnderWorldPlayer>
{
  // [SerializeField] Sprite playerSprite;

    public Transform t_player;

    public PlayerStatus status;     // 플레이어의 능력치 정보 
    SpriteEntity spriteEntity;

    //======= ui ========
    //
    PlayerInputManager playerInput;
    CharacterController controller;
    [SerializeField] Collider playerCollider;

    //-----------------
    [SerializeField] Vector3 lastMoveDir;

    public bool isAlive => status.hp >0;


    //====================================================================================

    private void Start()
    {
        InitPlayer();
    }

    void Update()
    {
        if (isAlive==false || UnderWorldManager.isGamePlaying == false )
        {
            return;
        }

        Move();
        UpdateSpriteDir();
    }

    //============================================================================
    void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("DropItem"))
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
        //
        playerCollider = GetComponent<Collider>();
        playerCollider.enabled = true;
        //
        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(controller.radius, controller.height);
    }

    //========================================================================

    /// <summary>
    /// 움직임
    /// </summary>
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


 
}
