using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerFunc
{
    //���鿡 �پ� �ִ��� �Ǵ�
    public void GroundJudgeMent();
    //��ȣ�ۿ� ����
    public void InteractionSense();
    //������ �ݱ�
    public void AcquireItem();
    //������ ���
    public void UseItem();
    //���׹̳� �Ҹ�
    public void ConsumeStamina();
    //���׹̳� ȸ��
    public void RecoveryStamina();
}