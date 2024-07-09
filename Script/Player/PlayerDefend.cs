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
            //방패 내구도 감소
            if (gameObject.CompareTag("PlayerParry")) //오브젝트가 패링 상태일 때
            {
                var monsterAtk = other.gameObject.GetComponent<MonsterAttack>();
                if (!enteredAtk.Contains(monsterAtk))
                {
                    enteredAtk.Add(monsterAtk);
                    BasicMonster mon = other.transform.parent.GetComponent<BasicMonster>(); //몬스터 공격 오브젝트의 부모오브젝트(몬스터) 정보 참조
                    //보스몬스터가 아닐 때
                    if (mon.monType != BasicMonster.MonsterType.BossMonster)
                    {
                        mon.Groggy(); //몬스터 상태 그로기상태 변경 매서드 호출
                    }
                    //보스몬스터일때
                    else if (mon.monType != BasicMonster.MonsterType.BossMonster)
                    {
                        shieldData.destroy = true;//방패 파괴
                    }
                    mon.Groggy(); //몬스터 종류 추가되면 지울것
                    shieldData.ShieldDurabilityManageMent(monsterAtk.atkPower); //실드 내구도 감소
                    if (monsterAtk != null) //몬스터 공격이 파괴되지 않았으면 파괴
                    {
                        Destroy(monsterAtk);
                    }
                }
            }
            else if (gameObject.CompareTag("PlayerGuard")) //오브젝트가 가드 상태 일 때
            {
                var monsterAtk = other.gameObject.GetComponent<MonsterAttack>();
                BasicMonster mon = monsterAtk.GetComponentInParent<BasicMonster>();
                if (mon.monType == BasicMonster.MonsterType.BossMonster)
                {
                    shieldData.destroy = true;//방패 파괴
                }
                else if (mon.monType != BasicMonster.MonsterType.BossMonster)
                {
                    //튕겨져 나가는 액션
                    mon.anim.SetTrigger("isCrash");
                }
                shieldData.ShieldDurabilityManageMent(monsterAtk.atkPower);
            }
        }
    }
    private void DestroyObj()
    {
        if (gameObject.CompareTag("PlayerParry")) //패링일 경우
        {
            if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("Parry"))
            {
                if(player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.36f) //방패가 플레이어 캐릭터 정면을 열어주었을 때
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (gameObject.CompareTag("PlayerGuard")) //가드 상태일 경우
        {
            if (!player.guard) //가드 해제시
            {
                Destroy(gameObject); //파괴
            }
        }
    }
}
