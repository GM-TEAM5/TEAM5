using System.Collections;
using BW.Util;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public BasicAttackSO data; 


    private Vector3 attackDir;

    float attackAvailableTime = -1;
    float comboResetTime = -1;
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
        if (Time.time >= attackAvailableTime )  
        {
            // 콤보 리셋 체크
            if (Time.time >= comboResetTime )
            {
                currCombo = 0;
            }

            
            CalAttackDir();

            StartCoroutine(Attack());
        }
    }


    IEnumerator Attack()
    {
        Player.Instance.animator.OnBasicAttackStart();      // 이건 이제 애니메이션 클립으로 재생할거임. 
        TestManager.Instance.TestSFX_NormalAttack();

        //
        attackAvailableTime = Time.time + data.delays[currCombo];
        comboResetTime = Time.time + data.comboResetTime;
        
        

        // 현재 이펙트만 활성화
        GameObject currEffect = effects[currCombo];
        currEffect.SetActive(true);
                
        // transform.localPosition = spherePosition - (attackDir * 1.5f);      // 이펙트 설정
        float angle = Mathf.Atan2(attackDir.x, attackDir.z) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.Euler(0, angle, 0);            //이펙트 설정


        // 데미지 판정 - 
        data.detections[currCombo].Detect(attackDir,transform.position,data);
    

        currCombo = (currCombo + 1) % data.comboCount;

        yield return new WaitForSeconds(1f);      // 이부분은 추후 이펙트 프리팹 스크립트로 빼자. 
        currEffect.SetActive(false);
    }


    void CalAttackDir()
    {
        attackDir = (PlayerInputManager.Instance.mouseWorldPos - Player.Instance.t.position).normalized;
        attackDir.y = 0;    
    }

    //===============================================================================

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && currCombo < effects.Length &&
            effects[currCombo] != null && effects[currCombo].activeSelf)
        {
            Gizmos.color = Color.red;
            Vector3 spherePos = transform.position + (effects[currCombo].transform.localPosition + (attackDir * 2f));
            Gizmos.DrawWireSphere(spherePos, 1f);
            Gizmos.DrawLine(transform.position, spherePos);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
#endif
}