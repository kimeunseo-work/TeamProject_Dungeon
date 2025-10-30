using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsInDungeonUI : MonoBehaviour
{
    [Header("List of skills acpuired")]
    [SerializeField] GameObject skillSlotPrefab;
    [SerializeField] RectTransform content;
    [SerializeField] ScrollRect scrollRect;

    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button bgmMuteButton;
    [SerializeField] Button sfxMuteButton;
    [SerializeField] Button loadLobbyButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnClickPlay);
        bgmMuteButton.onClick.AddListener(OnClickBgmMute);
        sfxMuteButton.onClick.AddListener(OnClickSfxMute);
        loadLobbyButton.onClick.AddListener(OnClickLoadLobby);
    }

    #region button click events
    private void OnClickPlay()
    {
        Time.timeScale = 1f;
        UIManager.Instance.PopUI();
    }

    private void OnClickBgmMute()
    {

    }

    private void OnClickSfxMute()
    {

    }

    private void OnClickLoadLobby()
    {
        Time.timeScale = 1f;
        UIManager.Instance.PopUI();
        SceneManager.LoadScene(nameof(GameManger.GameState.LobbyScene));
    }
    #endregion
}
