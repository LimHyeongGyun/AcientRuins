using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] backgroundMusicClips; // ���� ��� ������ �����ϴ� �迭
    private AudioSource audioSource;

    void Awake()
    {
        // �̱��� ������ ����Ͽ� ��� ���� �����ڰ� �� ��ȯ �� �ı����� �ʵ��� �մϴ�.
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // ���� ����� ������ OnSceneLoaded �޼��带 ȣ���ϵ��� �̺�Ʈ�� ����մϴ�.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ���� ���� �´� ������ ����մϴ�.
        PlayBackgroundMusic(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ����� ������ �ش� ���� �´� ������ ����մϴ�.
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
                audioSource.loop = true; // ��� ���� �ݺ� ���
                audioSource.Play();
            }
        }
    }
}
