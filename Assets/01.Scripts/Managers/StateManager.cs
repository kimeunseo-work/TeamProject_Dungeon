using UnityEngine;

public class StateManager : MonoBehaviour
{
    private byte clearRequireNum = 0; //스테이지 클리어가 되려면 몬스터가 0이어야함
    private int stageNum;               //스테이지의 숫자

    private bool isClear;       //클리어 확인여부의 불리언
    private bool isStageProcessing;     //로딩중일때 입력키방지용
    private enum stageType { Combat, Rest, Boss };  //일단 만들어둔 스테이지 enum들
    private stageType currentStageType; // 현재 스테이지 타입

    public void Startstage() //게임이 시작되고 스테이지가 시작되면
    {
        //플레이어의 위치를 시작지점(
        if (currentStageType == stageType.Combat) //만약 Combat 이름의 stageType 일 경우
        {
            Debug.Log("일반전투 시작!");

            //일반 몬스터가 일정 maxWaveCount 를가지고 클리어되면 스폰되게 한다
        }
        else if (currentStageType == stageType.Boss) //만약 Boss 이름의 stageType 일 경우
        {
            Debug.Log("보스가 낙타낳다!");
            //보스 몬스터만 스폰되게 한다
        }
        else
        {
            Debug.Log("잠깐 쉬어갈 장소가 나왔다");
            //남는건 Rest Stage 로 휴식방에서 처리할 코드
        }
    }
}
