using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStats
{
    //플레이어 스탯
    public int maxHp; //최대체력
    public int strength; //힘
    public int power;//공격력
    public int skillPower; //강공격 대미지
    public int silver; //재화
    public int acientMemorie; //고대의 기억
    public int maxStamina; //최대스태미나

    public int healthLv;
    public int strengthLv;
    public int powerLv;

    public float walkSpeed; //걷기 속도
    public float backSpeed; //실드 뒷걸음질 속도
    public float runSpeed; //달리기 속도
    public float evadeSpeed; //회피 속도
    public float jumpHeight; //점프 높이;

    public float atkLength; //공격 사거리
    public float atkRange; //공격 범위
    public float atkHeight; //공격 높이

    public float defendLength; //공격 사거리
    public float defendRange; //공격 범위
    public float defendHeight; //공격 높이
}
public abstract class PlayerInfo : MonoBehaviour
{
    public CharacterStats stats;
    public abstract void InitSetting();
}
