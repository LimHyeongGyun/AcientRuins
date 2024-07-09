using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UndeadKnightAtkObj : MonoBehaviour
{
    [HideInInspector]
    public UndeadHorseBehavior uhBossMonster;

    [HideInInspector]
    public DarkKnightBehavior dkBossMonster;

    [HideInInspector]
    public ToonUndeadBehavior tuBossMonster;

    public GameObject projectileObj; //화살 오브젝트
    public GameObject bossMissile; //유도탄 오브젝트
    public GameObject atkObj;
    public GameObject explosionObj;

    public GameObject[] missilePort; //유도탄 발사 위치


    private int numberOfProjectiles; // 발사체 수
    private float projectileSpeed = 8f; // 투사체 속도

    private Vector3 atkPoint; // 몬스터 공격위치 변수


    void Awake()
    {
        uhBossMonster = FindObjectOfType<UndeadHorseBehavior>();
        dkBossMonster = FindObjectOfType<DarkKnightBehavior>();
        tuBossMonster = FindObjectOfType<ToonUndeadBehavior>();
    }
    
    /// <summary>
    /// 언데드기사 화살5발쏘는 공격패턴
    /// </summary>
    public void SwordSlash()
    {
        numberOfProjectiles = 5;
        //오브젝트 생성 중심점
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        // 발사체 사이각
        float angleRange = 30f;

        // Calculate the angle increment between each projectile
        float angleIncrement = angleRange / 2; 

        // Loop to instantiate projectiles in a fan shape
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the angle for the current projectile
            float angle = -angleRange  + angleIncrement * i;

            // Create a rotation based on the angle
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Calculate the position and direction of the projectile
            Vector3 direction = rotation * transform.forward;

            Vector3 spawnPosition = transform.position + transform.up * -1f + (atkPoint * uhBossMonster.stats.atkData.atkRange);

            // Instantiate the projectile
            GameObject atk = Instantiate(projectileObj, spawnPosition, Quaternion.identity, transform);

            // Set the projectile's scale and attack power
            atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
            
            // Apply rotation to the projectile
            atk.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0); ;

            // Set the direction of the projectile
            atk.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        }
    }
    
    /// <summary>
    /// 언데드기사 근접공격 Obj생성패턴
    /// </summary>
    public void ComboAttack()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * -0.5f + (atkPoint * uhBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);

        //공격오브젝트 사이즈값 호출
        atk.transform.localScale = new Vector3(uhBossMonster.stats.atkData.atkSize, uhBossMonster.stats.atkData.atkHeight, uhBossMonster.stats.atkData.atkLength);
        //데미지값 호출
        atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
    }

    /// <summary>
    /// 유도탄 공격 패턴
    /// </summary>
    public void SwordCasting()
    {
        numberOfProjectiles = 2;

        //오브젝트 생성 중심점
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        // Loop to instantiate projectiles in a fan shape
        for (int i = 0; i < numberOfProjectiles; i++)
        { 
            // Instantiate the projectile
            GameObject atk = Instantiate(bossMissile, missilePort[i].transform.position, Quaternion.identity);

            atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
        }
    }

    /// <summary>
    /// 보스 점프공격 패턴
    /// </summary>
    public void Taunt()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * -1f + (atkPoint * uhBossMonster.stats.atkData.atkRange), Quaternion.identity);

        //공격오브젝트 사이즈값 호출
        atk.transform.localScale = new Vector3(uhBossMonster.stats.atkData.atkSize, uhBossMonster.stats.atkData.atkHeight, uhBossMonster.stats.atkData.atkLength);
        //데미지값 호출
        atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
    }



    /// <summary>
    /// 말 내린후 언데드기사 근접공격 Obj생성패턴
    /// </summary>
    public void ChargeAttack()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * -0.5f + (atkPoint * tuBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);

        //공격오브젝트 사이즈값 호출
        atk.transform.localScale = new Vector3(tuBossMonster.stats.atkData.atkSize, tuBossMonster.stats.atkData.atkHeight, tuBossMonster.stats.atkData.atkLength);
        //데미지값 호출
        atk.GetComponent<MonsterAttack>().atkPower = tuBossMonster.stats.atkData.atkDamage;
    }

    /// <summary>
    /// 말 내린후 언데드기사 불덩이 Obj생성패턴
    /// </summary>
    public void FireBreath()
    {
        numberOfProjectiles = 3;
        //오브젝트 생성 중심점
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        // 발사체 사이각
        float angleRange = 30f;

        // Loop to instantiate projectiles in a fan shape
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the angle for the current projectile
            float angle = -angleRange + (angleRange * i);

            // Create a rotation based on the angle
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Calculate the position and direction of the projectile
            Vector3 direction = rotation * transform.forward;

            Vector3 spawnPosition = transform.position + transform.up * 1f + (atkPoint * tuBossMonster.stats.atkData.atkRange);

            // Instantiate the projectile
            GameObject atk = Instantiate(projectileObj, spawnPosition, Quaternion.identity, transform);

            // Set the projectile's scale and attack power
            atk.GetComponent<MonsterAttack>().atkPower = tuBossMonster.stats.atkData.atkDamage;

            // Apply rotation to the projectile
            atk.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0); ;

            // Set the direction of the projectile
            atk.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        }
    }

    public void Taunt2()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * 0.1f + (atkPoint * tuBossMonster.stats.atkData.atkRange), Quaternion.identity);

        //공격오브젝트 사이즈값 호출
        atk.transform.localScale = new Vector3(tuBossMonster.stats.atkData.atkSize, tuBossMonster.stats.atkData.atkHeight, tuBossMonster.stats.atkData.atkLength);
        //데미지값 호출
        atk.GetComponent<MonsterAttack>().atkPower = tuBossMonster.stats.atkData.atkDamage;
    }

    public void FlexMuscle()
    {
        GameObject atk = Instantiate(explosionObj, transform.position + transform.up * -0.5f + (atkPoint * tuBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);
    }
}