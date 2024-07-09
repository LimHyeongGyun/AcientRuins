using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "·Õ¼Òµå";
        itemdata.partNum = 0;
        wStats.weaponNum = 3;
        wStats.needStr = 18;
        wStats.wPower = 22;
    }
}
