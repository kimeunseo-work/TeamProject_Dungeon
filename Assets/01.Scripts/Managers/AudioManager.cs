using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    [Header("BGM Clips")]
    public AudioClip startBGM;   // LobbyScene
    public AudioClip normalBGM;  // DungeonScene / 기본 진행
    public AudioClip clearBGM;   // 스테이지 클리어
    public AudioClip failBGM;    // 패배 연출
    public AudioClip bossBGM;    // 보스 스테이지

    [Header("SFX Clips")]
    public AudioClip arrowShot;
    public AudioClip hitSFX;
    public AudioClip clickSFX;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyScene")
        {
            PlayBGM(startBGM);
        }
        else if (scene.name == "DungeonScene")
        {
            PlayBGM(normalBGM);
        }
    }

    //BGM 컨트롤
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayBossBGM()
    {
        PlayBGM(bossBGM);
    }

    public void PlayClearBGM()
    {
        audioSource.clip = clearBGM;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayFailBGM()
    {
        audioSource.clip = failBGM;
        audioSource.loop = false;
        audioSource.Play();
    }

    // SFX 매서드
    public void PlayShot()
    {
        audioSource.PlayOneShot(arrowShot);
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(hitSFX);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSFX);
    }

    public void Mute(bool isMute)
    {
        audioSource.mute = isMute;
    }
}