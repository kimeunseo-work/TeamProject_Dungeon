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

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] private GameObject fixedUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (fadeImage != null)
            StartCoroutine(FadeIn());
    }

    #region UI Stack Management
    public void PushUI(GameObject ui)
    {
        // destroyed object check
        if (ui == null || ui.Equals(null)) return;

        if (ui == fixedUI)
        {
            ui.SetActive(true);
            return;
        }

        // clear stack
        while (uiStack.Count > 0 && (uiStack.Peek() == null || uiStack.Peek().Equals(null)))
            uiStack.Pop();

        if (uiStack.Count > 0 && uiStack.Peek() != null)
            uiStack.Peek().SetActive(false);

        ui.SetActive(true);
        uiStack.Push(ui);
    }

    public void PopUI()
    {
        if (uiStack == null || uiStack.Count == 0)
            return;

        // clear null ui
        while (uiStack.Count > 0 && (uiStack.Peek() == null || uiStack.Peek().Equals(null)))
            uiStack.Pop();

        if (uiStack.Count == 0)
            return;

        GameObject topUI = uiStack.Pop();
        if (topUI != null)
            topUI.SetActive(false);

        if (uiStack.Count > 0 && uiStack.Peek() != null)
            uiStack.Peek().SetActive(true);
    }

    public void ClearStack()
    {
        while (uiStack.Count > 0)
        {
            var ui = uiStack.Pop();
            if (ui != null) ui.SetActive(false);
        }
    }
    #endregion

    #region Scene Handling
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(HandleSceneLoaded(scene));
    }

    private IEnumerator HandleSceneLoaded(Scene scene)
    {
        yield return null;

        ClearStack();
        fixedUI = null;

        switch (scene.name)
        {
            case nameof(GameManager.GameState.LobbyScene):
                var lobbyUI = FindObjectOfType<LobbyUI>(true);
                if (lobbyUI != null)
                {
                    fixedUI = lobbyUI.gameObject;
                    fixedUI.SetActive(true);
                }
                break;
            case nameof(GameManager.GameState.DungeonScene):
                var stageUI = FindObjectOfType<StageUI>(true);
                if (stageUI != null)
                {
                    fixedUI = stageUI.gameObject;
                    fixedUI.SetActive(true);
                }
                break;
        }
    }
    #endregion

    #region Fade
    public IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        yield return Fade(0f);
        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        yield return Fade(1f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;

        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
    #endregion

    #region Slider Animation
    public void AnimateSlider(Slider slider, float targetValue, float duration = 0.4f)
    {
        if (slider == null) return;

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

        if (slider != null)
            slider.value = 0f;
    }
    #endregion
}
