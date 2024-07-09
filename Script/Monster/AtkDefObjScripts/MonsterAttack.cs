using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private HashSet<Player> touchPlayer = new HashSet<Player>(); //데미지 중복 방지위한 코드

    public enum AtkType { NormalAtk, ProjectileAtk }
    public AtkType atkType;

    //이 오브젝트를 사용하는 몬스터의 공격력을 받아와야함
    [HideInInspector]
    public int atkPower;

    public void Update()
    {
        ////언데드보스 투사체는 제외하는 조건
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
                //Debug.Log("방패에 부딪힘");
                touchPlayer.Add(player);              
                Destroy(this.gameObject,0.1f); //0.01초는 안막힘 머임?
            }
        }
        //공격 오브젝트가 플레이어에게 닿았을 때
        //보스던전 필드 벽에 오브젝트 닿았을때 사라지는 조건 추가하기
        else if (other.CompareTag("Player") && (!other.CompareTag("PlayerGuard") || !other.CompareTag("PlayerParry")))
        {
            //닿은 플레이어를 player변수에 넣어주고
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