using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerStats : MonsterStats
{
    void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        atkData.atkDamage = 20;
        atkData.atkRange = 1.5f;
        atkData.atkSize = 1.5f;
        atkData.atkHeight = 1.5f;
        atkData.atkLength = 1.5f;

        healthData.hp = 30;
        healthData.maxHp = healthData.hp;

        moveData.chaseSpeed = 5f;
        moveData.returnSpeed = 3.5f;
        
        scanData.scanRange = 5;
    }
}
