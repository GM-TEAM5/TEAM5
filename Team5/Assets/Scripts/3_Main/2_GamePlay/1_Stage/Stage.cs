using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Stage : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] Transform t_playerSpawnPoint;
    
    public Vector3 playerInitPos => t_playerSpawnPoint.position;
    public Vector3 portalPos => t_portalPos.position; 
    [SerializeField] Transform t_portalPos;
    [SerializeField] Transform t_enemySpawnAreaParent;
    [SerializeField] BoxCollider[] enemySpawnArea;
    [SerializeField] BoxCollider[] boundary;

    // public WaveActivationSwitch waveActivationSwitch;

    public StagePortal stagePortal;
    // public SelectableItemList selectableItemList;
    Vector3 offset;


    //
    public void Init(StageNode nodeData)
    {
        enemySpawnArea = t_enemySpawnAreaParent.GetComponentsInChildren<BoxCollider>();
        InitByMapInfo(nodeData.formInfo);
    }


    void InitByMapInfo(StageFormInfo formInfo)
    {
        float width = formInfo.width;
        float height = formInfo.height;

        navMeshSurface.size = new Vector3(width, navMeshSurface.size.y, height);
        offset = navMeshSurface.transform.position;
        // BoxCollider 크기 설정 (로컬 좌표 기준)
        foreach(BoxCollider collider in enemySpawnArea)
        {
            collider.size = new Vector3(width, collider.size.y, height);
            collider.transform.position = offset +  new Vector3(navMeshSurface.center.x, -2, navMeshSurface.center.z) ;
        }     

        navMeshSurface.BuildNavMesh();

        t_playerSpawnPoint.position = offset; 

        DrawBoundary();
        AdjustBoundaryCollider();
    
    }



    private void DrawBoundary()
    {
        // LineRenderer 설정
        lineRenderer.positionCount = 5; // 사각형 경계를 그리므로 5개 점
        lineRenderer.loop = false;      // 닫히지 않은 선으로 처리
        lineRenderer.startWidth = 0.2f; // 선의 두께
        lineRenderer.endWidth = 0.2f;
        lineRenderer.useWorldSpace = false; 
        
        // NavMeshSurface 경계 가져오기
        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        
        // Bounds의 각 꼭짓점 계산
        Vector3[] corners = new Vector3[5]
        {
            offset+new Vector3(bounds.min.x, transform.position.y + float.Epsilon, bounds.min.z), // Bottom-left
            offset+new Vector3(bounds.max.x, transform.position.y + float.Epsilon, bounds.min.z), // Bottom-right
            offset+new Vector3(bounds.max.x, transform.position.y + float.Epsilon, bounds.max.z), // Top-right
            offset+new Vector3(bounds.min.x, transform.position.y + float.Epsilon, bounds.max.z), // Top-left
            offset+new Vector3(bounds.min.x, transform.position.y + float.Epsilon, bounds.min.z)  // Close the loop
        };

        // LineRenderer에 점 설정
        lineRenderer.SetPositions(corners);
    }
    void AdjustBoundaryCollider()
    {
        // NavMesh 경계 가져오기
        float thickness  = 3;
        float height = 10;
        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        
        // 각 경계에 큐브 배치
        CreateBoundaryCube("Top",       boundary[0],new Vector3(0, 0, bounds.max.z), new Vector3(bounds.size.x, height, thickness  ));
        CreateBoundaryCube("Bottom",    boundary[1],new Vector3(0, 0, bounds.min.z), new Vector3(bounds.size.x, height, thickness  ));
        CreateBoundaryCube("Left",      boundary[2],new Vector3(bounds.min.x, 0, 0), new Vector3(thickness  , height, bounds.size.z));
        CreateBoundaryCube("Right",     boundary[3],new Vector3(bounds.max.x, 0, 0), new Vector3(thickness  , height, bounds.size.z));
    }

    void CreateBoundaryCube(string name, BoxCollider collider, Vector3 position, Vector3 scale)
    {
        // 위치 및 스케일 설정
        collider.transform.position = offset + position;
        collider.size = scale;

     
    }



    

    /// <summary>
    /// 해당 영역에서 임의의 좌표를 얻는다. 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomSpawnPoint()
    {
        Vector3 ret = Player.Instance.t.position;

        if (enemySpawnArea.Length>0)
        {
            int randIdx = Random.Range(0,enemySpawnArea.Length);
            BoxCollider area = enemySpawnArea[randIdx];

            Bounds bounds = area.bounds;

            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            ret = new Vector3(randomX, 0, randomZ);
        }

        return ret;
    }

}
