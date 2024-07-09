using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteMonster : MonsterBehavior, ITimer, IBattleMode
{
    private float timer = 0f;
    private float interval = 0.8f; // 0.8초마다 실행하려면 interval 값을 0.8로 설정

    System.Action[] actions;

    //공격 사운드 구현 위한 AudioSource

    public AudioClip BattleCrySound;
    public AudioClip ParryingSound;
    public AudioClip NormalAtkSound;
    public AudioClip PowerAtkSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //AbstractData에서 선언한 추상메서드 override
    #region
    #endregion
    public override void JudgeFight(int layerMask, Transform target, Animator anim, NavMeshAgent nvAgent)
    {
        Timer();
        transform.LookAt(target);

        // 공격 범위 계산
        RaycastHit[] atkHits = Physics.SphereCastAll(transform.position, stats.atkData.atkRange + 1f, transform.forward, 0, layerMask);

        bool isInChaseState = anim.GetCurrentAnimatorStateInfo(0).IsName("Chase");
        bool isInIdleState = anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.IsInTransition(0);
        bool isCurrentAnimationFinished = anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;

        // 공격 범위 밖에 있을 때
        if (atkHits.Length <= 0 )//|| (stats.curState == CurrentState.Attack && atkHits.Length <= 0))
        {
            nvAgent.isStopped = false;
            actions = new System.Action[] { Chase };
        }

        // 현재 추격 상태일 때 공격 범위에 들어왔을 때 전투 모드 실행
        // 전투 도중 플레이어가 멀어져도 전투 모드가 끝나기 전에 다시 공격 범위 내에 들어오면 바로 전투 모드 실행

        // 공격 범위 안에 플레이어가 들어왔을 때
        else if (atkHits.Length > 0)
        {
            basic.anim.SetBool("isChase", false);
            basic.nvAgent.velocity = Vector3.zero;
            nvAgent.isStopped = true;
            actions = new System.Action[] { PowerAttack, Parry, BattleCry, PowerAttack, NormalAttack, NormalAttack };
        }

        //타이머가 0초 & 현재 애니메이션이 끝났을때, Idle 상태에서 바로 전투 모드 실행
        if ((timer == 0 && isCurrentAnimationFinished) || (timer == 0 && isInIdleState) || (timer == 0 && isInChaseState))
        {
            int randomIndex = Random.Range(0, actions.Length);
            actions[randomIndex]?.Invoke();
        }
    }

    public void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
        }
    }

    //BattleMode 메서드
    #region

    public void Chase()
    {
        stats.curState = CurrentState.Chase;
        basic.anim.SetBool("isChase", true);
        basic.nvAgent.speed = stats.moveData.chaseSpeed;
    }

    public void NormalAttack()
    {
        audioSource.clip = NormalAtkSound;
        audioSource.Play();
        stats.curState = CurrentState.Attack;
        basic.monster.stats.atkData.atkDamage = 1;
        basic.anim.SetTrigger("doNormAtk");
    }
    public void PowerAttack()
    {
        audioSource.clip = PowerAtkSound;
        audioSource.Play();
        stats.curState = CurrentState.Attack;
        basic.monster.stats.atkData.atkDamage = 1;
        basic.anim.SetTrigger("doPwrAtk");
    }

    public void Parry()
    {
        audioSource.clip = ParryingSound;
        audioSource.Play();
        stats.curState = CurrentState.Attack;   
        basic.anim.SetTrigger("doParry");
    }

   
    #endregion

    public void BattleCry()
    {
        audioSource.clip = BattleCrySound;
        audioSource.Play();

        stats.curState = CurrentState.Attack;
        basic.monster.stats.healthData.hp += 11;
        if (basic.monster.stats.healthData.hp > basic.monster.stats.healthData.maxHp)
            basic.monster.stats.healthData.hp = basic.monster.stats.healthData.maxHp;
        Debug.Log("현재 체력: " + basic.monster.stats.healthData.hp);
        basic._healthbar.UpdateHealthBar(basic.monster.stats.healthData.maxHp, basic.monster.stats.healthData.hp);
        basic.anim.SetTrigger("doBattleCry");
    }
}