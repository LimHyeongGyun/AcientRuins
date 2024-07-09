using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UndeadHorseBehavior : MonoBehaviour
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
    //basic.anim = �� anim
    public Animator riderAnim; //��� anim

    public GameObject rider; //2������ ���


    //PwrUp�Լ����� ���������� �������� ���ݷ�
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

    // ���� ���� ����

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

        //���������� ���� ���� �ۿ� �÷��̾ ���� ��� �߰�
        if (atkHits.Length <= 0) 
        {
            //chase�� �����ϴ� ranNum
            ranAction = 0;
        }
        //���� ��Ÿ� �ȿ� ������ ��� �������� ����
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
                //�÷��̾� �߰�
                StartCoroutine(Chase());
                break;
            case 1:
                //ȭ��5�� ����ü�� ��ô
                StartCoroutine(SwordSlash());
                break;
            case 2:
                //������ ���鿡 ���
                StartCoroutine(Taunt());
                break;
            case 3:
                //�Ŀ���
                StartCoroutine(PowerUp());
                break;
            case 4:
                //����ź �߻�
                StartCoroutine(SwordCasting());
                break;
            case 5:
                //��� ��ü
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

    IEnumerator SwordSlash()//ȭ��5�� ����ü�� ��ô
    {
        BattleAction();
        AttackParam(5 + addAtkDamage, 2.5f, 1, 1, 1.5f);
        basic.anim.SetTrigger("doAttack1");
        riderAnim.SetTrigger("doAttack1");
        
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Taunt()//������ ���鿡 ���->Ÿ�ֿ̹� ���� �����ϸ� ���� �� ����
    {
        BattleAction();
        basic.anim.SetTrigger("doJump");
        riderAnim.SetTrigger("doJump");
        AttackParam(40 + addAtkDamage, 5, 15, 0.4f, 15); //40
        yield return new WaitForSeconds(1.2f);
        yield return new WaitForSeconds(1.8f);
        
        StartCoroutine(Think());
    }

    //ü�� 50���� ���Ϸ� ���������� �߰��Ǵ� ��������

    IEnumerator SwordCasting()//����ź �߻�
    {
        BattleAction();
        AttackParam(6 + addAtkDamage, 0, 3, 1, 3);
        basic.anim.SetTrigger("doAttack3");
        riderAnim.SetTrigger("doAttack3");

        yield return new WaitForSeconds(5f);
        StartCoroutine(Think());
    }

    IEnumerator PowerUp() //���ݷ� ��������
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
        //Debug.Log("����߳�");
        basic.nvAgent.enabled = false;
        basic.anim.SetTrigger("doDisMount");
        riderAnim.SetTrigger("doDisMount");
        isRide = true;

        yield return new WaitForSeconds(2f);

        Instantiate(rider, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject); //��Ÿ���ִ� ��� ���� 

         //�� ������ �ð���ŭ ��ٸ���
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