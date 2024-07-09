using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] backgroundMusicClips; // 씬별 배경 음악을 저장하는 배열
    private AudioSource audioSource;

    void Awake()
    {
        // 싱글톤 패턴을 사용하여 배경 음악 관리자가 씬 전환 시 파괴되지 않도록 합니다.
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // 씬이 변경될 때마다 OnSceneLoaded 메서드를 호출하도록 이벤트를 등록합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 현재 씬에 맞는 음악을 재생합니다.
        PlayBackgroundMusic(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 변경될 때마다 해당 씬에 맞는 음악을 재생합니다.
        PlayBackgroundMusic(scene.buildIndex);
    }

    void PlayBackgroundMusic(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < backgroundMusicClips.Length)
        {
            AudioClip clipToPlay = backgroundMusicClips[sceneIndex];

            if (clipToPlay != null && audioSource.clip != clipToPlay)
            {
                audioSource.clip = clipToPlay;
                audioSource.loop = true; // 배경 음악 반복 재생
                audioSource.Play();
            }
        }
    }
}
