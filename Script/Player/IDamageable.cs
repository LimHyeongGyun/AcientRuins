using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public interface IDamageable
{
    public void _DecreaseHp(int damage);
    public void TakeDamage();
    public void Groggy();
    public void Die();
    public void Revive();
}