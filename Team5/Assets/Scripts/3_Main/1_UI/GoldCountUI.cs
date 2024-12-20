using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GoldCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_goldCount;

    //===================================================================

    void Start()
    {
        Init();
    }

    void OnDisable()
    {
        // GameEventManager.Instance.onChangePlayerGold.RemoveListener(OnChangePlayerGold);    // 종료되면서, GameEventManager가 먼저 사라질 때, 참조 오류가 발생하는 듯. 다른 이벤트 시스템을 마련하자.  
    }

    //================================================================

    void Init()
    {
        GameEventManager.Instance.onChangePlayerGold.AddListener(OnChangePlayerGold);
        
        
        text_goldCount.SetText($"{Player.Instance.status.gold}");
    }

    void OnChangePlayerGold(int amount, int gold)
    {
        text_goldCount.SetText($"{gold}");
    } 
}
