using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteMonster : MonsterBehavior, ITimer, IBattleMode
{
    private float timer = 0f;
    private float interval = 0.8f; // 0.8�ʸ��� �����Ϸ��� interval ���� 0.8�� ����

    System.Action[] actions;

    //���� ���� ���� ���� AudioSource

    public AudioClip BattleCrySound;
    public AudioClip ParryingSound;
    public AudioClip NormalAtkSound;
    public AudioClip PowerAtkSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //AbstractData���� ������ �߻�޼��� override
    #region
    #endregion
    public override void JudgeFight(int layerMask, Transform target, Animator anim, NavMeshAgent nvAgent)
    {
        Timer();
        transform.LookAt(target);

        // ���� ���� ���
        RaycastHit[] atkHits = Physics.SphereCastAll(transform.position, stats.atkData.atkRange + 1f, transform.forward, 0, layerMask);

        bool isInChaseState = anim.GetCurrentAnimatorStateInfo(0).IsName("Chase");
        bool isInIdleState = anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.IsInTransition(0);
        bool isCurrentAnimationFinished = anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;

        // ���� ���� �ۿ� ���� ��
        if (atkHits.Length <= 0 )//|| (stats.curState == CurrentState.Attack && atkHits.Length <= 0))
        {
            nvAgent.isStopped = false;
            actions = new System.Action[] { Chase };
        }

        // ���� �߰� ������ �� ���� ������ ������ �� ���� ��� ����
        // ���� ���� �÷��̾ �־����� ���� ��尡 ������ ���� �ٽ� ���� ���� ���� ������ �ٷ� ���� ��� ����

        // ���� ���� �ȿ� �÷��̾ ������ ��
        else if (atkHits.Length > 0)
        {
            basic.anim.SetBool("isChase", false);
            basic.nvAgent.velocity = Vector3.zero;
            nvAgent.isStopped = true;
            actions = new System.Action[] { PowerAttack, Parry, BattleCry, PowerAttack, NormalAttack, NormalAttack };
        }

        //Ÿ�̸Ӱ� 0�� & ���� �ִϸ��̼��� ��������, Idle ���¿��� �ٷ� ���� ��� ����
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

    //BattleMode �޼���
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
        Debug.Log("���� ü��: " + basic.monster.stats.healthData.hp);
        basic._healthbar.UpdateHealthBar(basic.monster.stats.healthData.maxHp, basic.monster.stats.healthData.hp);
        basic.anim.SetTrigger("doBattleCry");
    }
}