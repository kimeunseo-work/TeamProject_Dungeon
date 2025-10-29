using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
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

    public void SetMonster()
    { 
        //stageNum을 확인하고 currentStageType 값을 결정하게하고
        //currentStageType 을 확인하고
        //currentStageType 이 Combat 이거나 Boss일경우
        //isClear를 false로 두고
        //clearRequre
    }

    public void GameClearMenu()
    { 
        //
    }
}
