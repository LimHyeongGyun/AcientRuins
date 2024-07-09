using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private EquipData eqData;

    public ItemInfo itemInfo;
    public Image itemImg;
    public Text itemName;
    public Image[] info_Img;
    public Text itemInfo_1;
    public Text itemInfo_2;
    public Text itemInfo_3;

    public Outline outLine;

    public void Awake()
    {
        eqData = FindObjectOfType<EquipData>();
    }
    public void GetInformation(ItemInfo _item, int _info1, int _info2, int _itemNo)
    {
        itemInfo = _item;
        itemImg.sprite = _item.itemSprite;
        itemName.text = _item.itemdata.itemName;
        itemInfo_1.text = _info1.ToString();
        itemInfo_2.text = _info2.ToString();
        if (_item.itemdata.itemKind == ItemData.ItemKind.Shield)
        {
            itemInfo_2.text = "미파괴";
        }
        itemInfo_3.text = "No." + _itemNo.ToString();
    }

    public void SelectSlot()
    {
        if (gameObject.CompareTag("WeaponSlot"))
        {
            SelectWeapon();
        }
        else if (gameObject.CompareTag("ShieldSlot"))
        {
            SelectShield();
        }
        else if (gameObject.CompareTag("ConsumeItemSlot"))
        {
            SelectItem();
        }
    }
    public void SelectWeapon()
    {
        this.gameObject.GetComponentInParent<InventoryUI>().EquipWeapon();
        outLine.enabled = true;
        this.outLine.effectColor = Color.red;
        eqData.EquipWeapon(itemInfo);
    }
    public void SelectShield()
    {
        if (!itemInfo.sStats.destory && itemInfo.sStats.durability != 0) //내구도가 0이 아니거나 방패가 파괴되지 않은 상태일 때만 장착 가능
        {
            this.gameObject.GetComponentInParent<InventoryUI>().EquipShield();
            outLine.enabled = true;
            this.outLine.effectColor = Color.red;
            eqData.EquipShield(itemInfo);
        }
    }
    public void SelectItem()
    {
        this.gameObject.GetComponentInParent<InventoryUI>().EquipConsumeableItem();
        outLine.enabled = true;
        this.outLine.effectColor = Color.red;
        eqData.EquipConsumableItem(itemInfo);
    }
}
