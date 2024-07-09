using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public interface IAttackFunc
{
    public void Attack(PlayerState attack, PlayerState skill);
    public void AttackFunc();
}
