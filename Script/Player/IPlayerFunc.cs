using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerFunc
{
    //지면에 붙어 있는지 판단
    public void GroundJudgeMent();
    //상호작용 감지
    public void InteractionSense();
    //아이템 줍기
    public void AcquireItem();
    //아이템 사용
    public void UseItem();
    //스테미나 소모
    public void ConsumeStamina();
    //스테미나 회복
    public void RecoveryStamina();
}