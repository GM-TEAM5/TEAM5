using System.Collections;
using BW.Util;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour, ITimeScaleable
{
    [SerializeField] private float attackDelay = 0.2f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float attackRange = 2f;

    private bool isAttacking = false;
    private GameObject slash;
    private Vector3 attackDirection;
    private PlayerInputManager playerInput;

    public Player player;

    private float timeScale = 1f;

    private PlayerDraw playerDraw;




    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }

    void Start()
    {
        player = Player.Instance;
        playerInput = PlayerInputManager.Instance;
        playerDraw = player.GetComponentInChildren<PlayerDraw>();
        slash = GetComponentInChildren<Transform>().Find("Slash").gameObject;
        slash.SetActive(false);
    }

    public void OnUpdate()
    {
        if (playerDraw.isInDrawMode)
        {
            return;
        }

        attackDirection = (playerInput.mouseWorldPos - transform.position).normalized;
        attackDirection.y = 0;

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        player.animator.OnBasicAttackStart();
        TestManager.Instance.TestSFX_NormalAttack();

        yield return new WaitForSeconds(attackDelay / timeScale);

        float angle = Mathf.Atan2(attackDirection.x, attackDirection.z) * Mathf.Rad2Deg - 90f;
        Vector3 spherePosition = attackDirection * attackRange;
        spherePosition.y = 1.5f;

        slash.transform.localPosition = spherePosition - (attackDirection * 1.5f);  // 이펙트 위치를 sphere보다 1.5f 뒤로
        slash.transform.localRotation = Quaternion.Euler(0, angle, 0);
        slash.SetActive(true);

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
        slash.SetActive(false);
        isAttacking = false;

        // player.animator.OnBasicAttackFinish();
    }

    private void OnDrawGizmos()
    {
        if (slash != null && slash.activeSelf)
        {
            Gizmos.color = Color.red;
            Vector3 spherePos = transform.position + (slash.transform.localPosition + (attackDirection * 2f));
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