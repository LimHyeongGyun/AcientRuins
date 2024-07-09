using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonStats : MonsterStats
{
    void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        atkData.atkDamage = 28;
        atkData.atkRange = 1.5f;
        atkData.atkSize = 0.1f;
        atkData.atkHeight = 0.15f;
        atkData.atkLength = 0.08f;

        healthData.hp = 110;
        healthData.maxHp = healthData.hp;

        moveData.chaseSpeed = 5.5f;
        moveData.returnSpeed = 3.5f;

        scanData.scanRange = 8;
    }
}
