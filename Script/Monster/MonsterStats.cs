using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//몬스터 State 관리 변수
public enum CurrentState { Idle, Chase, Attack, Return, Dead, Reflect };

//공격 관련 스텟
public struct AttackStats
{
    public int atkDamage;
    public float atkRange;
    public float atkSize;
    public float atkHeight;
    public float atkLength;
}

//체력 관련 스텟
public struct HealthStats
{
    public int hp;
    public int maxHp;
}

//이동속도 관련 스텟
public struct MovementStats
{
    public float chaseSpeed;
    public float returnSpeed;
}

//인식 관련 스텟
public struct ScanStats
{
    public float scanRange;
}

//몬스터의 스텟, 상태 및 스텟 세팅 메서드 관리
public abstract class MonsterStats : MonoBehaviour
{
    public AttackStats atkData;
    public HealthStats healthData;
    public MovementStats moveData;
    public ScanStats scanData;

    public CurrentState curState;
    //각 몬스터별로 능력치세팅
    public abstract void InitSetting();   
}