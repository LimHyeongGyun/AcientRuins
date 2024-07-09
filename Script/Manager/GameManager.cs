using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private LoadManager loadManager;
    [SerializeField]
    private Player player;
    [SerializeField]
    private PlayerView playerView;
    public UIController uiController;

    public Transform portalTransform;
    public bool crossScene;
    public bool gameOver = false;
    public bool saveGame;
    public bool load;
    public bool start;
    public string loadSceneName;

    public GameObject overUI;
    [SerializeField]
    private Text mainSceneText;
    [SerializeField]
    private Text keyText;

    public bool activeCursor;

    private void Start()
    {
        mainSceneText.enabled = false;
        keyText.enabled = false;
        start = true;
    }
    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "1 LoadScene")
        {
            loadManager = FindObjectOfType<LoadManager>();
            LoadSceneInfo();
        }
        //부활 했을 때
        if (gameOver && SceneManager.GetActiveScene().name == dataManager.saveSceneName)
        {
            //사망 후 부활시
            gameOver = false;
            overUI.SetActive(gameOver);
            player = FindObjectOfType<Player>();
            playerView = FindObjectOfType<PlayerView>();

            //저장한 베이스캠프의 위치로 이동
            Vector3 pos = new Vector3(dataManager.posx, dataManager.posy, dataManager.posz);
            player.transform.position = pos;
            playerView.transform.position = pos;
        }
        //포탈로 씬 전환시
        if (crossScene && SceneManager.GetActiveScene().name == loadSceneName)
        {
            crossScene = false;
            portalTransform = GameObject.Find("PortalPoint").transform;
            player.transform.position = portalTransform.position;
            playerView.transform.position = portalTransform.position;
        }
    }
    private void Update()
    {
        if (mainSceneText != null && mainSceneText.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                mainSceneText.enabled = false;
                keyText.enabled = false;
            }
        }
    }
    //새로운 게임 시작
    public void StartNewGame()
    {
        dataManager.save = false;
        saveGame = false;
        load = false;
        loadSceneName = "FirstMap";
        uiManager.ActiveCursor();
    }
    //저장해 놓은 게임 불러오기
    public void LoadSaveGame()
    {
        if (PlayerPrefs.HasKey("SAVE"))
        {
            dataManager.save = System.Convert.ToBoolean(PlayerPrefs.GetInt("SAVE"));
            saveGame = dataManager.save;
        }
        if (dataManager.save && saveGame)
        {
            load = true;
            uiManager.ActiveCursor();
            //저장된 씬 정보 불러오기
            dataManager.LoadData();
            loadSceneName = dataManager.saveSceneName;
            //로딩 씬으로 이동
            ChangeLoadingScene();
        }
        else if (!saveGame)
        {
            mainSceneText.enabled = true;
            keyText.enabled = true;
            mainSceneText.text = "저장된 게임 데이터가 없습니다." + "\n" + "새로운 게임을 플레이 해 주세요";
            keyText.text = "Enter키를 눌러 계속하기";
        }
    }
    public void LoadSceneInfo()
    {
        loadManager.sceneName = loadSceneName;
    }
    //로딩 씬으로 이동하기
    public void ChangeLoadingScene()
    {
        SceneManager.LoadScene("1 LoadScene");
    }
    public void SetElement()
    {
        player = FindObjectOfType<Player>();
        playerView = FindObjectOfType<PlayerView>();
        overUI = GameObject.FindWithTag("GameOver");
        overUI.SetActive(gameOver);
        dataManager.SetPlayerData();
    }
    public void GameOver()
    {
        overUI.SetActive(gameOver);
        overUI.GetComponentInChildren<Text>().text = "You Dead" + "\n" + "Press 'E' to ReStart";
        uiController = FindObjectOfType<UIController>();
        uiController.restart = false;
    }
    //죽었을 때 게임 재시작버튼
    public void RestartGame()
    {
        dataManager.LoadData();
        player.inventory.InventoryClear();
        player.inventory.inventoryUI.InventoryUIClear();
        //세이브 위치 호출
        loadSceneName = dataManager.saveSceneName;
        ChangeLoadingScene();
        //플레이어 호출
        player.Revive();
    }
    //게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }
}
