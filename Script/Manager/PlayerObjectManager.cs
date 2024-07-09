using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private GameObject cameraObject;
    [SerializeField]
    private GameObject UIElement;
    
    private Inventory inventory;

    public void Start()
    {
        UIElement.SetActive(false);
    }
    private void OnLevelWasLoaded()
    {
        //���� ó�� ���۽� �÷��̾� ����
        if (gameManager.start && SceneManager.GetActiveScene().name == "FirstMap")
        {
            SetPlayElement();
        }
        //����� ���� �ҷ� ���� ��
        else if (gameManager.load && SceneManager.GetActiveScene().name == dataManager.saveSceneName)
        {
            SetPlayElement();
        }
    }

    private void SetPlayElement()
    {
        UIElement.SetActive(true);
        gameManager.start = false;
        Instantiate(UIElement, transform.position, Quaternion.identity);
        GameObject player = Instantiate(playerObject);
        GameObject camera = Instantiate(cameraObject);
        player.transform.position = GameObject.Find("PortalPoint").transform.position;
        camera.transform.position = player.transform.position;
        gameManager.SetElement();
        if (gameManager.load)
        {
            inventory = FindObjectOfType<Inventory>();
            inventory.InventoryClear();
            inventory.inventoryUI.InventoryUIClear();
        }
    }
}
