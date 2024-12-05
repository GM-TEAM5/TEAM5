using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [SerializeField] Transform inkBarContainer;
    [SerializeField] Slider inkBarPrefab;

    private List<Slider> inkSegmentBars = new List<Slider>();
    private int inkSegments;
    private float segmentValue;



    //==========================================
    public void Init(Player player)
    {
        UpdateMaxHp(player.status.maxHp);
        UpdateCurrHp(player.status.currHp);

        inkSegments = player.status.totalInkSegments;
        segmentValue = player.status.maxInk / inkSegments;

        // 세그먼트 슬라이더 생성
        for (int i = 0; i < inkSegments; i++)
        {
            Slider newSegment = Instantiate(inkBarPrefab, inkBarContainer);
            inkSegmentBars.Add(newSegment);
        }

        UpdateMaxInk(player.status.maxInk);
        UpdateCurrInk(player.status.currInk);

        GameEventManager.Instance.onChangePlayerStatus_maxHp.AddListener(() => UpdateMaxHp(player.status.maxHp));
    }

    #region ====== HP =======

    /// <summary>
    /// hp bar 의 최댓값을 플레이어 능력치 값에 맞춘다. (주로 체력 증가 이벤트 발생시 호출됨 )
    /// </summary>
    public void UpdateMaxHp(float maxHp)
    {
        hpBar.maxValue = maxHp;
    }

    /// <summary>
    /// hp bar의 현재 값을 플레이어 능력치 값에 맞춘다. (주로 회복, 피해 발생시 호출됨)
    /// </summary>
    public void UpdateCurrHp(float hp)
    {
        hpBar.value = hp;
    }

    #endregion

    #region ====== Ink =======

    /// <summary>
    /// ink bar 의 최댓값을 플레이어 능력치 값에 맞춘다.
    /// </summary>
    public void UpdateMaxInk(float maxInk)
    {
        segmentValue = maxInk / inkSegments;
    }

    /// <summary>
    /// ink bar의 현재 값을 맞춘다. (붓칠 시 호출됨)
    /// </summary>
    public void UpdateCurrInk(float currInk)
    {
        var (fullSegments, partialSegment) = Player.Instance.status.GetInkSegmentInfo();

        for (int i = 0; i < inkSegments; i++)
        {
            if (i < fullSegments)
            {
                inkSegmentBars[i].value = 1f;
            }
            else if (i == fullSegments)
            {
                inkSegmentBars[i].value = partialSegment;
            }
            else
            {
                inkSegmentBars[i].value = 0f;
            }
        }
    }
    #endregion
}
