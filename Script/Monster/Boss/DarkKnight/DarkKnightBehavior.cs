using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DarkKnightBehavior : MonoBehaviour
{
    //�� ���ͺ� ���� ��ũ��Ʈ ĳ��
    [HideInInspector]
    public MonsterStats stats;

    [HideInInspector]
    public BasicMonster basic;

    [HideInInspector]
    public bool isBattle;

    [SerializeField]
    private int ranAction;

    private int ranN1, ranN2;

    //���� ���� ���� ���� AudioSource
    public AudioClip GruntSound; //���ԼҸ�
    public AudioClip GruntSound2; //���ԼҸ�2

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

    // ���� ���� ����

    IEnumerator Think()
    {
        isBattle = false;
        yield return new WaitForSeconds(0.1f);

        //ranAction �� ����
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

        //���������� ���� ���� �ۿ� �÷��̾ ���� ��� �߰�
        if (atkHits.Length <= 0) 
        {
            //chase�� �����ϴ� ranNum
            ranAction = 0;
        }
        //���� ��Ÿ� �ȿ� ������ ��� �������� ����
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
                //�÷��̾� �߰�
                StartCoroutine(Chase());
                break;
            case 1:
            case 2:
                //����1ȸ���� �����
                StartCoroutine(Attack1());
                break;
            case 3:
                //���ڸ�ȸ�������溣�� ������
                StartCoroutine(Attack2());
                break;
            case 4:
                StartCoroutine(Attack3());
                break;

            //ü�� 50�� ���Ϸ� ������ �� �߰� ��������
            case 5:
            case 6:
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator Chase()//����1ȸ���� �����
    {
        stats.curState = CurrentState.Chase;
        basic.nvAgent.enabled = true;
        basic.nvAgent.isStopped = false;

        basic.anim.SetBool("isChase",true);

        yield return null;
        StartCoroutine(Think());
    }

    IEnumerator Attack1()//����1ȸ���� �����
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack1");

        AttackParam(20, 3, 2.7f, 1.5f, 1f);
        audioSource.clip = GruntSound;
        audioSource.Play();

        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }
    IEnumerator Attack2()//���ڸ�ȸ�������溣�� ������
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack2");

        AttackParam(35, 3, 2.7f, 1.5f, 1f);
        

        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }
    
    IEnumerator Taunt()//������ ���鿡 ���->Ÿ�ֿ̹� ���� �����ϸ� ���� �� ����
    {
        BattleAction();
        basic.anim.SetTrigger("doJump");

        AttackParam(25, 0, 15, 0.05f, 15);  

        audioSource.clip = GruntSound2;
        audioSource.Play();

        yield return new WaitForSeconds(4f);
        StartCoroutine(Think());
    }

    //ü�� 50���� ���Ϸ� ���������� �߰��Ǵ� ��������

    IEnumerator Attack3()//�ֺ� ȸ��������
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
        basic.nvAgent.enabled = false; //navMesh ����
    }

    public void AttackParam(int atkDamage, float atkRange, float atkSize,float atkHeight, float atkLength)
    {
        stats.atkData.atkDamage = atkDamage;
        stats.atkData.atkRange = atkRange;
        stats.atkData.atkSize = atkSize; //�յ�
        stats.atkData.atkHeight = atkHeight; //����
        stats.atkData.atkLength= atkLength; //�¿�
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