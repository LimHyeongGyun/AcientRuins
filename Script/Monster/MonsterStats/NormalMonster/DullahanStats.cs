using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DullahanStats : MonsterStats
{
    void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        atkData.atkDamage = 10;
        atkData.atkRange = 1.5f;
        atkData.atkSize = 1.5f;
        atkData.atkHeight = 1.5f;
        atkData.atkLength = 1.5f;

        healthData.hp = 50;
        healthData.maxHp = healthData.hp;

        moveData.chaseSpeed = 4f;
        moveData.returnSpeed = 3.5f;

        scanData.scanRange = 6;
    }
}
