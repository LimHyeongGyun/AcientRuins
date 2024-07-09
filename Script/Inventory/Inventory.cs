using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;

public class Inventory : MonoBehaviour
{
    private UIManager uiManager;
    //참조 스크립트
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

    public List<ItemInfo> weaponStorage = new List<ItemInfo>(); //무기 저장
    public List<ItemInfo> shieldStorage = new List<ItemInfo>(); //방패 저장
    public List<ItemInfo> consumableItemStorage = new List<ItemInfo>(); //소모성 아이템 저장
    private int itemNum; //넘겨줄 아이템의 아이템번호정보
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

        AddItem(setWeapon); //녹슨검 획득
        AddItem(potion); //드레이크의 피 획득
        eqData.EquipWeapon(setWeapon);
        dataManager.ChangeWeapon();
        inventoryUI.weaponSlots[0].outLine.enabled = true; //시작시 소지한 녹슨검 장착표시 켜주기
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
            if (_item.itemdata.itemName == "고대의 삽")
            {
                dialogueManager.FindContext("삽 유물 획득");
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
        //반복되는 아이템이 아닐때
        if (_item.iInfo.repeat == false)
        {
            inventoryUI.InstantiateStorage(_item, _item.itemdata.partNum, itemNum);
        }
    }

    //현재 소지하고 있는 소비 아이템인지 확인
    private void CheckRepeatConsumableItem(ItemInfo _item)
    {
        //인벤토리 소비 아이템 창이 비어있다면
        if (consumableItemStorage.Count == 0)
        {
            consumableItemStorage.Add(ItemDB.ConsumeItem[_item.iInfo.ItemNum]); //인벤토리에 추가
        }
        else if (consumableItemStorage.Count != 0)
        {
            foreach (ItemInfo info in consumableItemStorage)
            {
                //인벤토리에 해당 아이템이 있다면
                if (info.iInfo.ItemNum == _item.iInfo.ItemNum)
                {
                    _item.iInfo.repeat = true;
                    //현재 최대 소지 갯수보다 많다면
                    if (info.iInfo.maxamount <= info.iInfo.amount)
                    {
                        break;
                    }
                    //최대 소지 갯수보다 많지 않다면
                    else if (info.iInfo.maxamount > info.iInfo.amount)
                    {
                        PlusItemAmount(info, _item);
                    }
                }
                //인벤토리에 해당 아이템이 없다면
                else if (info == consumableItemStorage[consumableItemStorage.Count - 1] && info.iInfo.ItemNum != _item.iInfo.ItemNum)
                {
                    consumableItemStorage.Add(ItemDB.ConsumeItem[_item.iInfo.ItemNum]); //인벤토리에 추가
                }
            }
        }
    }
    //중복되는 소비아이템 갯수 추가
    public void PlusItemAmount(ItemInfo curItem, ItemInfo getItem)
    {
        curItem.iInfo.amount += getItem.iInfo.amount;
        //아이템을 추가하고 최대 소지갯수보다 많다면
        if (curItem.iInfo.amount > curItem.iInfo.maxamount)
        {
            curItem.iInfo.amount = curItem.iInfo.maxamount;
        }
        //아이템을 추가하고 최대 소지갯수거나 보다 적다면
        else
        {
            
        }
        //슬롯에 데이터 갱신
        UpdateItemData(curItem);
    }
    public void UpdateItemData(ItemInfo _item)
    {
        //소모성 아이템 일 때
        if (_item.itemdata.itemKind == ItemKind.Consumable)
        {
            //인벤토리에 데이터 갱신
            foreach(ItemInfo item in consumableItemStorage)
            {
                //동일한 아이템을 찾았다면
                if (item.iInfo.ItemNum == _item.iInfo.ItemNum)
                {
                    item.iInfo.amount = _item.iInfo.amount;
                }
            }
            //인벤토리UI 슬롯에 데이터 갱신
            foreach (ItemSlot item in inventoryUI.consumableSlots)
            {
                //슬롯에서 같은 아이템을 찾았다면
                if (item.itemInfo.iInfo.ItemNum == _item.iInfo.ItemNum)
                {
                    item.GetInformation(_item, _item.iInfo.recoveryHp, _item.iInfo.amount, _item.iInfo.ItemNum);
                    //착용한 소모 아이템이 없거나 다른 아이템을 착용하고 있을 때
                    if (eqData.consumeItemData.consumeItemInfo == null || eqData.consumeItemData.consumeItemInfo.iInfo.ItemNum != _item.iInfo.ItemNum)
                    {
                        continue;
                    }
                    //해당 아이템을 착용하고 있는상태일 때
                    else if (eqData.consumeItemData.consumeItemInfo.iInfo.ItemNum == _item.iInfo.ItemNum)
                    {
                        //착용 아이템 데이터를 인벤토리의 동일 아이템 데이터로 업데이트
                        eqData.consumeItemData.consumeItemInfo = _item;
                        //슬롯의 소지갯수 업데이트
                        eqData.consumeItemData.eqItemAmount.text = _item.iInfo.amount.ToString();
                    }
                    break;
                }
            }
        }
        //방패 아이템일 때
        else if (_item.itemdata.itemKind == ItemKind.Shield)
        {
            //인벤토리 아이템 정보 갱신
            foreach (ItemInfo item in shieldStorage)
            {
                Debug.Log(item.itemdata.itemName);
                Debug.Log(_item.itemdata.itemName);
                if (item.itemdata.itemName == _item.itemdata.itemName)
                {
                    //아이템이 파괴되었다면
                    if (_item.sStats.destory)
                    {
                        //인벤토리에서 파괴로 바꿔주기
                        item.sStats.destory = true;
                        break;
                    }
                }
            }
            //인벤토리UI 슬롯 데이터 갱신
            foreach (ItemSlot item in inventoryUI.shieldSlots)
            {
                if (item.itemInfo.itemdata.itemName == _item.itemdata.itemName)
                {
                    item.GetInformation(_item, _item.sStats.durability, 0, _item.sStats.shieldNum);
                    //아이템이 파괴되었다면
                    if (_item.sStats.destory)
                    {
                        //슬롯에서 파괴로 바꿔주기
                        item.itemInfo.sStats.destory = true;
                        item.itemInfo_2.text = "파괴";
                        break;
                    }
                }
            }
        }
    }
}