using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ToonUndeadBehavior : MonoBehaviour
{
    //각 몬스터별 스탯 스크립트 캐싱
    //[HideInInspector]
    public MonsterStats stats;

    //[HideInInspector]
    public BasicMonster basic;

    [HideInInspector]
    public bool isBattle;

    [SerializeField]
    private int ranAction;

    private int ranN1, ranN2;

    //PwrUp함수에서 영구적으로 증가해줄 공격력
    [HideInInspector]
    public int addAtkDamage = 0;
    public int increaseValue = 0;

    private bool isBorn;
 
    void Start()
    {
        basic.layerMask = 1 << LayerMask.NameToLayer("Player");

        StartCoroutine(Think());
    }

    // 보스 전투 패턴

    IEnumerator Think()
    {
        isBattle = false;
        yield return new WaitForSeconds(0.1f);

        ranN1 = 1;
        ranN2 = 7;

        ranAction = Random.Range(ranN1, ranN2);

        //Debug.Log("Layer Mask: " + basic.layerMask);

        RaycastHit[] atkHits = Physics.SphereCastAll(transform.position, stats.scanData.scanRange, transform.forward, 0, basic.layerMask);
        //Debug.Log("Attack Hits Length: " + atkHits.Length);

        //보스몬스터의 공격 범위 밖에 플레이어가 있을 경우 추격
        if (atkHits.Length <= 0)
        {
            //chase를 실행하는 ranNum
            ranAction = 0;
        }
        //공격 사거리 안에 들어왔을 경우 전투패턴 실행
        else if (atkHits.Length > 0 && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            basic.anim.SetBool("isChase", false);
            if (basic.nvAgent.enabled)
            {
                basic.nvAgent.isStopped = true;
            }    
        }

        if (!isBorn)
        {
            ranAction = 7;
        }
        switch (ranAction)
        {
            case 0:
                //플레이어 추격
                StartCoroutine(Chase());
                break;
            case 1:
            case 2:
                //차지공격
                StartCoroutine(Attack1());
                break;
            case 3:
                //근육자랑
                StartCoroutine(Attack2());
                break;
            case 4:
                //파워업
                StartCoroutine(Attack3());
                break;
            case 5:
                //브레스
                StartCoroutine(Attack4());
                break;
            case 6:
                //짓밟기
                StartCoroutine(Taunt());
                break;

            case 7:
                StartCoroutine(IsBorn());
                    break;
        }
    }

    IEnumerator Chase()//추격
    {
        stats.curState = CurrentState.Chase;
        basic.nvAgent.enabled = true;
        basic.nvAgent.isStopped = false;

        basic.anim.SetBool("isChase", true);
        
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Think());
       //Debug.Log("생각중입니다");
    }

    IEnumerator Attack1()//차지 공격
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack1");

        AttackParam(50 + addAtkDamage, 3, 2.7f, 1.5f, 1f); //25
        yield return new WaitForSeconds(3.6f);
       
        StartCoroutine(Think());
    }
    IEnumerator Attack2()//근육자랑 - 불쑈
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack2");

        AttackParam(30 + addAtkDamage, 3, 2.0f, 1.5f, 1.5f); //50
        
        yield return new WaitForSeconds(3.6f);
        StartCoroutine(Think());
    }

    IEnumerator Taunt()//점프후 지면에 충격->타이밍에 맞춰 점프하면 피할 수 있음
    {
        BattleAction();
        basic.anim.SetTrigger("doJump");

        AttackParam(30 + addAtkDamage, 0, 15, 0.1f, 15); // 40
        
        yield return new WaitForSeconds(3.1f);
        StartCoroutine(Think());
    }

    IEnumerator Attack3()//파워업
    {
        increaseValue += 1;
        addAtkDamage += increaseValue;

        BattleAction();
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Attack4()//파이어브레스
    {
        BattleAction();
        AttackParam(15 + addAtkDamage, 0, 5, 0.01f, 5); //40
        basic.anim.SetTrigger("doAttack4");
        yield return new WaitForSeconds(3.3f);
        StartCoroutine(Think());
    }

    IEnumerator IsBorn()//생성되었을때 최초 1회 실행
    {  
        BattleAction();
        basic.anim.SetTrigger("doAttack3");
        yield return new WaitForSeconds(3f);
        isBorn = true;
        StartCoroutine(Think());
    }

    public void BattleAction()
    {
        basic.anim.SetBool("isChase", false);
        stats.curState = CurrentState.Attack;
        isBattle = true;
        basic.nvAgent.enabled = false; //navMesh 끄기
    }

    public void AttackParam(int atkDamage, float atkRange, float atkSize, float atkHeight, float atkLength)
    {
        stats.atkData.atkDamage = atkDamage;
        stats.atkData.atkRange = atkRange;
        stats.atkData.atkSize = atkSize; //앞뒤
        stats.atkData.atkHeight = atkHeight; //높이
        stats.atkData.atkLength = atkLength; //좌우
    }
}