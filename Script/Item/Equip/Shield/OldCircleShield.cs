using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCircleShield : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Shield;
        itemdata.itemName = "���� ��������";
        itemdata.partNum = 1;
        sStats.shieldNum = 1;
        sStats.durability = 60;
        sStats.maxdurability = 60;
        sStats.takeDamage = false;
        sStats.destory = false;
    }
}
