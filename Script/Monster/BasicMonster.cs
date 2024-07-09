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
    /// 스크립트 캐싱 부분
    /// </summary>
    #region
    //Normal, Elite 몬스터 스크립트 초기화
    [HideInInspector]
    public MonsterBehavior monster;
    //타겟 플레이어 초기화
    [HideInInspector]
    public DarkKnightBehavior dkBossMonster;

    public UndeadHorseBehavior uhBossMonster;
    
    public ToonUndeadBehavior tuBossMonster;

    //[HideInInspector]
    public Transform target;
    //애니메이션 초기화
    [HideInInspector]
    public Animator anim;
    //nvMesh 초기화
    [HideInInspector]
    public NavMeshAgent nvAgent;

    //체력바 코드
    [SerializeField] public HealthBar _healthbar;
    #endregion

    //사운드 구현 위한 AudioSource
    public AudioClip DieSound;
    public AudioClip GetHitSound;
    public AudioClip AttackSound;
    private AudioSource audioSource;

    public RaycastHit[] Hits;
    Rigidbody rigid;
    [SerializeField] //layer마스크
    private LayerMask targetLayer;
    [HideInInspector]
    public int layerMask;

    public GameObject bossTarget;
    public Transform spawnPoint; //스폰 포인트    

    public GameObject atkObj; //공격 오브젝트
    public GameObject defendObj; //패링 오브젝트
    //public GameObject[] dropItems; //사망시 떨어지는 아이템들 실버,메모리
    [SerializeField]
    private GameObject commodities;
    private Vector3 atkPoint; // 몬스터 공격위치 변수

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
        layerMask = 1 << LayerMask.NameToLayer("Player");  // Player 레이어만 충돌 체크함
    }

    void Update()
    {
        AnimControl();
        if (monType != MonsterType.BossMonster)// (monType != MonsterType.DKBossMonster || monType != MonsterType.UHBossMonster)
        {
            monster.CheckState(ref Hits, layerMask, ref target, ref spawnPoint, anim, nvAgent);

            if (!isDead)
            {
                //공격하는 도중에는 추격 x 
                // isChase가 true - 추격일때 따라가고
                if (isChase)
                {
                    nvAgent.SetDestination(target.position);
                }

                //공격 도중에는 LookAt을 하지 않고, 끝난 후 타겟을 바라보도록 // 공격 안하면서 추격할 때 타겟 바라보기
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Chase") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {              
                    transform.LookAt(target);
                }
            }
        }

        else
        {//사망상태가 아닐때    
            if (!isDead)
            {
                if (dkBossMonster != null) //Dark Knight 보스가 있을때
                {
                    if(!dkBossMonster.isBattle) //isBattle이 false일때
                    {
                        transform.LookAt(bossTarget.transform); //몬스터 플레이어방향 시선처리
                        if(dkBossMonster.stats.curState == CurrentState.Chase) //현재상태가 Chase일때
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
    /// 세이프티존 진입 시 복귀하는 함수
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (!isDead) {
            if (other.gameObject.CompareTag("SafetyZone"))
            {
                //Debug.Log("세이프티존 통과");
                monster.Return(nvAgent, ref target, ref spawnPoint, anim);
            }
        }
    }
    //게임오브젝트 생성 메서드
    #region
    public void Attack()
    {
        //Debug.Log("공격옵젝 생성");
        GameObject atk = atkObj;
        atk.GetComponent<BoxCollider>();   
        //오브젝트 생성 중심점
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
               
        if (dkBossMonster != null)
        {
            //공격 오브젝트 사이즈값 호출
            atk.transform.localScale = new Vector3(dkBossMonster.stats.atkData.atkSize, dkBossMonster.stats.atkData.atkHeight, dkBossMonster.stats.atkData.atkLength);
            //데미지값 호출
            atk.GetComponent<MonsterAttack>().atkPower = dkBossMonster.stats.atkData.atkDamage;
            Instantiate(atk, transform.position + transform.up + (atkPoint * dkBossMonster.stats.atkData.atkRange), Quaternion.identity,transform);
        }

        else if (uhBossMonster != null)
        {
            //공격 오브젝트 사이즈값 호출
            atk.transform.localScale = new Vector3(uhBossMonster.stats.atkData.atkSize, uhBossMonster.stats.atkData.atkHeight, uhBossMonster.stats.atkData.atkLength);
            //데미지값 호출
            atk.GetComponent<MonsterAttack>().atkPower = uhBossMonster.stats.atkData.atkDamage;
            Instantiate(atk, transform.position + transform.up + (atkPoint * uhBossMonster.stats.atkData.atkRange), Quaternion.identity, transform);
        }

        else if(tuBossMonster != null)
        {
            //공격 오브젝트 사이즈값 호출
            atk.transform.localScale = new Vector3(tuBossMonster.stats.atkData.atkSize, tuBossMonster.stats.atkData.atkHeight, tuBossMonster.stats.atkData.atkLength);
            //데미지값 호출
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
            //공격 오브젝트 사이즈값 호출
            atk.transform.localScale = new Vector3(monster.stats.atkData.atkSize, monster.stats.atkData.atkHeight, monster.stats.atkData.atkLength);
            //데미지값 호출
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

    //애니메이션에 따른 nvAgent 컨트롤 메서드
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
    //몬스터 체력감소 및 피격 함수
    #region
    public void DecreaseHp(int damage) //플레이어의 공격오브젝트에서 호출해줄 함수
    {
        //보스몬스터 체력감소 피격모션 x
        
       if(dkBossMonster != null)
       {
            dkBossMonster.stats.healthData.hp -= damage;

            _healthbar.UpdateHealthBar(dkBossMonster.stats.healthData.maxHp, dkBossMonster.stats.healthData.hp);
            //Debug.Log("보스체력: " + dkBossMonster.stats.healthData.hp);
       }
            
       else if(uhBossMonster != null)
       {
            uhBossMonster.stats.healthData.hp -= damage;
            //Debug.Log("보스체력: " + uhBossMonster.stats.healthData.hp);

            _healthbar.UpdateHealthBar(uhBossMonster.stats.healthData.maxHp, uhBossMonster.stats.healthData.hp);
            
       }

       else if (tuBossMonster != null)
        {
            tuBossMonster.stats.healthData.hp -= damage;

            _healthbar.UpdateHealthBar(tuBossMonster.stats.healthData.maxHp, tuBossMonster.stats.healthData.hp);
            //Debug.Log("보스체력: " + tuBossMonster.stats.healthData.hp);
        }
        //일반,엘리트몬스터 체력감소 피격모션 o
        else 
        {
            monster.stats.healthData.hp -= damage;
            _healthbar.UpdateHealthBar(monster.stats.healthData.maxHp, monster.stats.healthData.hp);

            //피격 사운드 출력
            
        }
        audioSource.clip = GetHitSound;
        audioSource.Play();

        OnDamage();
        //if(anim.GetCurrentAnimatorStateInfo(0).IsName("GetDamage") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
    }

    public void OnDamage()
    {
        //보스몬스터 사망로직
        if(dkBossMonster != null)
        {
            if (dkBossMonster.stats.healthData.hp <= 0)
            {
                dkBossMonster.StopAllCoroutines();
                Die();
                dialogueManager.FindContext("첫번째 보스 처치");
                dialogueManager.ActiveUI();
                //사망 사운드 출력
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

                //사망 사운드 출력
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
                dialogueManager.FindContext("마지막 보스 처치");
                dialogueManager.ActiveUI();
                //사망 사운드 출력
                audioSource.clip = DieSound;
                audioSource.Play();
            }
        }

        //일반,엘리트몬스터 피격, 사망로직
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

                //사망 사운드 출력
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
    //그로기, 사망 함수
    #region
    //플레이어패리를 맞고 그로기 상태가 되었을 때
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