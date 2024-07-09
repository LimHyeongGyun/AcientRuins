using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DarkKnightBehavior : MonoBehaviour
{
    //각 몬스터별 스탯 스크립트 캐싱
    [HideInInspector]
    public MonsterStats stats;

    [HideInInspector]
    public BasicMonster basic;

    [HideInInspector]
    public bool isBattle;

    [SerializeField]
    private int ranAction;

    private int ranN1, ranN2;

    //공격 사운드 구현 위한 AudioSource
    public AudioClip GruntSound; //고함소리
    public AudioClip GruntSound2; //고함소리2

    public AudioSource audioSource;

    private void Update()
    {
        AnimSetFloat();
    }

    void Start()
    {
        stats = GetComponent<MonsterStats>();
        basic = GetComponent<BasicMonster>();
        StartCoroutine(Think());
    }

    // 보스 전투 패턴

    IEnumerator Think()
    {
        isBattle = false;
        yield return new WaitForSeconds(0.1f);

        //ranAction 값 설정
        if (stats.healthData.hp >= (stats.healthData.maxHp / 2))
        {
            ranN1 = 1;
            ranN2 = 7;
        }
        else
        {
            ranN1 = 1;
            ranN2 = 7;

        }
        ranAction = Random.Range(ranN1, ranN2);

        RaycastHit[] atkHits = Physics.SphereCastAll(transform.position, stats.scanData.scanRange, transform.forward, 0, basic.layerMask);

        //보스몬스터의 공격 범위 밖에 플레이어가 있을 경우 추격
        if (atkHits.Length <= 0) 
        {
            //chase를 실행하는 ranNum
            ranAction = 0;
        }
        //공격 사거리 안에 들어왔을 경우 전투패턴 실행
        else if (atkHits.Length > 0) 
        {
            if (basic.nvAgent.enabled)
            {
                basic.nvAgent.isStopped = true;
            }
            basic.anim.SetBool("isChase", false);
        }      

        switch (ranAction)
        {
            case 0:
                //플레이어 추격
                StartCoroutine(Chase());
                break;
            case 1:
            case 2:
                //전방1회베기 약공격
                StartCoroutine(Attack1());
                break;
            case 3:
                //제자리회전후전방베기 강공격
                StartCoroutine(Attack2());
                break;
            case 4:
                StartCoroutine(Attack3());
                break;

            //체력 50퍼 이하로 떨어질 시 추가 공격패턴
            case 5:
            case 6:
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator Chase()//전방1회베기 약공격
    {
        stats.curState = CurrentState.Chase;
        basic.nvAgent.enabled = true;
        basic.nvAgent.isStopped = false;

        basic.anim.SetBool("isChase",true);

        yield return null;
        StartCoroutine(Think());
    }

    IEnumerator Attack1()//전방1회베기 약공격
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack1");

        AttackParam(20, 3, 2.7f, 1.5f, 1f);
        audioSource.clip = GruntSound;
        audioSource.Play();

        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }
    IEnumerator Attack2()//제자리회전후전방베기 강공격
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack2");

        AttackParam(35, 3, 2.7f, 1.5f, 1f);
        

        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }
    
    IEnumerator Taunt()//점프후 지면에 충격->타이밍에 맞춰 점프하면 피할 수 있음
    {
        BattleAction();
        basic.anim.SetTrigger("doJump");

        AttackParam(25, 0, 15, 0.05f, 15);  

        audioSource.clip = GruntSound2;
        audioSource.Play();

        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }

    //체력 50프로 이하로 떨어졌을때 추가되는 전투로직

    IEnumerator Attack3()//주변 회오리베기
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack3");

        AttackParam(15, 0, 3.5f, 0.01f, 3.5f); //40
        
        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }

    public void BattleAction()
    {
        basic.anim.SetFloat("multi", 1);
        basic.anim.SetBool("isChase", false);
        stats.curState = CurrentState.Attack;
        isBattle = true;
        basic.nvAgent.enabled = false; //navMesh 끄기
    }

    public void AttackParam(int atkDamage, float atkRange, float atkSize,float atkHeight, float atkLength)
    {
        stats.atkData.atkDamage = atkDamage;
        stats.atkData.atkRange = atkRange;
        stats.atkData.atkSize = atkSize; //앞뒤
        stats.atkData.atkHeight = atkHeight; //높이
        stats.atkData.atkLength= atkLength; //좌우
    }

    void AnimSetFloat()
    {
        if (basic.anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack") && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.12f)
            basic.anim.SetFloat("multi", 2);
        else if (basic.anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack") && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
            basic.anim.SetFloat("multi", 2);
        else if(basic.anim.GetCurrentAnimatorStateInfo(0).IsName("Taunt") && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            basic.anim.SetFloat("multi", 1);
        else if(basic.anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack") && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.43f)
            basic.anim.SetFloat("multi", 1.5f);
        else if(basic.anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack") && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            basic.anim.SetFloat("multi", 6);
    }
}