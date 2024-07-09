using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicMonster : MonoBehaviour, IAttack, IDamaged, IGroggy
{
    public enum MonsterType { NormalMonster, EliteMonster, BossMonster }
    public MonsterType monType;

    public Portal portal;
    public DialogueManager dialogueManager;
    public ClearFinalBoss clearFinalBoss;
    /// <summary>
    /// ��ũ��Ʈ ĳ�� �κ�
    /// </summary>
    #region
    //Normal, Elite ���� ��ũ��Ʈ �ʱ�ȭ
    [HideInInspector]
    public MonsterBehavior monster;
    //Ÿ�� �÷��̾� �ʱ�ȭ
    [HideInInspector]
    public DarkKnightBehavior dkBossMonster;

    public UndeadHorseBehavior uhBossMonster;
    
    public ToonUndeadBehavior tuBossMonster;

    //[HideInInspector]
    public Transform target;
    //�ִϸ��̼� �ʱ�ȭ
    [HideInInspector]
    public Animator anim;
    //nvMesh �ʱ�ȭ
    [HideInInspector]
    public NavMeshAgent nvAgent;

    //ü�¹� �ڵ�
    [SerializeField] public HealthBar _healthbar;
    #endregion

    //���� ���� ���� AudioSource
    public AudioClip DieSound;
    public AudioClip GetHitSound;
    public AudioClip AttackSound;
    private AudioSource audioSource;

    public RaycastHit[] Hits;
    Rigidbody rigid;
    [SerializeField] //layer����ũ
    private LayerMask targetLayer;
    [HideInInspector]
    public int layerMask;

    public GameObject bossTarget;
    public Transform spawnPoint; //���� ����Ʈ    

    public GameObject atkObj; //���� ������Ʈ
    public GameObject defendObj; //�и� ������Ʈ
    //public GameObject[] dropItems; //����� �������� �����۵� �ǹ�,�޸�
    [SerializeField]
    private GameObject commodities;
    private Vector3 atkPoint; // ���� ������ġ ����

    [HideInInspector]
    public bool isDead;
    private bool isChase;


    void Awake()
    {
        if(monType == MonsterType.BossMonster)
        {
            dkBossMonster = FindObjectOfType<DarkKnightBehavior>();

            uhBossMonster = FindObjectOfType<UndeadHorseBehavior>();

            tuBossMonster = FindObjectOfType<ToonUndeadBehavior>();
        }

        else
        {
            spawnPoint = this.transform.parent.transform;
            monster = GetComponent<MonsterBehavior>();
            rigid = GetComponent<Rigidbody>();
        }

        audioSource = GetComponent<AudioSource>();
        nvAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();  
    }

    void Start()
    {
        if (monType !=MonsterType.BossMonster)//(monType != MonsterType.DKBossMonster || monType != MonsterType.UHBossMonster)
        {
            target = spawnPoint;
        }

        else //if (monType == MonsterType.DKBossMonster || monType == MonsterType.UHBossMonster)
        {
            bossTarget = GameObject.FindGameObjectWithTag("Player");

            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        layerMask = 1 << LayerMask.NameToLayer("Player");  // Player ���̾ �浹 üũ��
    }

    void Update()
    {
        AnimControl();
        if (monType != MonsterType.BossMonster)// (monType != MonsterType.DKBossMonster || monType != MonsterType.UHBossMonster)
        {
            monster.CheckState(ref Hits, layerMask, ref target, ref spawnPoint, anim, nvAgent);

            if (!isDead)
            {
                //�����ϴ� ���߿��� �߰� x 
                // isChase�� true - �߰��϶� ���󰡰�
                if (isChase)
                {
                    nvAgent.SetDestination(target.position);
                }

                //���� ���߿��� LookAt�� ���� �ʰ�, ���� �� Ÿ���� �ٶ󺸵��� // ���� ���ϸ鼭 �߰��� �� Ÿ�� �ٶ󺸱�
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Chase") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {              
                    transform.LookAt(target);
                }
            }
        }

        else
        {//������°� �ƴҶ�    
            if (!isDead)
            {
                if (dkBossMonster != null) //Dark Knight ������ ������
                {
                    if(!dkBossMonster.isBattle) //isBattle�� false�϶�
                    {
                        transform.LookAt(bossTarget.transform); //���� �÷��̾���� �ü�ó��
                        if(dkBossMonster.stats.curState == CurrentState.Chase) //������°� Chase�϶�
                            nvAgent.SetDestination(bossTarget.transform.position);
                    }
                }

                else if (uhBossMonster != null)
                {
                    if (!uhBossMonster.isBattle)
                    {
                        transform.LookAt(bossTarget.transform);
                        if (uhBossMonster.stats.curState == CurrentState.Chase)
                            nvAgent.SetDestination(bossTarget.transform.position);
                    }
                }

                else if(tuBossMonster != null)
                {
                    if (!tuBossMonster.isBattle)
                    {
                        transform.LookAt(bossTarget.transform);
                        if (tuBossMonster.stats.curState == CurrentState.Chase)
                            nvAgent.SetDestination(bossTarget.transform.position);
                    }
                }
            }
        }      
    }

    void FixedUpdate()
    {
        if (monType != MonsterType.BossMonster) //(monType != MonsterType.DKBossMonster || monType != MonsterType.UHBossMonster)
            FreezeVelocity();
    }
    void FreezeVelocity()
    {
        if (!isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// ������Ƽ�� ���� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (!isDead) {
            if (other.gameObject.CompareTag("SafetyZone"))
            {
                //Debug.Log("������Ƽ�� ���");
                monster.Return(nvAgent, ref target, ref spawnPoint, anim);
            }
        }
    }
    //���ӿ�����Ʈ ���� �޼���
    #region
    public void Attack()
    {
        //Debug.Log("���ݿ��� ����");
        GameObject atk = atkObj;
        atk.GetComponent<BoxCollider>();   
        //������Ʈ ���� �߽���
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
               
        if (dkBossMonster != null)
        {
            //���� ������Ʈ ����� ȣ��
            atk.transform.localScale = new Vector3(dkBossMonster.stats.atkData.atkSize, dkBossMonster.stats.atkData.atkHeight, dkBossMonster.stats.atkData.atkLength);
            //�������� ȣ��
            atk.GetComponent<MonsterAttack>().atkPower = dkBossMonster.stats.atkData.atkDamage;
            Instantiate(atk, transform.position + transform.up + (atkPoint * dkBossMonster.stats.atkData.atkRange), Quaternion.identity,transform);
        }

        else if (uhBossMonster != null)
        {
            //���� ������Ʈ ����� ȣ��
            atk.transform.localScale = new Vector3(uhBossMonster.stats.atkData.atkSize, uhBossMonster.stats.atkData.atkHeight, uhBossMonster.stats.atkData.atkLength);
            //�������� ȣ��
            atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
            Instantiate(atk, transform.position + transform.up + (atkPoint * uhBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);
        }

        else if(tuBossMonster != null)
        {
            //���� ������Ʈ ����� ȣ��
            atk.transform.localScale = new Vector3(tuBossMonster.stats.atkData.atkSize, tuBossMonster.stats.atkData.atkHeight, tuBossMonster.stats.atkData.atkLength);
            //�������� ȣ��
            atk.GetComponent<MonsterAttack>().atkPower = tuBossMonster.stats.atkData.atkDamage;
            Instantiate(atk, transform.position + transform.up + (atkPoint * tuBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);
        }
        else
        { 
            if(AttackSound != null)
            {
                audioSource.clip = AttackSound;
                audioSource.Play();
            }
            //���� ������Ʈ ����� ȣ��
            atk.transform.localScale = new Vector3(monster.stats.atkData.atkSize, monster.stats.atkData.atkHeight, monster.stats.atkData.atkLength);
            //�������� ȣ��
            atk.GetComponent<MonsterAttack>().atkPower = monster.stats.atkData.atkDamage;
            Instantiate(atk, transform.position + transform.up + (atkPoint * monster.stats.atkData.atkRange), Quaternion.identity, transform);
        }
    }

    public void Parrying()
    {
        defendObj.transform.localScale = new Vector3(monster.stats.atkData.atkSize, monster.stats.atkData.atkHeight, monster.stats.atkData.atkLength);
        defendObj.tag = "MonsterParry";
        GameObject defend = Instantiate(defendObj, transform.position + transform.up , Quaternion.identity,transform);
        defend.transform.forward = this.transform.forward;
    }
    #endregion

    //�ִϸ��̼ǿ� ���� nvAgent ��Ʈ�� �޼���
    public void AnimControl()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Parry") || anim.GetCurrentAnimatorStateInfo(0).IsName("Evasion")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Battlecry"))
        {
            isChase = false;
        }

        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Chase") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            isChase = true;
        }

        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            isDead = true;
            gameObject.layer = 8;

            if (tuBossMonster != null)
            {
                clearFinalBoss = FindObjectOfType<ClearFinalBoss>();
                clearFinalBoss.ActiveRelic();
            }
            Destroy(this.gameObject, 3.5f);            
        }
    }
    //���� ü�°��� �� �ǰ� �Լ�
    #region
    public void DecreaseHp(int damage) //�÷��̾��� ���ݿ�����Ʈ���� ȣ������ �Լ�
    {
        //�������� ü�°��� �ǰݸ�� x
        
       if(dkBossMonster != null)
       {
            dkBossMonster.stats.healthData.hp -= damage;

            _healthbar.UpdateHealthBar(dkBossMonster.stats.healthData.maxHp, dkBossMonster.stats.healthData.hp);
            //Debug.Log("����ü��: " + dkBossMonster.stats.healthData.hp);
       }
            
       else if(uhBossMonster != null)
       {
            uhBossMonster.stats.healthData.hp -= damage;
            //Debug.Log("����ü��: " + uhBossMonster.stats.healthData.hp);

            _healthbar.UpdateHealthBar(uhBossMonster.stats.healthData.maxHp, uhBossMonster.stats.healthData.hp);
            
       }

       else if (tuBossMonster != null)
        {
            tuBossMonster.stats.healthData.hp -= damage;

            _healthbar.UpdateHealthBar(tuBossMonster.stats.healthData.maxHp, tuBossMonster.stats.healthData.hp);
            //Debug.Log("����ü��: " + tuBossMonster.stats.healthData.hp);
        }
        //�Ϲ�,����Ʈ���� ü�°��� �ǰݸ�� o
        else 
        {
            monster.stats.healthData.hp -= damage;
            _healthbar.UpdateHealthBar(monster.stats.healthData.maxHp, monster.stats.healthData.hp);

            //�ǰ� ���� ���
            
        }
        audioSource.clip = GetHitSound;
        audioSource.Play();

        OnDamage();
        //if(anim.GetCurrentAnimatorStateInfo(0).IsName("GetDamage") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
    }

    public void OnDamage()
    {
        //�������� �������
        if(dkBossMonster != null)
        {
            if (dkBossMonster.stats.healthData.hp <= 0)
            {
                dkBossMonster.StopAllCoroutines();
                Die();
                dialogueManager.FindContext("ù��° ���� óġ");
                dialogueManager.ActiveUI();
                //��� ���� ���
                audioSource.clip = DieSound;
                audioSource.Play();
            }
        }

        else if (uhBossMonster != null)
        {
            if (uhBossMonster.stats.healthData.hp <= 0)
            {
                uhBossMonster.StopAllCoroutines();
                Die();

                //��� ���� ���
                audioSource.clip = DieSound;
                audioSource.Play();
            }
        }

        else if(tuBossMonster != null)
        {
            if (tuBossMonster.stats.healthData.hp <= 0)
            {
                tuBossMonster.StopAllCoroutines();
                Die();
                dialogueManager.FindContext("������ ���� óġ");
                dialogueManager.ActiveUI();
                //��� ���� ���
                audioSource.clip = DieSound;
                audioSource.Play();
            }
        }

        //�Ϲ�,����Ʈ���� �ǰ�, �������
        else
        {
            if (monster.stats.healthData.hp > 0)
            {
                anim.SetTrigger("GetDamage");
                foreach (AnimatorControllerParameter parameter in anim.parameters)
                {
                    if (parameter.type == AnimatorControllerParameterType.Bool)
                    {
                        anim.SetBool(parameter.name, false);
                    }
                }
            }
            else if (monster.stats.healthData.hp <= 0)
            {
                DropCommodities();
                Die();

                //��� ���� ���
                audioSource.clip = DieSound;
                audioSource.Play();
            }
        }
    }
    #endregion

    private void DropCommodities()
    {
        GameObject _commodities = Instantiate(commodities, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        _commodities.GetComponent<Commodities>().monster = this;
        _commodities.GetComponent<Commodities>().CommoditiesAmount();
    }
    //�׷α�, ��� �Լ�
    #region
    //�÷��̾��и��� �°� �׷α� ���°� �Ǿ��� ��
    public void Groggy()
    {
        anim.SetTrigger("isGroggy");
    }

    public void Die()
    {
        anim.SetTrigger("doDie");
        if (monType == MonsterType.BossMonster)
        {
            portal = FindObjectOfType<Portal>();
            portal.ActiveClearPortal();
        }
    }
    #endregion
}