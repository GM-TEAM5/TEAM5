using System.Collections;
using BW.Util;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float attackDelay = 0.2f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float comboResetTime = 5f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float attackAngle = 90f;

    private PlayerInputManager playerInput;
    private bool isAttacking = false;
    private int currentCombo = 0;
    private float lastAttackTime;
    private GameObject currentSlashEffect;
    private Vector3 attackDirection;

    void Start()
    {
        playerInput = PlayerInputManager.Instance;

        currentSlashEffect = Instantiate(slashPrefab, transform.position, Quaternion.identity, transform);
        currentSlashEffect.SetActive(false);
    }

    void Update()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            currentCombo = 0;
        }

        if (playerInput.isMouseLeftButtonOn && !isAttacking)
        {
            UpdateAttackDirection();
            isAttacking = true;
            StartCoroutine(BasicAttack());
        }
    }

    void UpdateAttackDirection()
    {
        Vector3 mouseWorldPosition = playerInput.mouseWorldPos;
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.y = 0;
        attackDirection = direction.normalized;
    }

    IEnumerator BasicAttack()
    {
        yield return new WaitForSeconds(attackDelay);

        float angle = Mathf.Atan2(attackDirection.x, attackDirection.z) * Mathf.Rad2Deg;

        // 콤보에 따른 회전 설정
        Vector3 rotation;
        if (currentCombo == 0)
        {
            rotation = new Vector3(0, angle, 0);
        }
        else if (currentCombo == 1)
        {
            rotation = new Vector3(0, angle, -180);
        }
        else
        {
            rotation = new Vector3(-30, angle, -30);
        }

        // 마우스가 가리키는 방향으로 이펙트 위치 설정
        Vector3 effectPosition = attackDirection * (attackRadius - 1);
        effectPosition.y = 1.5f;
        currentSlashEffect.transform.localPosition = effectPosition;

        currentSlashEffect.transform.rotation = Quaternion.Euler(rotation);
        currentSlashEffect.SetActive(true);

        ApplyDamage();

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.5f);
        currentSlashEffect.SetActive(false);

        currentCombo = (currentCombo + 1) % 3;

        isAttacking = false;
    }

    void ApplyDamage()
    {
        Vector3 damageOrigin = transform.position + Vector3.up * 1.5f;

        Collider[] hitColliders = Physics.OverlapSphere(damageOrigin, attackRadius, GameConstants.enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - damageOrigin).normalized;
            directionToTarget.y = 0;
            float angle = Vector3.Angle(attackDirection, directionToTarget);

            if (angle <= attackAngle / 2)
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetDamaged(damage);
                }
            }
        }
    }
}