using System.Collections;
using BW.Util;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour, ITimeScaleable
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDelay = 0.2f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float comboCooldown = 3f;
    [SerializeField] private float[] comboDelays = new float[] { 0.1f, 0.2f, 0.3f };

    private bool isAttacking = false;
    private Vector3 attackDirection;
    private PlayerInputManager playerInput;
    private int comboCount = 0;
    private float lastAttackTime;
    private GameObject[] slashEffects;
    private Transform slashParent;  // Slash 오브젝트의 Transform

    private float timeScale = 1f;
    private PlayerDraw playerDraw;
    private Player player;

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }

    void Start()
    {
        player = Player.Instance;
        playerInput = PlayerInputManager.Instance;
        playerDraw = player.GetComponentInChildren<PlayerDraw>();

        // Slash 오브젝트와 이펙트들 자동으로 찾기
        slashParent = transform.Find("Slash");
        if (slashParent == null)
        {
            Debug.LogError("Slash object not found!");
            return;
        }

        // 자식 이펙트들 가져오기
        slashEffects = new GameObject[slashParent.childCount];
        for (int i = 0; i < slashParent.childCount; i++)
        {
            slashEffects[i] = slashParent.GetChild(i).gameObject;
            slashEffects[i].SetActive(false);
        }
    }

    public void OnUpdate()
    {
        if (playerDraw.isInDrawMode) return;

        attackDirection = (playerInput.mouseWorldPos - transform.position).normalized;
        attackDirection.y = 0;

        // 콤보 리셋 체크
        if (Time.time - lastAttackTime > comboCooldown)
        {
            comboCount = 0;
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        if (slashEffects[comboCount] == null)
        {
            Debug.LogError($"Slash effect {comboCount} is not assigned!");
            yield break;
        }

        isAttacking = true;
        lastAttackTime = Time.time;

        GameObject currentSlash = slashEffects[comboCount];
        float currentDelay = comboDelays[comboCount];

        player.animator.OnBasicAttackStart();
        TestManager.Instance.TestSFX_NormalAttack();

        yield return new WaitForSeconds(currentDelay / timeScale);

        // 공격 방향 계산
        float angle = Mathf.Atan2(attackDirection.x, attackDirection.z) * Mathf.Rad2Deg - 90f;
        Vector3 spherePosition = attackDirection * attackRange;
        spherePosition.y = 1f;

        // Slash 부모 오브젝트의 위치와 회전 설정
        slashParent.localPosition = spherePosition - (attackDirection * 1.5f);
        slashParent.localRotation = Quaternion.Euler(0, angle, 0);

        // 현재 이펙트만 활성화
        currentSlash.SetActive(true);

        // 데미지 판정
        Collider[] hits = Physics.OverlapSphere(transform.position + spherePosition, 1f);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamaged(damage);
            }
        }

        yield return new WaitForSeconds(0.5f / timeScale);
        currentSlash.SetActive(false);

        comboCount = (comboCount + 1) % 3;
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && comboCount < slashEffects.Length &&
            slashEffects[comboCount] != null && slashEffects[comboCount].activeSelf)
        {
            Gizmos.color = Color.red;
            Vector3 spherePos = transform.position + (slashEffects[comboCount].transform.localPosition + (attackDirection * 2f));
            Gizmos.DrawWireSphere(spherePos, 1f);
            Gizmos.DrawLine(transform.position, spherePos);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}