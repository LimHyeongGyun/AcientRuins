using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStats
{
    //�÷��̾� ����
    public int maxHp; //�ִ�ü��
    public int strength; //��
    public int power;//���ݷ�
    public int skillPower; //������ �����
    public int silver; //��ȭ
    public int acientMemorie; //����� ���
    public int maxStamina; //�ִ뽺�¹̳�

    public int healthLv;
    public int strengthLv;
    public int powerLv;

    public float walkSpeed; //�ȱ� �ӵ�
    public float backSpeed; //�ǵ� �ް����� �ӵ�
    public float runSpeed; //�޸��� �ӵ�
    public float evadeSpeed; //ȸ�� �ӵ�
    public float jumpHeight; //���� ����;

    public float atkLength; //���� ��Ÿ�
    public float atkRange; //���� ����
    public float atkHeight; //���� ����

    public float defendLength; //���� ��Ÿ�
    public float defendRange; //���� ����
    public float defendHeight; //���� ����
}
public abstract class PlayerInfo : MonoBehaviour
{
    public CharacterStats stats;
    public abstract void InitSetting();
}
