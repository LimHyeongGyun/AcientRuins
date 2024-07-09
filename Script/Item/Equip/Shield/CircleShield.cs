using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShield : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Shield;
        itemdata.itemName = "원형방패";
        itemdata.partNum = 1;
        sStats.shieldNum = 2;
        sStats.durability = 80;
        sStats.maxdurability = 80;
        sStats.takeDamage = false;
        sStats.destory = false;
    }
}
