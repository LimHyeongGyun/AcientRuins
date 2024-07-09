using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ItemData;

public class InventoryUI : MonoBehaviour
{
    private UIManager uiManager;
    private DataManager dataManager;
    [SerializeField]
    private ItemDataBase ItemDB;
    public GameObject InventoryPannel; //InventoryUI
    public bool activeUI; //UI 비/활성화

    public ItemSlot itemSlot;
    //무기 아이템창
    public List<ItemSlot> weaponSlots;
    //방패 아이템창
    public List<ItemSlot> shieldSlots;
    //소비아이템창
    public List<ItemSlot> consumableSlots;

    public GameObject[] PartList;
    public Sprite[] wInfoImgArr; //무기아이템 정보 이미지 저장
    public Sprite[] sInfoImgArr; //방패아이템 정보 이미지 저장
    public Sprite[] cInfoImgArr; //소비아이템 정보 이미지 저장

    //플레이어 정보 띄워주는 UI
    #region
    public Text playerHpInfo;
    public Text playerStrengthInfo;
    public Text playerPowerInfo;
    public Text silverInfo;
    public Text acientMemorieInfo;
    public string fillStr;
    #endregion

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        dataManager = FindObjectOfType<DataManager>();
    }
    private void Start()
    {
        activeUI = true;
        ActiveInventory();
    }
    public void InventoryUIClear()
    {
        weaponSlots.Clear();
        shieldSlots.Clear();
        consumableSlots.Clear();
        for (int i = 0; i < PartList.Length; i++)
        {
            for (int j = 0; j < PartList[i].GetComponent<ScrollRect>().content.transform.childCount; j++)
            {
                Destroy(PartList[i].GetComponent<ScrollRect>().content.transform.GetChild(j).gameObject);
                if (PartList[i] == PartList[^1])
                {
                    PartList[i].GetComponent<ScrollRect>().content.transform.DetachChildren();
                    if (PartList[i].GetComponent<ScrollRect>().content.transform.childCount == 0)
                    {
                        dataManager.LoadItemInfo();
                    }
                }
            }
        }
    }
    //UI 활성 비활성화
    public void ActiveInventory()
    {
        activeUI = !activeUI;
        InventoryPannel.SetActive(activeUI);
        PlayerInformation();
        if (activeUI) //InventoryUI가 켜질 때
        {
            //항상 다른 모든 Part는 보이지 않도록 하고 맨 첫 Part가 표시되도록
            foreach(GameObject list in PartList)
            {
                list.SetActive(false);
            }
            PartList[0].SetActive(true);
        }
    }
    //Part UI 비/활성화
    public void ActivePartUI(int num)
    {
        foreach(GameObject list in PartList)
        {
            list.SetActive(false);
        }
        PartList[num].SetActive(true);
    }
    public void PlayerInformation()
    {
        playerHpInfo.text = $"{dataManager.maxHp}";
        playerStrengthInfo.text = $"{dataManager.strength}";
        playerPowerInfo.text = $"근력{fillStr}/" + $"총 공격력: {dataManager.power}" + 
            "\n" + $"기본 공격력: {1 + (dataManager.powerLv - 1) * 2}" + $"무기 공격력:{dataManager.player.equipData.weaponData.handweaponInfo.wStats.wPower}";
        silverInfo.text = $"{dataManager.silver}";
        acientMemorieInfo.text = $"{dataManager.acientMemorie}";
    }

    //아이템 슬롯 생성
    public void InstantiateStorage(ItemInfo iteminfo, int partnum, int itemNum)
    {
        ItemSlot slot = Instantiate(itemSlot);
        if(partnum == 0)
        {
            for (int i = 0; i < slot.info_Img.Length; i++)
            {
                slot.info_Img[i].sprite = wInfoImgArr[i];
            }
            slot.GetInformation(iteminfo, iteminfo.wStats.wPower, iteminfo.wStats.needStr, itemNum);
            slot.tag = "WeaponSlot";
            weaponSlots.Add(slot);
        }
        else if(partnum == 1)
        {
            slot.info_Img[1].sprite = sInfoImgArr[0];
            Color color = slot.info_Img[0].GetComponent<Image>().color;
            color.a = 0f;
            slot.info_Img[0].GetComponent<Image>().color = color;
            slot.info_Img[0].transform.GetChild(0).GetComponent<Text>().text = "내구도";

            slot.GetInformation(iteminfo, ItemDB.Shield[itemNum].sStats.durability, 0, itemNum);
            slot.tag = "ShieldSlot";
            shieldSlots.Add(slot);
        }
        else if (partnum == 2)
        {
            for (int i = 0; i < slot.info_Img.Length; i++)
            {
                slot.info_Img[i].sprite = cInfoImgArr[i];
            }
            slot.GetInformation(iteminfo, ItemDB.ConsumeItem[itemNum].iInfo.recoveryHp, iteminfo.iInfo.amount, itemNum);
            slot.tag = "ConsumeItemSlot";
            consumableSlots.Add(slot);
        }
        slot.transform.parent = PartList[partnum].GetComponent<ScrollRect>().content.transform;
    }
    public void EquipWeapon()
    {
        foreach (ItemSlot slots in weaponSlots)
        {
            slots.outLine.enabled = false;
        }
    }
    public void EquipShield()
    {
        foreach (ItemSlot slots in shieldSlots)
        {
            slots.outLine.enabled = false;
        }
    }
    public void EquipConsumeableItem()
    {
        foreach (ItemSlot slots in consumableSlots)
        {
            slots.outLine.enabled = false;
        }
    }
}