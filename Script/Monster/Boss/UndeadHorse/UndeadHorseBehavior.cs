using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UndeadHorseBehavior : MonoBehaviour
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
    //basic.anim = 말 anim
    public Animator riderAnim; //기사 anim

    public GameObject rider; //2페이즈 기사


    //PwrUp함수에서 영구적으로 증가해줄 공격력
    [HideInInspector]
    public int addAtkDamage = 0;
    public int increaseValue = 0;

    private bool isRide;

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

        if (stats.healthData.hp >= (stats.healthData.maxHp / 2))
        {
            ranN1 = 1;
            ranN2 = 5;
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
        else if (atkHits.Length > 0 && basic.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) 
        {
            if (basic.nvAgent.enabled)
            {
                basic.nvAgent.isStopped = true;
            }
            basic.anim.SetBool("isChase", false);
            riderAnim.SetBool("isChase", false);  
        }
        if(stats.healthData.hp < (stats.healthData.maxHp / 2))
        {
            if (!isRide)
            {
                ranN1 = 5;
                ranN2 = 6;
            }
        }

        switch (ranAction)
        {
            case 0:
                //플레이어 추격
                StartCoroutine(Chase());
                break;
            case 1:
                //화살5개 투사체로 투척
                StartCoroutine(SwordSlash());
                break;
            case 2:
                //점프후 지면에 충격
                StartCoroutine(Taunt());
                break;
            case 3:
                //파워업
                StartCoroutine(PowerUp());
                break;
            case 4:
                //유도탄 발사
                StartCoroutine(SwordCasting());
                break;
            case 5:
                //기사 교체
                StartCoroutine(DisMount());
                break;
        }
    }

    IEnumerator Chase()
    {
        stats.curState = CurrentState.Chase;
        basic.nvAgent.enabled = true;
        basic.nvAgent.isStopped = false;

        basic.anim.SetBool("isChase",true);
        riderAnim.SetBool("isChase", true);
        yield return null;   
        StartCoroutine(Think());
    }

    IEnumerator SwordSlash()//화살5개 투사체로 투척
    {
        BattleAction();
        AttackParam(5 + addAtkDamage, 2.5f, 1, 1, 1.5f);
        basic.anim.SetTrigger("doAttack1");
        riderAnim.SetTrigger("doAttack1");
        
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Taunt()//점프후 지면에 충격->타이밍에 맞춰 점프하면 피할 수 있음
    {
        BattleAction();
        basic.anim.SetTrigger("doJump");
        riderAnim.SetTrigger("doJump");
        AttackParam(40 + addAtkDamage, 5, 15, 0.4f, 15); //40
        yield return new WaitForSeconds(1.2f);
        yield return new WaitForSeconds(1.8f);
        
        StartCoroutine(Think());
    }

    //체력 50프로 이하로 떨어졌을때 추가되는 전투로직

    IEnumerator SwordCasting()//유도탄 발사
    {
        BattleAction();
        AttackParam(6 + addAtkDamage, 0, 3, 1, 3);
        basic.anim.SetTrigger("doAttack3");
        riderAnim.SetTrigger("doAttack3");

        yield return new WaitForSeconds(5f);
        StartCoroutine(Think());
    }

    IEnumerator PowerUp() //공격력 영구증가
    {
        increaseValue += 1;
        addAtkDamage += increaseValue;
        
        BattleAction();
        AttackParam(15 + addAtkDamage, 4f, 1, 1, 1.5f);
        basic.anim.SetTrigger("doAttack4");
        riderAnim.SetTrigger("doAttack4");

        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator DisMount()
    {
        //Debug.Log("고생했네");
        basic.nvAgent.enabled = false;
        basic.anim.SetTrigger("doDisMount");
        riderAnim.SetTrigger("doDisMount");
        isRide = true;

        yield return new WaitForSeconds(2f);

        Instantiate(rider, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject); //말타고있는 기사 삭제 

         //말 내리는 시간만큼 기다리기
    }
    public void BattleAction()
    {
        basic.anim.SetBool("isChase", false);
        stats.curState = CurrentState.Attack;
        isBattle = true;
        basic.nvAgent.enabled = false;
    }

    public void AttackParam(int atkDamage, float atkRange, float atkSize,float atkHeight, float atkLength)
    {
        stats.atkData.atkDamage = atkDamage;
        stats.atkData.atkRange = atkRange;
        stats.atkData.atkSize = atkSize;
        stats.atkData.atkHeight = atkHeight;
        stats.atkData.atkLength= atkLength;
    }
}