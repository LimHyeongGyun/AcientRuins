using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefend : MonoBehaviour
{
    private HashSet<MonsterAttack> enteredAtk = new HashSet<MonsterAttack>();

    private Player player;
    private ShieldData shieldData;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        shieldData = FindObjectOfType<ShieldData>();
    }
    void Update()
    {
        DestroyObj();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterAttack"))
        {
            //���� ������ ����
            if (gameObject.CompareTag("PlayerParry")) //������Ʈ�� �и� ������ ��
            {
                var monsterAtk = other.gameObject.GetComponent<MonsterAttack>();
                if (!enteredAtk.Contains(monsterAtk))
                {
                    enteredAtk.Add(monsterAtk);
                    BasicMonster mon = other.transform.parent.GetComponent<BasicMonster>(); //���� ���� ������Ʈ�� �θ������Ʈ(����) ���� ����
                    //�������Ͱ� �ƴ� ��
                    if (mon.monType != BasicMonster.MonsterType.BossMonster)
                    {
                        mon.Groggy(); //���� ���� �׷α���� ���� �ż��� ȣ��
                    }
                    //���������϶�
                    else if (mon.monType != BasicMonster.MonsterType.BossMonster)
                    {
                        shieldData.destroy = true;//���� �ı�
                    }
                    mon.Groggy(); //���� ���� �߰��Ǹ� �����
                    shieldData.ShieldDurabilityManageMent(monsterAtk.atkPower); //�ǵ� ������ ����
                    if (monsterAtk != null) //���� ������ �ı����� �ʾ����� �ı�
                    {
                        Destroy(monsterAtk);
                    }
                }
            }
            else if (gameObject.CompareTag("PlayerGuard")) //������Ʈ�� ���� ���� �� ��
            {
                var monsterAtk = other.gameObject.GetComponent<MonsterAttack>();
                BasicMonster mon = monsterAtk.GetComponentInParent<BasicMonster>();
                if (mon.monType == BasicMonster.MonsterType.BossMonster)
                {
                    shieldData.destroy = true;//���� �ı�
                }
                else if (mon.monType != BasicMonster.MonsterType.BossMonster)
                {
                    //ƨ���� ������ �׼�
                    mon.anim.SetTrigger("isCrash");
                }
                shieldData.ShieldDurabilityManageMent(monsterAtk.atkPower);
            }
        }
    }
    private void DestroyObj()
    {
        if (gameObject.CompareTag("PlayerParry")) //�и��� ���
        {
            if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("Parry"))
            {
                if(player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.36f) //���а� �÷��̾� ĳ���� ������ �����־��� ��
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (gameObject.CompareTag("PlayerGuard")) //���� ������ ���
        {
            if (!player.guard) //���� ������
            {
                Destroy(gameObject); //�ı�
            }
        }
    }
}
