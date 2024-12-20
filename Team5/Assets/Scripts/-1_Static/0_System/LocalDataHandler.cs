using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// public enum LocalDataType
// {
//     TotalNodeData,
// }

public class LocalDataHandler
{
    static string path_dataRoot = "99_SaveData";


    //==========================
    // 저장된 캐시를 불러와 T타입의 데이터로 변환한다. - 나중에는 csv파일이나 json으로 옮길예정
    //=========================
    public static T LoadJson<T>(string path)
    {
        T ret = default;

        CheckDataPath(path);
        string data = File.ReadAllText(path);  
        try
        {
            if (!string.IsNullOrEmpty(data))
            {
                ret = JsonUtility.FromJson<T>(data);
                Debug.LogWarning($"{path}로부터 데이터 읽음");
                //Debug.LogError($" 길이:{data.Length }, 내용:{data}");

            }
            else
            {
                Debug.LogError($"{path}에 데이터가 없음;; ");
            }
        }
        catch 
        {
            // Debug.LogError("json 파일 손상되었음 : "+path);
        }
        return ret;


    }

    //===========================
    // content를 json string으로 변환하여 path에 저장한다. 
    //==========================
    public static void SaveJson(string path, string json)
    {

        CheckDataPath(path);
        try
        {
            File.WriteAllText(path, json);

            Debug.Log($" 파일 저장 완료 - {path}");
            // Debug.LogWarning($"{json}");
        }
        catch
        {
            Debug.LogError("해당 경로에 파일 저장 에러 " + path);
        }

    }


    public static void CheckDataPath(string path)
    { 
        // 디렉토리 경로를 추출
        string directory = Path.GetDirectoryName(path);

        // 디렉토리가 없으면 생성
        if (Directory.Exists(directory)==false)
        {
            Directory.CreateDirectory(directory);
        }
        
        
        //
        if (File.Exists(path)== false)
        {
            //Debug.LogError(" 파일 찾기 오류 : " + path + " 파일이 없습니다!!!");

            File.Create(path).Dispose();
        }
        else
        {
            //Debug.LogWarning(" 파일 찾기 : " + path + " 파일을 찾았습니다. ");
        }
                  
    }

    //=====================================================================================
#region StageNode

    public static List<StageNode> LoadTotalNodeData()
    {
        string filePath = $"{Application.persistentDataPath}/{path_dataRoot}/TotalNodeData.json" ;

        JsonListWrapper<StageNode> stageNodes = LoadJson<JsonListWrapper<StageNode>>(filePath);

        return stageNodes.data;
    }
    
    
    public static void SaveTotalNodeData(List<StageNode> stageNodes)
    {
        string filePath = $"{Application.persistentDataPath}/{path_dataRoot}/TotalNodeData.json" ;

        string json = JsonUtility.ToJson(new JsonListWrapper<StageNode>(stageNodes), true);
        SaveJson(filePath, json);
    }

#endregion
}
