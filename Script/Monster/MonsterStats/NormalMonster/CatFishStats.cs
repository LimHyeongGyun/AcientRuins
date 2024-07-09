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

        //AtkSize = 몬스터 기준 좌 우 공격 폭 x축
        //AtkHeigth = 몬스터 기준 위 아래 길이 y축
        //AtkLength = 몬스터 기준 앞 뒤 거리 z축길이

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
