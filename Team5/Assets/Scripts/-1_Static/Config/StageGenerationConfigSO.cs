using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


/// <summary>
/// 스테이지 노드 및 스테이지 생성 관련 설정
/// </summary>
[CreateAssetMenu(fileName = "StageGenerationConfig", menuName = "SO/Config/StageGeneration", order = int.MaxValue)]
public class StageGenerationConfigSO : ScriptableObject 
{
    public List<StageNodeWeight> nodeWeights;
    
    
    //================================================================
    
    private void OnValidate()
    {
        ValidateProp();
    }
    
    
    //================================================================
    
    private void ValidateProp()
    {
        int maxProp = 100;
    
        // 총합이 100인지 확인 (약간의 오차 허용)
        for(int i=0;i<nodeWeights.Count-1;i++)
        {
            StageNodeWeight nw = nodeWeights[i];
            nw.prop = Mathf.Clamp(nw.prop, 0, maxProp);
            maxProp-= nw.prop;
        }
        nodeWeights[nodeWeights.Count-1].prop = maxProp;
    }








    //=================================================================================================
    [System.Serializable]
    public class StageNodeWeight
    {
        public StageNodeType type;
        [Range(0, 100)] public int prop; // 인스펙터 슬라이더로 범위를 제한할 수도 있음
        public bool continuousGenerationAllowed;
    }
}
