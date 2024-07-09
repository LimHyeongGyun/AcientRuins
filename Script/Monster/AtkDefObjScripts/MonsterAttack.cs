using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private HashSet<Player> touchPlayer = new HashSet<Player>(); //������ �ߺ� �������� �ڵ�

    public enum AtkType { NormalAtk, ProjectileAtk }
    public AtkType atkType;

    //�� ������Ʈ�� ����ϴ� ������ ���ݷ��� �޾ƿ;���
    [HideInInspector]
    public int atkPower;

    public void Update()
    {
        ////�𵥵庸�� ����ü�� �����ϴ� ����
        if (atkType != AtkType.ProjectileAtk)
            DestroyObj(0.1f);
        else
        {
            DestroyObj(3f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGuard") || other.CompareTag("PlayerParry"))
        {
            var player = other.GetComponentInParent<Player>();
            if (!touchPlayer.Contains(player))
            {
                //Debug.Log("���п� �ε���");
                touchPlayer.Add(player);              
                Destroy(this.gameObject,0.1f); //0.01�ʴ� �ȸ��� ����?
            }
        }
        //���� ������Ʈ�� �÷��̾�� ����� ��
        //�������� �ʵ� ���� ������Ʈ ������� ������� ���� �߰��ϱ�
        else if (other.CompareTag("Player") && (!other.CompareTag("PlayerGuard") || !other.CompareTag("PlayerParry")))
        {
            //���� �÷��̾ player������ �־��ְ�
            var player = other.GetComponent<Player>();
            if (!touchPlayer.Contains(player))
            {
                touchPlayer.Add(player);
                if (player.guardState != Player.GuardState.Unbeatable)
                {
                    player._DecreaseHp(atkPower);
                }               
                Destroy(this.gameObject);
            }
        }
    }

    private void DestroyObj(float time)
    {
        if(time == 0.1f)
            Destroy(gameObject,0.1f);
        else if(time == 3)
            Destroy(gameObject, 1.7f);
    }
}