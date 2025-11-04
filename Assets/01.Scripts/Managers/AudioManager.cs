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
    public AudioClip restBGM;    // 휴식방브금

    [Header("SFX Clips")]
    public AudioClip arrowShot;
    public AudioClip arrowHit;
    public AudioClip getHitSFX;
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
    public void PlayBGM(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null) return;
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayNormalBGM()
    {
        audioSource.clip = normalBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayRestBGM()
    {
        audioSource.clip = restBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayBossBGM()
    {
        PlayBGM(bossBGM, 0.5f);
    }

    public void PlayClearBGM()
    {
        audioSource.PlayOneShot(clearBGM);
    }

    public void PlayFailBGM()
    {
        audioSource.clip = failBGM;
        audioSource.loop = false;
        audioSource.Play();
    }

    // SFX 매서드
    public void ArrowShot()
    {
        audioSource.PlayOneShot(arrowShot);
    }

    public void ArrowHit()
    { 
        audioSource.PlayOneShot(arrowHit);
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(getHitSFX);
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