using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHammer : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "¹¨´Ï¸£";
        itemdata.partNum = 0;
        wStats.weaponNum = 2;
        wStats.needStr = 13;
        wStats.wPower = 17;
    }
}
