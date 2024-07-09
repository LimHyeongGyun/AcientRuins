using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;

public class Inventory : MonoBehaviour
{
    private UIManager uiManager;
    //���� ��ũ��Ʈ
    [HideInInspector]
    public InventoryUI inventoryUI;
    private Player player;
    [HideInInspector]
    public ItemDataBase ItemDB;
    [SerializeField]
    private EquipData eqData;
    private GameManager gameManager;
    private DataManager dataManager;
    private DialogueManager dialogueManager;

    public List<ItemInfo> weaponStorage = new List<ItemInfo>(); //���� ����
    public List<ItemInfo> shieldStorage = new List<ItemInfo>(); //���� ����
    public List<ItemInfo> consumableItemStorage = new List<ItemInfo>(); //�Ҹ� ������ ����
    private int itemNum; //�Ѱ��� �������� �����۹�ȣ����
    private void Awake()
    {
        player = GetComponent<Player>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        ItemDB = FindObjectOfType<ItemDataBase>();
        gameManager = FindObjectOfType<GameManager>();
        dataManager = FindObjectOfType<DataManager>();
        uiManager = FindObjectOfType<UIManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Start()
    {
        ItemInfo setWeapon = ItemDB.Weapon[0];
        ItemInfo potion = ItemDB.ConsumeItem[0];

        AddItem(setWeapon); //�콼�� ȹ��
        AddItem(potion); //�巹��ũ�� �� ȹ��
        eqData.EquipWeapon(setWeapon);
        dataManager.ChangeWeapon();
        inventoryUI.weaponSlots[0].outLine.enabled = true; //���۽� ������ �콼�� ����ǥ�� ���ֱ�
    }
    public void InventoryClear()
    {
        weaponStorage.Clear();
        shieldStorage.Clear();
        consumableItemStorage.Clear();
    }
    public void AddItem(ItemInfo _item)
    {
        _item.InitSetData();
        if (_item.itemdata.itemKind == ItemKind.Weapon)
        {
            weaponStorage.Add(ItemDB.Weapon[_item.wStats.weaponNum]);
            itemNum = _item.wStats.weaponNum;
            if (_item.itemdata.itemName == "����� ��")
            {
                dialogueManager.FindContext("�� ���� ȹ��");
                dialogueManager.ActiveUI();
            }
        }
        else if (_item.itemdata.itemKind == ItemKind.Shield)
        {
            shieldStorage.Add(ItemDB.Shield[_item.sStats.shieldNum]);
            itemNum = _item.sStats.shieldNum;
        }
        else if (_item.itemdata.itemKind == ItemKind.Consumable)
        {
            CheckRepeatConsumableItem(_item);
            itemNum = _item.iInfo.ItemNum;
        }
        //�ݺ��Ǵ� �������� �ƴҶ�
        if (_item.iInfo.repeat == false)
        {
            inventoryUI.InstantiateStorage(_item, _item.itemdata.partNum, itemNum);
        }
    }

    //���� �����ϰ� �ִ� �Һ� ���������� Ȯ��
    private void CheckRepeatConsumableItem(ItemInfo _item)
    {
        //�κ��丮 �Һ� ������ â�� ����ִٸ�
        if (consumableItemStorage.Count == 0)
        {
            consumableItemStorage.Add(ItemDB.ConsumeItem[_item.iInfo.ItemNum]); //�κ��丮�� �߰�
        }
        else if (consumableItemStorage.Count != 0)
        {
            foreach (ItemInfo info in consumableItemStorage)
            {
                //�κ��丮�� �ش� �������� �ִٸ�
                if (info.iInfo.ItemNum == _item.iInfo.ItemNum)
                {
                    _item.iInfo.repeat = true;
                    //���� �ִ� ���� �������� ���ٸ�
                    if (info.iInfo.maxamount <= info.iInfo.amount)
                    {
                        break;
                    }
                    //�ִ� ���� �������� ���� �ʴٸ�
                    else if (info.iInfo.maxamount > info.iInfo.amount)
                    {
                        PlusItemAmount(info, _item);
                    }
                }
                //�κ��丮�� �ش� �������� ���ٸ�
                else if (info == consumableItemStorage[consumableItemStorage.Count - 1] && info.iInfo.ItemNum != _item.iInfo.ItemNum)
                {
                    consumableItemStorage.Add(ItemDB.ConsumeItem[_item.iInfo.ItemNum]); //�κ��丮�� �߰�
                }
            }
        }
    }
    //�ߺ��Ǵ� �Һ������ ���� �߰�
    public void PlusItemAmount(ItemInfo curItem, ItemInfo getItem)
    {
        curItem.iInfo.amount += getItem.iInfo.amount;
        //�������� �߰��ϰ� �ִ� ������������ ���ٸ�
        if (curItem.iInfo.amount > curItem.iInfo.maxamount)
        {
            curItem.iInfo.amount = curItem.iInfo.maxamount;
        }
        //�������� �߰��ϰ� �ִ� ���������ų� ���� ���ٸ�
        else
        {
            
        }
        //���Կ� ������ ����
        UpdateItemData(curItem);
    }
    public void UpdateItemData(ItemInfo _item)
    {
        //�Ҹ� ������ �� ��
        if (_item.itemdata.itemKind == ItemKind.Consumable)
        {
            //�κ��丮�� ������ ����
            foreach(ItemInfo item in consumableItemStorage)
            {
                //������ �������� ã�Ҵٸ�
                if (item.iInfo.ItemNum == _item.iInfo.ItemNum)
                {
                    item.iInfo.amount = _item.iInfo.amount;
                }
            }
            //�κ��丮UI ���Կ� ������ ����
            foreach (ItemSlot item in inventoryUI.consumableSlots)
            {
                //���Կ��� ���� �������� ã�Ҵٸ�
                if (item.itemInfo.iInfo.ItemNum == _item.iInfo.ItemNum)
                {
                    item.GetInformation(_item, _item.iInfo.recoveryHp, _item.iInfo.amount, _item.iInfo.ItemNum);
                    //������ �Ҹ� �������� ���ų� �ٸ� �������� �����ϰ� ���� ��
                    if (eqData.consumeItemData.consumeItemInfo == null || eqData.consumeItemData.consumeItemInfo.iInfo.ItemNum != _item.iInfo.ItemNum)
                    {
                        continue;
                    }
                    //�ش� �������� �����ϰ� �ִ»����� ��
                    else if (eqData.consumeItemData.consumeItemInfo.iInfo.ItemNum == _item.iInfo.ItemNum)
                    {
                        //���� ������ �����͸� �κ��丮�� ���� ������ �����ͷ� ������Ʈ
                        eqData.consumeItemData.consumeItemInfo = _item;
                        //������ �������� ������Ʈ
                        eqData.consumeItemData.eqItemAmount.text = _item.iInfo.amount.ToString();
                    }
                    break;
                }
            }
        }
        //���� �������� ��
        else if (_item.itemdata.itemKind == ItemKind.Shield)
        {
            //�κ��丮 ������ ���� ����
            foreach (ItemInfo item in shieldStorage)
            {
                Debug.Log(item.itemdata.itemName);
                Debug.Log(_item.itemdata.itemName);
                if (item.itemdata.itemName == _item.itemdata.itemName)
                {
                    //�������� �ı��Ǿ��ٸ�
                    if (_item.sStats.destory)
                    {
                        //�κ��丮���� �ı��� �ٲ��ֱ�
                        item.sStats.destory = true;
                        break;
                    }
                }
            }
            //�κ��丮UI ���� ������ ����
            foreach (ItemSlot item in inventoryUI.shieldSlots)
            {
                if (item.itemInfo.itemdata.itemName == _item.itemdata.itemName)
                {
                    item.GetInformation(_item, _item.sStats.durability, 0, _item.sStats.shieldNum);
                    //�������� �ı��Ǿ��ٸ�
                    if (_item.sStats.destory)
                    {
                        //���Կ��� �ı��� �ٲ��ֱ�
                        item.itemInfo.sStats.destory = true;
                        item.itemInfo_2.text = "�ı�";
                        break;
                    }
                }
            }
        }
    }
}