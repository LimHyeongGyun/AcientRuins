using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipData : MonoBehaviour
{
    //shileddata로 정보 보내주기
    //weapondata로 정보 보내주기
    //인벤토리를 통해 현재 가지고 있는 장비 정보를 가지고 있음
    //장착한 장비의 정보를 가지고 있음
    public ShieldData shieldData;
    public WeaponData weaponData;
    public ConsumeItemData consumeItemData;

    public GameObject[] backWeaponList;
    public GameObject[] handWeaponList;

    public GameObject[] ShieldList;
    public GameObject[] ConsumeItemList;

    public GameObject attackObj;
    public GameObject defendObj;

    //무기 교체시 호출
    public void EquipWeapon(ItemInfo _weapon)
    {
        weaponData.equip = true;
        ItemInfo backWeapon = backWeaponList[_weapon.wStats.weaponNum].GetComponent<ItemInfo>();
        ItemInfo handWeapon = handWeaponList[_weapon.wStats.weaponNum].GetComponent<ItemInfo>();
        weaponData.EquipWeapon(backWeapon, handWeapon);
    }
    //방패 장착시 호출
    public void EquipShield(ItemInfo _shield)
    {
        shieldData.equip = true;
        shieldData.EquipShield(_shield, ShieldList[_shield.sStats.shieldNum]);
    }
    //소비아이템 장착시 호출
    public void EquipConsumableItem(ItemInfo _item)
    {
        consumeItemData.equip = true;
        consumeItemData.EquipItem(_item, ConsumeItemList[_item.iInfo.ItemNum]);
    }
}