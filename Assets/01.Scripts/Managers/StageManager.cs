using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    [SerializeField] private Transform player;      
    [SerializeField] private Transform startPoint;

    [SerializeField] private List<GameObject> enemyprefabs;

    [SerializeField] List<Rect> spawnAreas;

    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, .3f);

    private bool enemySpawnComplite;



    private byte clearRequireNum = 0; //스테이지 클리어가 되려면 몬스터가 0이어야함
    private int stageNum;               //스테이지의 숫자

    private bool isClear;       //클리어 확인여부의 불리언
    private bool isStageProcessing;     //로딩중일때 입력키방지용
    private enum stageType { Combat, Rest, Boss };  //일단 만들어둔 스테이지 enum들
    private stageType currentStageType; // 현재 스테이지 타입

    public void StartStage()
    { 
        PlacePlayerToStageStart();
    }

    public void GameClear()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void GameOver()
    { 

    }

    private void SpawnRandomEnemy()
    {
        if (enemyprefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Enemy Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        GameObject randomPrefab = enemyprefabs[Random.Range(0, enemyprefabs.Count)];


        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];


        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax)
        );


        Instantiate(randomPrefab, randomPosition, Quaternion.identity);
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomEnemy();
        }
    }

    public void GameClearMenu()
    {
        //이건테스트 서버 주석
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
}


