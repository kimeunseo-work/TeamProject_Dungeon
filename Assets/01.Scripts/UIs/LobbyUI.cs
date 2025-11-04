using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("Level & Exp")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expPercentageText;

    [Header("Gold")]
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("Play")]
    [SerializeField] private Button playButton;

    [Header("Door")]
    [SerializeField] private Image doorImage;
    [SerializeField] private Sprite doorCloseSprite;
    [SerializeField] private Sprite doorOpenSprite;

    private void Awake()
    {
        playButton.onClick.AddListener(OnClickPlay);
    }

    private void Start()
    {
        UpdateAllUIs();

        doorImage.sprite = doorCloseSprite;

        PlayerLobbyStatus.Instance.OnPointChanged += UpdateGoldUI;
        PlayerLobbyStatus.Instance.OnBaseExpChanged += UpdateExpUI;
        PlayerLobbyStatus.Instance.OnBaseLevelChanged += UpdateLevelUI;
    }

    private void OnDestroy()
    {
        if (PlayerLobbyStatus.Instance != null)
        {
            PlayerLobbyStatus.Instance.OnPointChanged -= UpdateGoldUI;
            PlayerLobbyStatus.Instance.OnBaseExpChanged -= UpdateExpUI;
            PlayerLobbyStatus.Instance.OnBaseLevelChanged -= UpdateLevelUI;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerLobbyStatus.Instance.IncreaseBaseExp(10);
        }
    }

    private void UpdateAllUIs()
    {
        UpdateGoldUI();
        UpdateExpUI();
        UpdateLevelUI();
    }

    private void UpdateGoldUI()
    {
        goldText.text = PlayerLobbyStatus.Instance.Point.ToString();
    }

    private void UpdateExpUI()
    {
        float targetValue;
        if (PlayerLobbyStatus.Instance.RequiredBaseExp == 0)
            targetValue = (float)PlayerLobbyStatus.Instance.BaseExp / 1;
        else
            targetValue = (float)PlayerLobbyStatus.Instance.BaseExp/ PlayerLobbyStatus.Instance.RequiredBaseExp;
        UIManager.Instance.AnimateSlider(expSlider, targetValue);

        expPercentageText.text = $"{(int)(targetValue * 100)}%";
    }

    private void UpdateLevelUI()
    {
        levelText.text = PlayerLobbyStatus.Instance.BaseLevel.ToString();
        UpdateExpUI();
    }

    private void OnClickPlay()
    {
        AudioManager.instance.PlayButtonClick();
        doorImage.sprite = doorOpenSprite;
        GameManager.Instance.ChangeGameState(GameManager.GameState.DungeonScene);
    }
}
