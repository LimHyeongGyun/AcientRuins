using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikingShield : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Shield;
        itemdata.itemName = "바이킹 방패";
        itemdata.partNum = 1;
        sStats.shieldNum = 3;
        sStats.durability = 100;
        sStats.maxdurability = 100;
        sStats.takeDamage = false;
        sStats.destory = false;
    }
}
