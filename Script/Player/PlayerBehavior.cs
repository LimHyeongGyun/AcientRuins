using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public abstract class PlayerBehavior : MonoBehaviour
{
    public float speed; //�ӵ�

    public abstract void WeaponOut(PlayerState player);
    public abstract void WeaponIn(PlayerState player);
    //�ȱ�
    public abstract void Walk(PlayerState state);
    //�޸���
    public abstract void Run(PlayerState state);
    //����
    public abstract void Jump(PlayerState state, JumpState air);
    //�߶�
    public abstract void Falling(PlayerState air, JumpState state);
    //����
    public abstract void Landing();
    public abstract void Evade(PlayerState state);
    public abstract void RecoveryHp(PlayerState state);
}
