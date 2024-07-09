using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Text guideText;
    [SerializeField]
    private Slider loadSlider;
    public string sceneName;
    private int guide;

    private float time;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        GuideText();
        StartCoroutine(LoadAsyncScene());
    }

    public void GuideText()
    {
        guide = Random.Range(0, 6);
        switch (guide)
        {
            case 0:
                guideText.text = "Acient Ruins�� �÷��� ���ּż� �����մϴ�. ������ ��������� ��հ� �÷��� ���ּ���.";
                break;
            case 1:
                guideText.text = "���� �ȿ��� � ������ ���縮�� ������ �ƹ��� �𸨴ϴ�.   ����������� ������";
                break; 
            case 2:
                guideText.text = "����� ���� ã�Եȴٸ� �� ������ ���� ��õ�帮�� �ʽ��ϴ�!   ���� �ݴ������ �ĳ����� �𸣴ϱ�� ����!";
                break;
            case 3:
                guideText.text = "������� �����ٸ� ģ���ϰ� �����ִ°� ����? ��ſ��� ������ ������ �𸨴ϴ�!   ���� ����� ������ ��������.";
                break;
            case 4:
                guideText.text = "��ȥ ��ȣ�ڸ� �����ٸ� ���� ����ġ����.   ����ĥ �� �ִٸ� ������.";
                break;
            case 5:
                guideText.text = "��������Ʈ�� ����� ��ȥ�� ���Ϸ� ��̴ϴ�.   �¼��ο�°͵� ����̰���.";
                break;
            default:
                guideText.text = "���� �������� ������ ������ �𸨴ϴ�.   Ȥ�� ���� Ȳ���� �߰��� �λ������� ������?";
                break;
        }
    }
    IEnumerator LoadAsyncScene()
    {
        time = 0;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            time += Time.deltaTime;
            loadSlider.value = time / 4f;
            if (loadSlider.value >= 1)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
