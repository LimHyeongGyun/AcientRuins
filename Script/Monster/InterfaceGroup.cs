using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �������̽�
public interface IAttack
{
    public void Attack();
}

//ü�� ����, �ǰ� �������̽�
public interface IDamaged
{
    public void DecreaseHp(int damage);

    public void OnDamage();

    public void Die();
}

//�÷��̾� �и��� ������ �� �׷α� �������̽�
public interface IGroggy
{
    public void Groggy();
}

//����Ʈ ���� Ÿ�̸�
public interface ITimer
{
    void Timer();
}

//����Ʈ ���� ������� �������̽�
public interface IBattleMode
{
    public void NormalAttack();
    public void PowerAttack();
    public void Parry();
    public void Chase();
    public void BattleCry();
}
