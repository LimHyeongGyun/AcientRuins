using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurStats : MonsterStats
{
    void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        atkData.atkDamage = 30;
        atkData.atkRange = 0.8f;
        atkData.atkSize = 2.0f;
        atkData.atkHeight = 1.5f;
        atkData.atkLength = 1.5f;

        healthData.hp = 120;
        healthData.maxHp = healthData.hp;

        moveData.chaseSpeed = 5.5f;
        moveData.returnSpeed = 3.5f;

        scanData.scanRange = 8.5f;
    }
}
