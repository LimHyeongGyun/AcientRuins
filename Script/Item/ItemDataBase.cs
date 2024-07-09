using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : SingleTon<ItemDataBase>
{
    public List<ItemInfo> Weapon = new List<ItemInfo>();
    public List<ItemInfo> Shield = new List<ItemInfo>();
    public List<ItemInfo> ConsumeItem = new List<ItemInfo>();

    private void Awake()
    {
        foreach (ItemInfo item in Weapon)
        {
            item.InitSetData();
        }
        foreach (ItemInfo item in Shield)
        {
            item.InitSetData();
        }
        foreach (ItemInfo item in ConsumeItem)
        {
            item.InitSetData();
        }
    }
}
