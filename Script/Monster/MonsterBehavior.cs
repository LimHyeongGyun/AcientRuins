using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterBehavior : MonoBehaviour
{
    //�� ���ͺ� ���� ��ũ��Ʈ ĳ��
    public MonsterStats stats;

    public BasicMonster basic;

    private void Start()
    {
        basic = GetComponent<BasicMonster>();
    }
    public abstract void JudgeFight(int layerMask, Transform target, Animator anim, NavMeshAgent nvAgent);

    public void CheckState(ref RaycastHit[] Hits, int layerMask, ref Transform target, ref Transform spawnPoint, Animator anim, NavMeshAgent nvAgent)
    {
        Hits = Physics.SphereCastAll(transform.position, stats.scanData.scanRange, transform.forward, 0, layerMask);
        if (target.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (Hits.Length > 0 && stats.curState != CurrentState.Return)
            {
                target = Hits[0].transform;
            }

            else if (Hits.Length <= 0 || stats.curState == CurrentState.Return)
            {
                target = spawnPoint;
                anim.SetBool("isWalk", true);
                anim.SetBool("isChase", false);

                if (Vector3.Distance(this.transform.position, target.position) < 2.5f)
                {
                    nvAgent.velocity = Vector3.zero;
                    anim.SetBool("Idle", true);
                    anim.SetBool("isWalk", false);
                    stats.curState = CurrentState.Idle;
                    gameObject.layer = 7;
                }
            }
        }

        else if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (stats.curState != CurrentState.Return && Hits.Length > 0 && !basic.isDead) //���ư��� ���߿��� �÷��̾ ��ó�� �͵� Ÿ�������� �ʵ��� �ϴ� ����
                JudgeFight(layerMask, target, anim, nvAgent);

            else if (stats.curState != CurrentState.Return && Hits.Length <= 0 &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !basic.isDead)
            {
                Return(nvAgent, ref target, ref spawnPoint, anim);
            }
        }
    }

    public void Return(NavMeshAgent nvAgent, ref Transform target, ref Transform spawnPoint, Animator anim) 
    {
        target = spawnPoint;
        transform.LookAt(target);
        
        nvAgent.velocity = Vector3.zero;

        nvAgent.speed = stats.moveData.returnSpeed;
        stats.curState = CurrentState.Return;
        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(parameter.name, false);
            }
        }
        
        anim.SetBool("isWalk", true);
        gameObject.layer = 8;
        stats.healthData.hp = stats.healthData.maxHp;
        basic._healthbar.UpdateHealthBar(basic.monster.stats.healthData.maxHp, basic.monster.stats.healthData.hp);
        Debug.Log("������");
    }
}