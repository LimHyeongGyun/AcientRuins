using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeBlood : ItemInfo
{
    public int amount;
    private void Start()
    {
        SetAmount();
    }
    public void SetAmount()
    {
        iInfo.amount = amount;
    }
    public override void InitSetData()
    {
        itemdata.itemKind = ItemData.ItemKind.Consumable;
        iInfo.consumeKind = ConsumItem.ConsumeKind.Potion;

        itemdata.itemName = "드레이크의 피";
        itemdata.partNum = 2;
        iInfo.ItemNum = 0;
        iInfo.recoveryHp = 30;
        iInfo.maxamount = 5;
    }
}