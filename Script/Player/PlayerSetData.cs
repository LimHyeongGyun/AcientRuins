using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetData : PlayerInfo
{
    public void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        stats.maxHp = 100;
        stats.strength = 8;
        stats.power = 1;
        stats.skillPower = stats.power * 2;
        stats.silver = 0;
        stats.acientMemorie = 10;

        stats.healthLv = 1;
        stats.strengthLv = 1;
        stats.powerLv = 1;

        stats.maxStamina = 100;

        stats.walkSpeed = 3f;
        stats.backSpeed = 2f;
        stats.runSpeed = 5f;
        stats.evadeSpeed = 8f;
        stats.jumpHeight = 6f;

        stats.atkLength = 3f;
        stats.atkRange = 2;
        stats.atkHeight = 1.8f;

        stats.defendLength = 1.5f;
        stats.defendRange = 2;
        stats.defendHeight = 3;
    }
}