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
        //��Ȱ ���� ��
        if (gameOver && SceneManager.GetActiveScene().name == dataManager.saveSceneName)
        {
            //��� �� ��Ȱ��
            gameOver = false;
            overUI.SetActive(gameOver);
            player = FindObjectOfType<Player>();
            playerView = FindObjectOfType<PlayerView>();

            //������ ���̽�ķ���� ��ġ�� �̵�
            Vector3 pos = new Vector3(dataManager.posx, dataManager.posy, dataManager.posz);
            player.transform.position = pos;
            playerView.transform.position = pos;
        }
        //��Ż�� �� ��ȯ��
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
    //���ο� ���� ����
    public void StartNewGame()
    {
        dataManager.save = false;
        saveGame = false;
        load = false;
        loadSceneName = "FirstMap";
        uiManager.ActiveCursor();
    }
    //������ ���� ���� �ҷ�����
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
            //����� �� ���� �ҷ�����
            dataManager.LoadData();
            loadSceneName = dataManager.saveSceneName;
            //�ε� ������ �̵�
            ChangeLoadingScene();
        }
        else if (!saveGame)
        {
            mainSceneText.enabled = true;
            keyText.enabled = true;
            mainSceneText.text = "����� ���� �����Ͱ� �����ϴ�." + "\n" + "���ο� ������ �÷��� �� �ּ���";
            keyText.text = "EnterŰ�� ���� ����ϱ�";
        }
    }
    public void LoadSceneInfo()
    {
        loadManager.sceneName = loadSceneName;
    }
    //�ε� ������ �̵��ϱ�
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
    //�׾��� �� ���� ����۹�ư
    public void RestartGame()
    {
        dataManager.LoadData();
        player.inventory.InventoryClear();
        player.inventory.inventoryUI.InventoryUIClear();
        //���̺� ��ġ ȣ��
        loadSceneName = dataManager.saveSceneName;
        ChangeLoadingScene();
        //�÷��̾� ȣ��
        player.Revive();
    }
    //���� ����
    public void QuitGame()
    {
        Application.Quit();
    }
}
