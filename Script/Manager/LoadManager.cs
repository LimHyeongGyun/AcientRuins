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
                guideText.text = "Acient Ruins를 플레이 해주셔서 감사합니다. 열심히 만들었으니 재밌게 플레이 해주세요.";
                break;
            case 1:
                guideText.text = "유적 안에는 어떤 위험이 도사리고 있을지 아무도 모릅니다.   드워프까지도 말이죠";
                break; 
            case 2:
                guideText.text = "고대의 삽을 찾게된다면 그 삽으로 농사는 추천드리지 않습니다!   지구 반대편까지 파낼지도 모르니까요 하하!";
                break;
            case 3:
                guideText.text = "드워프를 만난다면 친절하게 대해주는건 어떨까요? 당신에게 도움을 줄지도 모릅니다!   많은 비용이 들지도 모르지만요.";
                break;
            case 4:
                guideText.text = "영혼 수호자를 만난다면 당장 도망치세요.   도망칠 수 있다면 말이죠.";
                break;
            case 5:
                guideText.text = "데스나이트는 당신의 영혼을 취하려 들겁니다.   맞서싸우는것도 방법이겠죠.";
                break;
            default:
                guideText.text = "깊은 유적에는 무엇이 있을지 모릅니다.   혹시 모르죠 황금을 발견해 인생역전을 할지도?";
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
