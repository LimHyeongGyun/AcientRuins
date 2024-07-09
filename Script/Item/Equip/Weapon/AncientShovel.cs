using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientShovel : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "°í´ëÀÇ »ð";
        itemdata.partNum = 0;
        wStats.weaponNum = 6;
        wStats.needStr = 0;
        wStats.wPower = 666;
    }
}
