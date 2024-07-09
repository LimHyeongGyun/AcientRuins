using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private HashSet<BasicMonster> enteredMon = new HashSet<BasicMonster>(); //중복 데미지 방지
    public int attackPower;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip groundAtkClip;
    [SerializeField]
    private GameObject attackEft;

    void Update()
    {
        DestroyObj();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterParry"))
        {
            var parryMonster = other.gameObject.GetComponentInParent<BasicMonster>();
            if (!enteredMon.Contains(parryMonster))
            {
                enteredMon.Add(parryMonster);
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag("Monster"))
        {
            var monster = other.gameObject.GetComponentInChildren<BasicMonster>(); //몬스터의 스크립트 담아오기
            if (!enteredMon.Contains(monster)) //몬스터가 대미지를 받았는지 확인
            {
                Instantiate(attackEft, transform.position, Quaternion.identity);
                enteredMon.Add(monster); //대미지를 받지 않았다면 HashSet 추가
                audioSource.PlayOneShot(attackClip);
                monster.DecreaseHp(attackPower);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            if (!audioSource.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
        else if (!other.CompareTag("Monster") && (other.CompareTag("Untagged") || other.CompareTag("Ground")))
        {
            Instantiate(attackEft, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(groundAtkClip);
        }
    }

    private void DestroyObj()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(this.gameObject, 0.05f);
        }
    }
}
