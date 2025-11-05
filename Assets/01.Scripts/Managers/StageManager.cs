using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static StageData;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    /*필드 & 프로퍼티*/
    //=======================================// 
    public static StageManager Instance;

    [SerializeField] private Transform player;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Collider2D exitCollider;
    [SerializeField] private TilemapRenderer nextStage;
    [SerializeField] private GameObject onLoadedMonsters;
    [SerializeField] private bool testMode = true; // 테스트용 프리패스 치트키

    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, .3f);
    [SerializeField] private List<StageData> stageDatas;

    [SerializeField] private GameObject angelPrefab;
    private GameObject angel;

    private StageData currentStageData;

    private int clearRequireNum; //스테이지 클리어가 되려면 몬스터가 0이어야함
    private int stageNum;               //스테이지의 숫자

    //private bool isClear;       //클리어 확인여부의 불리언
    //private bool isStageProcessing;     //로딩중일때 입력키방지용

    /*Events*/
    public Action OnAllStageCleared;
    public Action<int> OnStageChanged;

    /*생명 주기*/
    //=======================================//
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        stageNum = 1;
        StartStage();
    }

    /*외부 호출*/
    //=======================================//

    // 스테이지 시작 매서드
    public void StartStage()
    {
        var objs = onLoadedMonsters.GetComponentsInChildren<Transform>().ToList();
        objs.RemoveAt(0);
        foreach (var obj in objs)
        {
            Destroy(obj.gameObject);
        }

        OnStageChanged?.Invoke(stageNum);

        //isClear = false;
        exitCollider.enabled = false;
        nextStage.enabled = false;
        PlacePlayerToStageStart();

        StageType stageType = GetStageType(stageNum);
        currentStageData = GetStageDataByType(stageType);

        //Debug.Log($"현재 {stageNum} 스테이지 - [{stageType}]");

        if (angel != null)
            Destroy(angel);

        switch (stageType)
        {
            case StageType.Combat:
                AudioManager.Instance.PlayNormalBGM();
                SpawnFromStageData();
                break;
            case StageType.Rest:
                AudioManager.Instance.PlayRestBGM();
                angel = Instantiate(angelPrefab);
                //Debug.Log("휴식의시간");
                stageNum++;
                //isClear = true;
                exitCollider.enabled = true;
                //천사 생성
                break;
            case StageType.Boss:
                AudioManager.Instance.PlayBossBGM();
                SpawnFromStageData();
                //Debug.Log("보스 전투 시작!");
                break;
        }

        // 스테이지 끝났을 때 호출
        player.GetComponent<Player>().FindEnemy();
    }

    public void OnMonsterKilled()
    {
        clearRequireNum--;

        //Debug.Log($"남은 몬스터 수: {clearRequireNum}");

        if (clearRequireNum <= 0
            && GameManager.Instance.CurrentState == GameManager.GameState.DungeonScene)
        {

            StageClear();
        }
    }

    public void GoToNextStage()
    {
        //stageNum++;
        if (stageNum > 10)
        {
            //GameManager.Instance.ChangeGameState(GameManager.GameState.LobbyScene);
            OnAllStageCleared?.Invoke();
            stageNum = 1;
            return;
        }

        Time.timeScale = 0f;
        StartCoroutine(LoadNextStage());
        //StartStage();
    }

    /*내부 로직*/
    //=======================================//

    private IEnumerator LoadNextStage()
    {
        yield return UIManager.Instance.FadeOut();

        StartStage();

        Time.timeScale = 1f;
        yield return UIManager.Instance.FadeIn();
    }

    private void StageClear()
    {
        AudioManager.Instance.PlayClearBGM();
        //isClear = true;
        exitCollider.enabled = true;
        nextStage.enabled = true;
        //Debug.Log("Stage Clear! Exit is now active!");
        //GoToNextStage();
        stageNum++;

        if (stageNum <= 10)
            SkillManager.Instance.RequestOpenSkillPanel("Stage Clear");
    }

    private StageType GetStageType(int stage)
    {
        if (stage % 10 == 0)
            return StageType.Boss;

        if (stage % 5 == 0)
            return StageType.Rest;

        return StageType.Combat;
    }

    private StageData GetStageDataByType(StageType type)
    {
        return stageDatas.Find(x => x.stageType == type);
    }

    private void PlacePlayerToStageStart()
    {
        if (player == null || startPoint == null)
        {
            Debug.LogError("Player 또는 StartPoint가 지정되지 않았습니다!");
            player = GameObject.FindWithTag("Player").transform;
        }

        // 위치 리셋
        player.position = startPoint.position;
        //Debug.Log("플레이어를 시작 위치에 배치했습니다!");
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

        var mon = Instantiate(prefab, pos, Quaternion.identity);
        mon.transform.parent = onLoadedMonsters.transform;
        mon.GetComponent<Monster>().Init(stageNum);
        //monsterStatuses.Add(monster.GetComponent<MonsterStatus>());
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