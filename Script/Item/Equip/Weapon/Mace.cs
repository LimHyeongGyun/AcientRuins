using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : ItemInfo
{
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Weapon;
        itemdata.itemName = "∏ﬁ¿ÃΩ∫";
        itemdata.partNum = 0;
        wStats.weaponNum = 4;
        wStats.needStr = 24;
        wStats.wPower = 28;
    }
}
