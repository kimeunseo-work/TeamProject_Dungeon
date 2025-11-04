using UnityEngine;
using UnityEngine.UI;

public class SettingsInDungeonUI : MonoBehaviour
{
    [Header("List of skills acpuired")]
    [SerializeField] RectTransform content;
    [SerializeField] ScrollRect scrollRect;

    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button muteButton;
    [SerializeField] Button loadLobbyButton;

    [Header("MuteImages")]
    [SerializeField] private Image muteImage;
    [SerializeField] private Sprite unMuteSprite;
    [SerializeField] private Sprite muteSprite;

    bool isMute = false;

    private void Start()
    {
        playButton.onClick.AddListener(OnClickPlay);
        muteButton.onClick.AddListener(OnClickMute);
        loadLobbyButton.onClick.AddListener(OnClickLoadLobby);
    }

    #region button click events
    private void OnClickPlay()
    {
        AudioManager.instance.PlayButtonClick();
        Time.timeScale = 1f;
        UIManager.Instance.PopUI();
    }

    private void OnClickMute()
    {
        isMute = !isMute;
        if (isMute)
            muteImage.sprite = muteSprite;
        else
            muteImage.sprite = unMuteSprite;

        AudioManager.instance.Mute(isMute);
    }

    private void OnClickLoadLobby()
    {
        AudioManager.instance.PlayButtonClick();
        GameManager.Instance.ChangeGameState(GameManager.GameState.LobbyScene);
    }
    #endregion
}
