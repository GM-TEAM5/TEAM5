using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 플레이어가 갖고있는 '아이템' 의 UI. 마우스를 갖다대면 설명도 보여줄 예정
/// </summary>
public class PlayerItemUI : MonoBehaviour
{
    ItemDataSO data;
    [SerializeField] Image img_icon;

    public void Init(ItemDataSO data)
    {
        this.data =data;
        img_icon.sprite  = data?.sprite;    // null 일수도 있음.
    }

}
