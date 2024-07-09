using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uiManager;
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private GameObject setList;
    [SerializeField]
    private GameObject gameInformation;

    public bool activeUI;
    public bool activeInfo;
    public bool restart;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager.uiController = this;
    }
    public void Start()
    {
        restart = true;
        activeInfo = true;
        ActiveGameInformation();
        setList.SetActive(activeUI);
    }
    private void Update()
    {
        ActiveSetList();
        if (!restart)
        {
            RestartUI();
        }
    }
    private void ActiveSetList()
    {
        if (!setList.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            activeUI = true;
            setList.SetActive(activeUI);
            uiManager.ActiveCursor();
        }
        else if (setList.activeSelf && activeUI && Input.GetKeyDown(KeyCode.Escape))
        {
            //아무것도 안켜져 있다면 리스트 창 끄기
            if (!gameInformation.activeSelf && !inventoryUI.InventoryPannel.activeSelf)
            {
                ActiveUI();
            }
            //정보창이 켜져있다면 정보창 끄기
            else if (gameInformation.activeSelf)
            {
                gameInformation.SetActive(false);
            }
            //인벤토리가 켜져있다면 인벤토리 끄기
            else if (inventoryUI.InventoryPannel.activeSelf)
            {
                inventoryUI.ActiveInventory();
            }
        }
        else if (setList.activeSelf && !activeUI)
        {
            setList.SetActive(activeUI);
            uiManager.ActiveCursor();
        }
    }
    public void ActiveUI()
    {
        activeUI = false;
    }
    public void ActiveGameInformation()
    {
        activeInfo = !activeInfo;
        gameInformation.SetActive(activeInfo);
    }
    public void RestartUI()
    {
        if (gameManager.overUI.activeSelf && gameManager.gameOver && Input.GetKeyDown(KeyCode.E))
        {
            restart = true;
            gameManager.RestartGame();
        }
    }
    public void QuitGame()
    {
        gameManager.QuitGame();
    }
}
