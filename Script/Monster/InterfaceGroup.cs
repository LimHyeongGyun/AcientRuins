using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격 인터페이스
public interface IAttack
{
    public void Attack();
}

//체력 감소, 피격 인터페이스
public interface IDamaged
{
    public void DecreaseHp(int damage);

    public void OnDamage();

    public void Die();
}

//플레이어 패링에 막혔을 시 그로기 인터페이스
public interface IGroggy
{
    public void Groggy();
}

//엘리트 몬스터 타이머
public interface ITimer
{
    void Timer();
}

//엘리트 몬스터 전투모드 인터페이스
public interface IBattleMode
{
    public void NormalAttack();
    public void PowerAttack();
    public void Parry();
    public void Chase();
    public void BattleCry();
}
