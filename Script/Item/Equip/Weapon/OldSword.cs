using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSword : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "³ì½¼°Ë";
        itemdata.partNum = 0;
        wStats.weaponNum = 0;
        wStats.needStr = 8;
        wStats.wPower = 11;
    }
}
