using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public abstract class PlayerBehavior : MonoBehaviour
{
    public float speed; //속도

    public abstract void WeaponOut(PlayerState player);
    public abstract void WeaponIn(PlayerState player);
    //걷기
    public abstract void Walk(PlayerState state);
    //달리기
    public abstract void Run(PlayerState state);
    //점프
    public abstract void Jump(PlayerState state, JumpState air);
    //추락
    public abstract void Falling(PlayerState air, JumpState state);
    //착지
    public abstract void Landing();
    public abstract void Evade(PlayerState state);
    public abstract void RecoveryHp(PlayerState state);
}
