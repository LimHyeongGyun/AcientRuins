using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeItemData : MonoBehaviour
{
    [SerializeField]
    private Inventory inven;
    public ItemInfo consumeItemInfo;
    public GameObject equipItem;

    [SerializeField]
    private GameObject eqItemSlot;
    public Text eqItemAmount;

    public bool equip;

    private void Awake()
    {
        eqItemSlot = GameObject.Find("ConsumeableItemSlot").transform.GetChild(1).gameObject;
        eqItemAmount = eqItemSlot.transform.GetChild(0).GetComponent<Text>();
    }
    public void UseItem()
    {
        if (consumeItemInfo.iInfo.ItemNum == 0)
        {
            consumeItemInfo.iInfo.amount -= 1;
            var player = FindObjectOfType<Player>();
            player.IncreaseHp(consumeItemInfo.iInfo.recoveryHp);
            eqItemAmount.text = consumeItemInfo.iInfo.amount.ToString();
            inven.UpdateItemData(consumeItemInfo);
        }
    }
    public void EquipItem(ItemInfo _item, GameObject _itemobj)
    {
        if (equipItem != null) //이미 장착중인 아이템이 있다면
        {
            ClearItem();
        }
        consumeItemInfo = _item;
        equipItem = _itemobj;
        _item.InitSetData();

        eqItemSlot.GetComponent<Image>().sprite = _item.itemSprite;
        eqItemSlot.SetActive(true);
        eqItemAmount.text = consumeItemInfo.iInfo.amount.ToString();
    }
    public void ClearItem()
    {
        consumeItemInfo = null;
        eqItemSlot.SetActive(false);
        eqItemSlot.GetComponent<Image>().sprite = null;
        eqItemAmount.text = null;
    }
}