using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipData : MonoBehaviour
{
    //shileddata�� ���� �����ֱ�
    //weapondata�� ���� �����ֱ�
    //�κ��丮�� ���� ���� ������ �ִ� ��� ������ ������ ����
    //������ ����� ������ ������ ����
    public ShieldData shieldData;
    public WeaponData weaponData;
    public ConsumeItemData consumeItemData;

    public GameObject[] backWeaponList;
    public GameObject[] handWeaponList;

    public GameObject[] ShieldList;
    public GameObject[] ConsumeItemList;

    public GameObject attackObj;
    public GameObject defendObj;

    //���� ��ü�� ȣ��
    public void EquipWeapon(ItemInfo _weapon)
    {
        weaponData.equip = true;
        ItemInfo backWeapon = backWeaponList[_weapon.wStats.weaponNum].GetComponent<ItemInfo>();
        ItemInfo handWeapon = handWeaponList[_weapon.wStats.weaponNum].GetComponent<ItemInfo>();
        weaponData.EquipWeapon(backWeapon, handWeapon);
    }
    //���� ������ ȣ��
    public void EquipShield(ItemInfo _shield)
    {
        shieldData.equip = true;
        shieldData.EquipShield(_shield, ShieldList[_shield.sStats.shieldNum]);
    }
    //�Һ������ ������ ȣ��
    public void EquipConsumableItem(ItemInfo _item)
    {
        consumeItemData.equip = true;
        consumeItemData.EquipItem(_item, ConsumeItemList[_item.iInfo.ItemNum]);
    }
}