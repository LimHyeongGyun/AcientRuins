using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorShield : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Shield;
        itemdata.itemName = "ºÎ½¤Áø ¹®";
        itemdata.partNum = 1;
        sStats.shieldNum = 0;
        sStats.durability = 20;
        sStats.maxdurability = 20;
        sStats.takeDamage = true;
        sStats.destory = false;
    }
}
