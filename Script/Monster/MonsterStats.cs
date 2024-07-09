using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� State ���� ����
public enum CurrentState { Idle, Chase, Attack, Return, Dead, Reflect };

//���� ���� ����
public struct AttackStats
{
    public int atkDamage;
    public float atkRange;
    public float atkSize;
    public float atkHeight;
    public float atkLength;
}

//ü�� ���� ����
public struct HealthStats
{
    public int hp;
    public int maxHp;
}

//�̵��ӵ� ���� ����
public struct MovementStats
{
    public float chaseSpeed;
    public float returnSpeed;
}

//�ν� ���� ����
public struct ScanStats
{
    public float scanRange;
}

//������ ����, ���� �� ���� ���� �޼��� ����
public abstract class MonsterStats : MonoBehaviour
{
    public AttackStats atkData;
    public HealthStats healthData;
    public MovementStats moveData;
    public ScanStats scanData;

    public CurrentState curState;
    //�� ���ͺ��� �ɷ�ġ����
    public abstract void InitSetting();   
}