using System.Collections;
using BW.Util;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public BasicAttackSO data; 


    private Vector3 attackDirection;
    float attackRange = 2f;

    float lastAttackTime = -1;
    int currCombo;

    

    GameObject[] effects;
    

    //==============================================================================
    // 평타 아이템 획득시, 장착
    public void Equip(BasicAttackSO basicAttackData)
    {
        data = basicAttackData;
        
        effects = new GameObject[data.comboCount];
        for(int i=0;i<data.comboCount;i++)
        {
            GameObject effect = Instantiate(data.effects[i],transform);
            effect.SetActive(false);

            effects[i] = effect;
        }

    }

    // 장착중인 아이템 해제 (보통 교체될 때 호출됨. )
    public void UnEquip()
    {
        // 플레이어 계층 구조 하위에 생성되었던 이펙트 파괴
        for(int i=0;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void TryAttack()
    {
        //
        if (Time.time >= lastAttackTime )   //0.1, 0.1, 0.2 
        {
            // 콤보 리셋 체크
            if (Time.time >= lastAttackTime + data.comboResetTime)
            {
                currCombo = 0;
            }

            
            attackDirection = (PlayerInputManager.Instance.mouseWorldPos - Player.Instance.t.position).normalized;
            attackDirection.y = 0;    

            StartCoroutine(Attack());
        }
    }



    IEnumerator Attack()
    {
        Player.Instance.animator.OnBasicAttackStart();      // 이건 이제 애니메이션 클립으로 재생할거임. 
        TestManager.Instance.TestSFX_NormalAttack();

        //
        lastAttackTime = Time.time + data.delays[currCombo];
        

        // 현재 이펙트만 활성화
        GameObject currEffect = effects[currCombo];
        currEffect.SetActive(true);


        currCombo = (currCombo + 1) % data.comboCount;


        // 공격 방향 계산
        float angle = Mathf.Atan2(attackDirection.x, attackDirection.z) * Mathf.Rad2Deg - 90f;
        Vector3 spherePosition = (attackDirection * attackRange).WithStandardHeight();  //평타 종류별로 2f 는 달라질 수 잇음. 

        // Slash 부모 오브젝트의 위치와 회전 설정
        transform.localPosition = spherePosition - (attackDirection * 1.5f);
        transform.localRotation = Quaternion.Euler(0, angle, 0);


        // 데미지 판정 - 
        Collider[] hits = Physics.OverlapSphere(transform.position + spherePosition, 1f);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                float dmg = data.defaultDamage + data.damageWeight * Player.Instance.status.pDmg;
                enemy.GetDamaged(dmg );
            }
        }


        

        yield return new WaitForSeconds(0.5f);      // 이부분은 추후 이펙트 프리팹 스크립트로 빼자. 
        currEffect.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && currCombo < effects.Length &&
            effects[currCombo] != null && effects[currCombo].activeSelf)
        {
            Gizmos.color = Color.red;
            Vector3 spherePos = transform.position + (effects[currCombo].transform.localPosition + (attackDirection * 2f));
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