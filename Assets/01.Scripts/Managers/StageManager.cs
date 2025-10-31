using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StageData;

public class StageManager : MonoBehaviour
{

    [SerializeField] private Transform player;      
    [SerializeField] private Transform startPoint;


    [SerializeField] List<Rect> spawnAreas;

    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, .3f);

    private bool enemySpawnComplite;

    [SerializeField] private List<StageData> stageDatas;
    private StageData currentStageData;


    private int clearRequireNum; //스테이지 클리어가 되려면 몬스터가 0이어야함
    private int stageNum;               //스테이지의 숫자

    private bool isClear;       //클리어 확인여부의 불리언
    private bool isStageProcessing;     //로딩중일때 입력키방지용

    private void Start()
    {
        stageNum = 1;
        StartStage();
    }

    // 스테이지 시작 매서드
    public void StartStage()
    {
        PlacePlayerToStageStart();

        currentStageData = stageDatas.Find(x => x.stageNum == stageNum);

        if (currentStageData == null)
        {
            Debug.LogError($"StageData 없음! StageNum: {stageNum}");
            return;
        }

        if (currentStageData.stageType == StageType.Combat)
        {
            SpawnFromStageData();
        }
        if (currentStageData.stageType == StageType.Boss)
        { 
            
        }


    }

    public void OnMonsterKilled()
    {
        clearRequireNum--;

        Debug.Log($"남은 몬스터 수: {clearRequireNum}");

        if (clearRequireNum <= 0)
        {
            StageClear();
        }
    }

    public void StageClear()
    {
        isClear = true;

        //Exit 활성화


    }





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomEnemyFromData();
        }
    }

    private void PlacePlayerToStageStart()
    {
        if (player == null || startPoint == null)
        {
            Debug.LogError("Player 또는 StartPoint가 지정되지 않았습니다!");
            return;
        }

        // 위치 리셋
        player.position = startPoint.position;
        Debug.Log("플레이어를 시작 위치에 배치했습니다!");
    }

    // 스폰 관련 매서드
    private void SpawnRandomEnemyFromData()
    {
        GameObject prefab = currentStageData.monsterPrefabs[
            Random.Range(0, currentStageData.monsterPrefabs.Count)
        ];

        Rect area = spawnAreas[Random.Range(0, spawnAreas.Count)];
        Vector2 pos = new Vector2(
            Random.Range(area.xMin, area.xMax),
            Random.Range(area.yMin, area.yMax)
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }

    private void SpawnFromStageData()
    {
        if (currentStageData == null)
        {
            Debug.LogError("StageData가 설정되지 않았습니다!");
            return;
        }

        if (currentStageData.monsterPrefabs.Count == 0)
        {
            Debug.LogWarning("몬스터 프리팹이 StageData에 없습니다!");
            return;
        }

        int count = Random.Range(
            currentStageData.minMonsterCount,
            currentStageData.maxMonsterCount + 1
        );

        for (int i = 0; i < count; i++)
            SpawnRandomEnemyFromData();

        clearRequireNum = count;
    }

    // 기즈모 코드
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);
            Gizmos.DrawCube(center, size);
        }
    }
}


// 아직까진 미구현 된 매서드들
//public void GameOver()
//{

//}

//public void GameClearMenu()
//{
//    //이건테스트 서버 주석
//}