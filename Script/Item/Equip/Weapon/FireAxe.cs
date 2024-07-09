using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAxe : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "�ҹ浵��";
        itemdata.partNum = 0;
        wStats.weaponNum = 5;
        wStats.needStr = 35;
        wStats.wPower = 38;
    }
}
