using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemData
{
    public enum ItemKind { Weapon, Shield, Consumable}
    public ItemKind itemKind;
    public string itemName;
    public int partNum;
}
public struct WeaponStats
{
    public int weaponNum;
    public int wPower;
    public int needStr;
}
public struct ShiledStats
{
    public int shieldNum;
    public int maxdurability;
    public int durability;
    public bool takeDamage;
    public bool destory;
}
public struct ConsumItem
{
    public enum ConsumeKind { Potion }
    public ConsumeKind consumeKind;
    public int ItemNum;
    public int recoveryHp;
    public int amount;
    public int maxamount;
    public bool repeat;
}

public abstract class ItemInfo : MonoBehaviour
{
    public ItemData itemdata;
    public WeaponStats wStats;
    public ShiledStats sStats;
    public ConsumItem iInfo;

    public Sprite itemSprite;
    //데이터 적용
    public abstract void InitSetData();
}
