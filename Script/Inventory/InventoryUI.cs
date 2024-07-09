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
    public bool activeUI; //UI ��/Ȱ��ȭ

    public ItemSlot itemSlot;
    //���� ������â
    public List<ItemSlot> weaponSlots;
    //���� ������â
    public List<ItemSlot> shieldSlots;
    //�Һ������â
    public List<ItemSlot> consumableSlots;

    public GameObject[] PartList;
    public Sprite[] wInfoImgArr; //��������� ���� �̹��� ����
    public Sprite[] sInfoImgArr; //���о����� ���� �̹��� ����
    public Sprite[] cInfoImgArr; //�Һ������ ���� �̹��� ����

    //�÷��̾� ���� ����ִ� UI
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
    //UI Ȱ�� ��Ȱ��ȭ
    public void ActiveInventory()
    {
        activeUI = !activeUI;
        InventoryPannel.SetActive(activeUI);
        PlayerInformation();
        if (activeUI) //InventoryUI�� ���� ��
        {
            //�׻� �ٸ� ��� Part�� ������ �ʵ��� �ϰ� �� ù Part�� ǥ�õǵ���
            foreach(GameObject list in PartList)
            {
                list.SetActive(false);
            }
            PartList[0].SetActive(true);
        }
    }
    //Part UI ��/Ȱ��ȭ
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
        playerPowerInfo.text = $"�ٷ�{fillStr}/" + $"�� ���ݷ�: {dataManager.power}" + 
            "\n" + $"�⺻ ���ݷ�: {1 + (dataManager.powerLv - 1) * 2}" + $"���� ���ݷ�:{dataManager.player.equipData.weaponData.handweaponInfo.wStats.wPower}";
        silverInfo.text = $"{dataManager.silver}";
        acientMemorieInfo.text = $"{dataManager.acientMemorie}";
    }

    //������ ���� ����
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
            slot.info_Img[0].transform.GetChild(0).GetComponent<Text>().text = "������";

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