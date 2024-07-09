using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalMonster : MonsterBehavior
{

    public override void JudgeFight(int layerMask, Transform target, Animator anim, NavMeshAgent nvAgent)
    {
        RaycastHit[] atkHits = Physics.SphereCastAll(transform.position, stats.atkData.atkRange, transform.forward, 0, layerMask);
        
        if (atkHits.Length <= 0) //atkrange < 플레이어 거리 < scanRange
        {
            stats.curState = CurrentState.Chase;
            anim.SetBool("isChase", true);
            anim.SetBool("isAttack", false);
            nvAgent.speed = stats.moveData.chaseSpeed;
        }

        else if (atkHits.Length > 0) //공격 사거리 안에 들어왔을때
        {
            transform.LookAt(target);
            stats.curState = CurrentState.Attack;
            //nvAgent.velocity = Vector3.zero; // 몬스터 추격에서 공격 전환시 미끄러짐 방지
            anim.SetBool("isAttack", true);
            anim.SetBool("isChase", false);
        }
    }
}