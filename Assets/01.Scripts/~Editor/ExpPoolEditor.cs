using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExpPool))]
public class ExpPoolEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 표시
        DrawDefaultInspector();

        ExpPool pool = (ExpPool)target;

        // 실행 버튼
        if (GUILayout.Button("Get() 실행"))
        {
            // 런타임 중일 때만 작동하도록 제한
            if (Application.isPlaying)
            {
                GameObject obj = pool.Get();
                Debug.Log($"[ExpPoolEditor] 오브젝트 생성됨: {obj.name}");
            }
            else
            {
                Debug.LogWarning("Play 모드에서만 실행할 수 있습니다.");
            }
        }
    }
}
