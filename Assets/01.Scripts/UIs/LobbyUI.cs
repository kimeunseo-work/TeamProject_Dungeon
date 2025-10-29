using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button playButton;

    private void Awake()
    {
        UIManager.Instance.PushUI(gameObject);
        playButton.onClick.AddListener(OnClickPlay);
    }

    private void Start()
    {
        UpdateGoldUI();
        // StatusManager.Instance.OnGoldChanged += UpdateGoldUI;
    }

    private void OnDestroy()
    {
        //if (StatusManager.Instance != null)
        //    StatusManager.Instance.OnGoldChanged -= UpdateGoldUI;
    }

    private void UpdateGoldUI()
    {
        //goldText.text = StatusManager.Instance.Gold.ToString();
    }

    private void OnClickPlay()
    {
        UIManager.Instance.PopUI();
        SceneManager.LoadScene(nameof(GameManger.GameState.DungeonScene));
    }
}
