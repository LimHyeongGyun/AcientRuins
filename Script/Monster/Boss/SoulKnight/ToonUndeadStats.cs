using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonUndeadStats : MonsterStats
{
    void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        atkData.atkDamage = 0;
        atkData.atkRange = 0;
        atkData.atkSize = 0;
        atkData.atkHeight = 0;
        atkData.atkLength = 0;

        healthData.hp = 350;
        healthData.maxHp = healthData.hp;

        moveData.chaseSpeed = 7f;
        moveData.returnSpeed = 4.5f;

        scanData.scanRange = 4f;
    }
}