using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public static UIManager Instance;

    private Stack<GameObject> uiStack = new Stack<GameObject>();

    private Coroutine sliderCoroutine;

    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
    }

    public void PushUI(GameObject ui)
    {
        if (uiStack.Count > 0)
            uiStack.Peek().SetActive(false);

        ui.SetActive(true);
        uiStack.Push(ui);
    }

    public void PopUI()
    {
        if (uiStack.Count == 0)
        {
            Debug.Log("uiStack is empty");
            return;
        }

        GameObject topUI = uiStack.Pop();
        topUI.SetActive(false);

        if (uiStack.Count > 0)
            uiStack.Peek().SetActive(true);
    }

    public void ClearStack()
    {
        while (uiStack.Count > 0)
            uiStack.Pop().SetActive(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case nameof(GameManger.GameState.LobbyScene):
                Debug.Log("load lobby scene");
                LobbyUI lobbyUI = FindObjectOfType<LobbyUI>();

                if (lobbyUI != null)
                    uiStack.Push(lobbyUI.gameObject);

                break;
            case nameof(GameManger.GameState.DungeonScene):
                Debug.Log("load dungeon scene");
                StageUI stageUI = FindObjectOfType<StageUI>();

                if (stageUI != null)
                {
                    Debug.Log("push stageUI");
                    uiStack.Push(stageUI.gameObject);
                }

                break;
        }
    }

    #region Effect
    public void AnimateSlider(Slider slider, float targetValue, float duration = 0.4f)
    {
        if (sliderCoroutine != null)
            StopCoroutine(sliderCoroutine);

        sliderCoroutine = StartCoroutine(SliderCoroutine(slider, targetValue, duration));
    }

    private IEnumerator SliderCoroutine(Slider slider, float targetValue, float duration)
    {
        float startValue = slider.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            slider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }

        slider.value = targetValue;
        sliderCoroutine = null;
    }

    public void ResetSlider(Slider slider)
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null;
        }

        slider.value = 0f;
    }

    public IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        yield return Fade(0);
        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        yield return Fade(1);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null)
        {
            Debug.Log("fade image is null");
            yield break;
        }

        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration) 
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time);
            fadeImage.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
    #endregion
}
