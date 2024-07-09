using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ToonUndeadBehavior : MonoBehaviour
{
    //�� ���ͺ� ���� ��ũ��Ʈ ĳ��
    //[HideInInspector]
    public MonsterStats stats;

    //[HideInInspector]
    public BasicMonster basic;

    [HideInInspector]
    public bool isBattle;

    [SerializeField]
    private int ranAction;

    private int ranN1, ranN2;

    //PwrUp�Լ����� ���������� �������� ���ݷ�
    [HideInInspector]
    public int addAtkDamage = 0;
    public int increaseValue = 0;

    private bool isBorn;
 
    void Start()
    {
        basic.layerMask = 1 << LayerMask.NameToLayer("Player");

        StartCoroutine(Think());
    }

    // ���� ���� ����

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

        //���������� ���� ���� �ۿ� �÷��̾ ���� ��� �߰�
        if (atkHits.Length <= 0)
        {
            //chase�� �����ϴ� ranNum
            ranAction = 0;
        }
        //���� ��Ÿ� �ȿ� ������ ��� �������� ����
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
                //�÷��̾� �߰�
                StartCoroutine(Chase());
                break;
            case 1:
            case 2:
                //��������
                StartCoroutine(Attack1());
                break;
            case 3:
                //�����ڶ�
                StartCoroutine(Attack2());
                break;
            case 4:
                //�Ŀ���
                StartCoroutine(Attack3());
                break;
            case 5:
                //�극��
                StartCoroutine(Attack4());
                break;
            case 6:
                //�����
                StartCoroutine(Taunt());
                break;

            case 7:
                StartCoroutine(IsBorn());
                    break;
        }
    }

    IEnumerator Chase()//�߰�
    {
        stats.curState = CurrentState.Chase;
        basic.nvAgent.enabled = true;
        basic.nvAgent.isStopped = false;

        basic.anim.SetBool("isChase", true);
        
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Think());
       //Debug.Log("�������Դϴ�");
    }

    IEnumerator Attack1()//���� ����
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack1");

        AttackParam(50 + addAtkDamage, 3, 2.7f, 1.5f, 1f); //25
        yield return new WaitForSeconds(3.6f);
       
        StartCoroutine(Think());
    }
    IEnumerator Attack2()//�����ڶ� - �Ҿ�
    {
        BattleAction();
        basic.anim.SetTrigger("doAttack2");

        AttackParam(30 + addAtkDamage, 3, 2.0f, 1.5f, 1.5f); //50
        
        yield return new WaitForSeconds(3.6f);
        StartCoroutine(Think());
    }

    IEnumerator Taunt()//������ ���鿡 ���->Ÿ�ֿ̹� ���� �����ϸ� ���� �� ����
    {
        BattleAction();
        basic.anim.SetTrigger("doJump");

        AttackParam(30 + addAtkDamage, 0, 15, 0.1f, 15); // 40
        
        yield return new WaitForSeconds(3.1f);
        StartCoroutine(Think());
    }

    IEnumerator Attack3()//�Ŀ���
    {
        increaseValue += 1;
        addAtkDamage += increaseValue;

        BattleAction();
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Attack4()//���̾�극��
    {
        BattleAction();
        AttackParam(15 + addAtkDamage, 0, 5, 0.01f, 5); //40
        basic.anim.SetTrigger("doAttack4");
        yield return new WaitForSeconds(3.3f);
        StartCoroutine(Think());
    }

    IEnumerator IsBorn()//�����Ǿ����� ���� 1ȸ ����
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
        basic.nvAgent.enabled = false; //navMesh ����
    }

    public void AttackParam(int atkDamage, float atkRange, float atkSize, float atkHeight, float atkLength)
    {
        stats.atkData.atkDamage = atkDamage;
        stats.atkData.atkRange = atkRange;
        stats.atkData.atkSize = atkSize; //�յ�
        stats.atkData.atkHeight = atkHeight; //����
        stats.atkData.atkLength = atkLength; //�¿�
    }
}