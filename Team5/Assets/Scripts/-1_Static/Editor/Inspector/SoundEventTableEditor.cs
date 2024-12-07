using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundEventTableSO))]
public class SoundEventTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 대상 객체 가져오기
        SoundEventTableSO soundEventTable = (SoundEventTableSO)target;

        // 버튼 추가
        if (GUILayout.Button("테이블 동기화"))
        {
            SyncTable(soundEventTable);
            RemoveDuplicateKeys(soundEventTable);
            Debug.Log("테이블 동기화");
        }
    }

    private void SyncTable(SoundEventTableSO soundEventTable)
    {
        // 모든 enum 값 가져오기
        var enumValues = System.Enum.GetValues(typeof(SoundEventType));

        // 테이블에 키 추가
        foreach (SoundEventType eventType in enumValues)
        {
            if (!soundEventTable.table.ContainsKey(eventType))
            {
                soundEventTable.table[eventType] = null;
            }
        }

        // 변경 사항 저장
        EditorUtility.SetDirty(soundEventTable);
        
    }



    private void RemoveDuplicateKeys(SoundEventTableSO soundEventTable)
    {
        // 중복 키 제거를 위한 Dictionary
        var uniqueKeys = new HashSet<SoundEventType>();
        var duplicateKeys = new List<SoundEventType>();

        // 테이블의 키 검사
        foreach (var key in soundEventTable.table.Keys)
        {
            if (!uniqueKeys.Add(key))
            {
                duplicateKeys.Add(key); // 중복 키 목록에 추가
            }
        }

        // 중복 키 제거
        foreach (var key in duplicateKeys)
        {
            soundEventTable.table.Remove(key);
            Debug.Log($"중복 키 {key} 제거");
        }

        EditorUtility.SetDirty(soundEventTable);
        
    }
}
