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

    public GameObject projectileObj; //ȭ�� ������Ʈ
    public GameObject bossMissile; //����ź ������Ʈ
    public GameObject atkObj;
    public GameObject explosionObj;

    public GameObject[] missilePort; //����ź �߻� ��ġ


    private int numberOfProjectiles; // �߻�ü ��
    private float projectileSpeed = 8f; // ����ü �ӵ�

    private Vector3 atkPoint; // ���� ������ġ ����


    void Awake()
    {
        uhBossMonster = FindObjectOfType<UndeadHorseBehavior>();
        dkBossMonster = FindObjectOfType<DarkKnightBehavior>();
        tuBossMonster = FindObjectOfType<ToonUndeadBehavior>();
    }
    
    /// <summary>
    /// �𵥵��� ȭ��5�߽�� ��������
    /// </summary>
    public void SwordSlash()
    {
        numberOfProjectiles = 5;
        //������Ʈ ���� �߽���
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        // �߻�ü ���̰�
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
    /// �𵥵��� �������� Obj��������
    /// </summary>
    public void ComboAttack()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * -0.5f + (atkPoint * uhBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);

        //���ݿ�����Ʈ ����� ȣ��
        atk.transform.localScale = new Vector3(uhBossMonster.stats.atkData.atkSize, uhBossMonster.stats.atkData.atkHeight, uhBossMonster.stats.atkData.atkLength);
        //�������� ȣ��
        atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
    }

    /// <summary>
    /// ����ź ���� ����
    /// </summary>
    public void SwordCasting()
    {
        numberOfProjectiles = 2;

        //������Ʈ ���� �߽���
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
    /// ���� �������� ����
    /// </summary>
    public void Taunt()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * -1f + (atkPoint * uhBossMonster.stats.atkData.atkRange), Quaternion.identity);

        //���ݿ�����Ʈ ����� ȣ��
        atk.transform.localScale = new Vector3(uhBossMonster.stats.atkData.atkSize, uhBossMonster.stats.atkData.atkHeight, uhBossMonster.stats.atkData.atkLength);
        //�������� ȣ��
        atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
    }



    /// <summary>
    /// �� ������ �𵥵��� �������� Obj��������
    /// </summary>
    public void ChargeAttack()
    {
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        GameObject atk = Instantiate(atkObj, transform.position + transform.up * -0.5f + (atkPoint * tuBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);

        //���ݿ�����Ʈ ����� ȣ��
        atk.transform.localScale = new Vector3(tuBossMonster.stats.atkData.atkSize, tuBossMonster.stats.atkData.atkHeight, tuBossMonster.stats.atkData.atkLength);
        //�������� ȣ��
        atk.GetComponent<MonsterAttack>().atkPower = tuBossMonster.stats.atkData.atkDamage;
    }

    /// <summary>
    /// �� ������ �𵥵��� �ҵ��� Obj��������
    /// </summary>
    public void FireBreath()
    {
        numberOfProjectiles = 3;
        //������Ʈ ���� �߽���
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;

        // �߻�ü ���̰�
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

        //���ݿ�����Ʈ ����� ȣ��
        atk.transform.localScale = new Vector3(tuBossMonster.stats.atkData.atkSize, tuBossMonster.stats.atkData.atkHeight, tuBossMonster.stats.atkData.atkLength);
        //�������� ȣ��
        atk.GetComponent<MonsterAttack>().atkPower = tuBossMonster.stats.atkData.atkDamage;
    }

    public void FlexMuscle()
    {
        GameObject atk = Instantiate(explosionObj, transform.position + transform.up * -0.5f + (atkPoint * tuBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);
    }
}