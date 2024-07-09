using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCamp : MonoBehaviour
{
    public Player player;
    private DataManager dataManager;
    private UIManager uiManager;
    [SerializeField]
    private ItemDataBase itemDB;
    [SerializeField]
    private UpgradePlayerStats upgradePlayerStats;

    public GameObject basecampeUI;
    public bool activeUI;

    public GameObject upgradeUI;
    private bool activeUpgradeUI;

    public Transform basecampTransform;

    public SpawnManager[] spawnerList;
    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
        uiManager = FindObjectOfType<UIManager>();
        upgradeUI.SetActive(false);
    }
    private void Start()
    {
        activeUI = true;
        ActiveBaseCampUI();
    }
    //���̽�ķ�� ���Խ� UIȭ�� ����
    public void ActiveBaseCampUI()
    {
        activeUI = !activeUI;
        basecampeUI.SetActive(activeUI);
    }
    //���̽� ķ���� ���Խ� �ڵ� ����
    #region
    public void EnterBaseCamp()
    {
        FillHealth();
        ReSpawnMonster();
        FillDrakeBlood();
    }
    //���� ����
    private void FillDrakeBlood()
    {
        if (player.equipData.consumeItemData.equip)
        {
            if (player.equipData.consumeItemData.consumeItemInfo.name == "�巹��ũ�� ��")
            {
                if (player.inventory.consumableItemStorage[0].iInfo.amount < itemDB.ConsumeItem[0].iInfo.maxamount)
                {
                    player.inventory.consumableItemStorage[0].iInfo.amount = itemDB.ConsumeItem[0].iInfo.maxamount;
                    player.equipData.consumeItemData.eqItemAmount.text = player.equipData.consumeItemData.consumeItemInfo.iInfo.amount.ToString();
                    player.inventory.UpdateItemData(player.equipData.consumeItemData.consumeItemInfo);
                }
            }
        }
    }
    //ü�� ȸ��
    private void FillHealth()
    {
        player.IncreaseHp(player.data.maxHp); //Nur
    }
    #endregion

    //������ ����
    public void SavePlayData()
    {
        dataManager.saveSceneName = SceneManager.GetActiveScene().name;
        dataManager.baseTransform = basecampTransform;
        dataManager.SaveData();
    }

    //��ȭ ȭ�� ���� button
    public void ActiveUpgradeUI()
    {
        activeUpgradeUI = !activeUpgradeUI;
        upgradeUI.SetActive(activeUpgradeUI);
        if (activeUpgradeUI)
        {
            upgradePlayerStats.player = this.player;
            upgradePlayerStats.UpdateInformation();
        }
    }

    //ȭ�� �ݱ�
    public void ReturnGame()
    {
        ActiveBaseCampUI();
        uiManager.ActiveCursor();
        player.interactive = false;
    }
    //ü�� ȸ���� ���� ���� ������
    public void ReSpawnMonster()
    {
        spawnerList = null; //����Ʈ �ʱ�ȭ
        spawnerList = FindObjectsOfType<SpawnManager>();
        foreach (SpawnManager spawner in spawnerList)
        {
            //�ڽ� ������Ʈ�� ������ ���� �ʱ�ȭ
            for (int i = 0; i < spawner.transform.childCount; i++)
            {
                Destroy(spawner.transform.GetChild(i).gameObject);
            }
            //���� ����
            spawner.Spawn();
        }
    }
}
