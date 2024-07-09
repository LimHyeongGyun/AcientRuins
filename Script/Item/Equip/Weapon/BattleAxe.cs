using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAxe : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "πË∆≤ø¢Ω∫";
        itemdata.partNum = 0;
        wStats.weaponNum = 1;
        wStats.needStr = 10;
        wStats.wPower = 15;
    }
}