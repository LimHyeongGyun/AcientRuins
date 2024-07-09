using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFishStats : MonsterStats
{
    void Awake()
    {
        InitSetting();
    }

    public override void InitSetting()
    {
        atkData.atkDamage = 15;
        atkData.atkRange = 1.75f;

        //AtkSize = ���� ���� �� �� ���� �� x��
        //AtkHeigth = ���� ���� �� �Ʒ� ���� y��
        //AtkLength = ���� ���� �� �� �Ÿ� z�����

        atkData.atkSize = 2.0f;
        atkData.atkHeight = 1.5f;
        atkData.atkLength = 2.3f;

        healthData.hp = 40;
        healthData.maxHp = healthData.hp;

        moveData.chaseSpeed = 4.5f;
        moveData.returnSpeed = 3.5f;
        
        scanData.scanRange = 7;
    }
}
