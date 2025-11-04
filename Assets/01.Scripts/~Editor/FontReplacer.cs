#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class FontReplacer
{
    [MenuItem("Tools/Replace Fonts In Current Scene")]
    public static void ReplaceFontsInScene()
    {
        Font uiFont = Selection.activeObject as Font;
        TMP_FontAsset tmpFont = Selection.activeObject as TMP_FontAsset;

        if (uiFont == null && tmpFont == null)
        {
            EditorUtility.DisplayDialog("Font Replacer",
                "먼저 Project 창에서 교체할 Font 또는 TMP_FontAsset을 선택하세요.", "확인");
            return;
        }

        int count = 0;

        // 일반 Unity UI Text 교체
        foreach (var text in Object.FindObjectsOfType<Text>(true))
        {
            if (uiFont != null)
            {
                Undo.RecordObject(text, "Replace Font");
                text.font = uiFont;
                EditorUtility.SetDirty(text);
                count++;
            }
        }

        // TextMeshProUGUI 교체
        foreach (var tmp in Object.FindObjectsOfType<TextMeshProUGUI>(true))
        {
            if (tmpFont != null)
            {
                Undo.RecordObject(tmp, "Replace TMP Font");
                tmp.font = tmpFont;
                EditorUtility.SetDirty(tmp);
                count++;
            }
        }

        Debug.Log($"{count}개의 텍스트 폰트가 교체되었습니다.");
    }
}
#endif
