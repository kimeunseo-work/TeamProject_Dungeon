using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private Coroutine waveRoutine;

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
        //플레이어의 위치를 맵의 특정 좌표에 transform. 사용해서 고정
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


    public void GameClearMenu()
    {
        //이건테스트 서버 주석
    }
}


