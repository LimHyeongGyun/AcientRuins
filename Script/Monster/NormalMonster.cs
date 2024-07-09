using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalMonster : MonsterBehavior
{

    public override void JudgeFight(int layerMask, Transform target, Animator anim, NavMeshAgent nvAgent)
    {
        RaycastHit[] atkHits = Physics.SphereCastAll(transform.position, stats.atkData.atkRange, transform.forward, 0, layerMask);
        
        if (atkHits.Length <= 0) //atkrange < �÷��̾� �Ÿ� < scanRange
        {
            stats.curState = CurrentState.Chase;
            anim.SetBool("isChase", true);
            anim.SetBool("isAttack", false);
            nvAgent.speed = stats.moveData.chaseSpeed;
        }

        else if (atkHits.Length > 0) //���� ��Ÿ� �ȿ� ��������
        {
            transform.LookAt(target);
            stats.curState = CurrentState.Attack;
            //nvAgent.velocity = Vector3.zero; // ���� �߰ݿ��� ���� ��ȯ�� �̲����� ����
            anim.SetBool("isAttack", true);
            anim.SetBool("isChase", false);
        }
    }
}