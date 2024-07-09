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
    //베이스캠프 진입시 UI화면 띄우기
    public void ActiveBaseCampUI()
    {
        activeUI = !activeUI;
        basecampeUI.SetActive(activeUI);
    }
    //베이스 캠프를 진입시 자동 진행
    #region
    public void EnterBaseCamp()
    {
        FillHealth();
        ReSpawnMonster();
        FillDrakeBlood();
    }
    //물약 충전
    private void FillDrakeBlood()
    {
        if (player.equipData.consumeItemData.equip)
        {
            if (player.equipData.consumeItemData.consumeItemInfo.name == "드레이크의 피")
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
    //체력 회복
    private void FillHealth()
    {
        player.IncreaseHp(player.data.maxHp); //Nur
    }
    #endregion

    //데이터 저장
    public void SavePlayData()
    {
        dataManager.saveSceneName = SceneManager.GetActiveScene().name;
        dataManager.baseTransform = basecampTransform;
        dataManager.SaveData();
    }

    //강화 화면 열기 button
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

    //화면 닫기
    public void ReturnGame()
    {
        ActiveBaseCampUI();
        uiManager.ActiveCursor();
        player.interactive = false;
    }
    //체력 회복시 죽은 몬스터 리스폰
    public void ReSpawnMonster()
    {
        spawnerList = null; //리스트 초기화
        spawnerList = FindObjectsOfType<SpawnManager>();
        foreach (SpawnManager spawner in spawnerList)
        {
            //자식 오브젝트로 생성된 몬스터 초기화
            for (int i = 0; i < spawner.transform.childCount; i++)
            {
                Destroy(spawner.transform.GetChild(i).gameObject);
            }
            //새로 스폰
            spawner.Spawn();
        }
    }
}
