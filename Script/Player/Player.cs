using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Player : PlayerBehavior, IDamageable, IDefenceFunc, IAttackFunc, IPlayerFunc
{
    //상태디자인 패턴
    public enum PlayerState { Idle, SwordIdle, Move, Run, Turn, Attack, Skill, Evade, Recovery, Interaction, Air, Damage, Die }
    public enum GuardState { Idle, Evade, Unbeatable }
    public enum JumpState { Ground, Jump, JumpIng, Attack, Fall}
    public PlayerState state;
    public GuardState guardState;
    public JumpState jumpState;

    //참조 스크립트
    #region
    private HPSlider hpSlider;
    private StaminaSlider staminaSlider;
    private BaseCamp baseCamp;
    public PlayerView playerView;
    public EquipData equipData;
    public Inventory inventory;
    private InventoryUI inventoryUI;
    private DwarfUI dwarfUI;
    private UIController uiController;

    public PlayerSetData playerSetdata;
    [HideInInspector]
    public DataManager data;
    private GameManager gameManager;
    private DialogueManager dialogueManager;
    private UIManager uiManager;
    #endregion

    public int stamina;
    public bool useStamina;
    public float jumpTime = 2f;
    public float moveValue;
    public float blendValue;

    //전투
    #region
    private Vector3 atkPoint; //공격 위치
    private Vector3 guardPoint;//방어 위치
    #endregion

    //애니메이션 bool
    #region
    public bool sword;

    public bool isMove;
    private bool isRun;
    public bool isfall;
    public bool jump;
    public bool guard;
    private bool recovery;

    public bool actEnd = true;
    #endregion

    //상호작용
    #region
    private float time;
    private float resttime = 5f;
    bool rest;
    private int InteractionpLayerMask; //상호작용 레이어 마스크
    private int groundMask;
    private float senseRadius;
    public bool isGround = true;
    public bool interactive;
    RaycastHit interactionHit;
    public GameObject interactionBtn;
    #endregion

    //바닥 체크
    #region
    RaycastHit lefthit;
    RaycastHit righthit;
    RaycastHit fronthit;
    RaycastHit backhit;
    //RaycastHit sphereHit;
    Ray leftRay;
    Ray rightRay;
    Ray frontRay;
    Ray backRay;
    #endregion

    public bool left;
    public bool right;
    public bool front;
    public bool back;
    //public bool circle;
    //컴포넌트
    #region
    [HideInInspector]
    public Animator anim;
    private Rigidbody rigid;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip stepSound;
    [SerializeField]
    private AudioClip attackSound;
    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        data = FindObjectOfType<DataManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        hpSlider = FindObjectOfType<HPSlider>();
        staminaSlider = FindObjectOfType<StaminaSlider>();
        baseCamp = FindObjectOfType<BaseCamp>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        dwarfUI = FindObjectOfType<DwarfUI>();
        uiController = FindObjectOfType<UIController>();
    }
    private void Start()
    {
        InteractionpLayerMask = (1 << LayerMask.NameToLayer("NPC")) + (1 << LayerMask.NameToLayer("Item")) + (1 << LayerMask.NameToLayer("BaseCamp")) + (1 << LayerMask.NameToLayer("Portal")) + (1 << LayerMask.NameToLayer("Commodities"));
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        senseRadius = 0.8f;

        staminaSlider.SetStamina();
        interactionBtn = GameObject.Find("InteractionBtn");
        interactionBtn.SetActive(false);
    }


    private void Update()
    {
        if (!uiController.activeUI && !inventoryUI.activeUI && !dwarfUI.activeUI && !dialogueManager.activeUI && !baseCamp.activeUI && state != PlayerState.Die)
        {
            InteractionSense(); //상호작용 센서
        }
        if (actEnd)
        {
            ActionController();
        }
        GroundJudgeMent();//지면 확인
        StaminaManaged();
        AnimationController();
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetTrigger("Trick");
        }
    }
    /// <summary>
    /// 액션 컨트롤
    /// </summary>
    #region
    private void ActionController()
    {
        if (!sword)
        {
            IdleAction();
        }
        else if (sword && (state == PlayerState.SwordIdle || state == PlayerState.Move || state == PlayerState.Run))
        {
            SwordIdleAction();
        }
        StateOfficialUseAction();
    }
    //액션
    private void StateOfficialUseAction()
    {
        if ((state == PlayerState.Idle || state == PlayerState.SwordIdle || state == PlayerState.Move || state == PlayerState.Run || (state == PlayerState.Air && jumpState != JumpState.Fall)))
        {
            Walk(PlayerState.Move);
        }
        if ((state == PlayerState.Idle || state == PlayerState.SwordIdle || state == PlayerState.Move || state == PlayerState.Run))
        {
            Run(PlayerState.Run);
        }
        if ((state == PlayerState.Idle || state == PlayerState.SwordIdle || state == PlayerState.Move || state == PlayerState.Run) && isGround && Input.GetKeyDown(KeyCode.F))
        {
            Jump(PlayerState.Air, JumpState.Jump);
        }
        if ((state == PlayerState.Idle || state == PlayerState.SwordIdle || state == PlayerState.Move || state == PlayerState.Run) && state != PlayerState.Evade && Input.GetKeyDown(KeyCode.Space) && stamina >= 25)
        {
            Evade(PlayerState.Evade);
        }
        if (equipData.consumeItemData.equip && Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItem();
        }
    }
    private void IdleAction()
    {
        if (state == PlayerState.Idle || state == PlayerState.Move || state == PlayerState.Run || state == PlayerState.Evade)
        {
            WeaponOut(PlayerState.SwordIdle);
        }
    }
    private void SwordIdleAction()
    {
        if (state == PlayerState.SwordIdle || state == PlayerState.Move || state == PlayerState.Run)
        {
            WeaponIn(PlayerState.Idle);
        }
        if ((Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(1) && stamina >= 70)) && !guard)
        {
            Attack(PlayerState.Attack, PlayerState.Skill);
        }
        Defend();
    }

    private void AnimationController()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.49f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.79f)
            {
                if (jump)
                {
                    jump = false;
                    rigid.AddForce(transform.up * playerSetdata.stats.jumpHeight, ForceMode.VelocityChange);
                }
                jumpTime -= Time.deltaTime;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.66f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Attack(PlayerState.Attack, PlayerState.Skill);
                }
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.76f)
            {
                jumpState = JumpState.JumpIng;
                jumpTime -= Time.deltaTime;
            }
            if (isGround && jumpState != JumpState.Attack)
            {
                Landing();
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && !isGround && jumpTime <= 0.0f)
            {
                state = PlayerState.Air;
                jumpState = JumpState.Fall;
                Falling(PlayerState.Air, JumpState.Fall);
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpLanding"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
            {
                anim.ResetTrigger("JumpLanding");
                actEnd = true;
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            if (isGround)
            {
                isfall = false;
                Landing();
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HardLanding"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
            {
                anim.SetBool("Falling", false);
                anim.ResetTrigger("HardLanding");
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Picking Up"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                state = PlayerState.Idle;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Recovery"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                equipData.consumeItemData.equipItem.SetActive(false);
                if (equipData.shieldData.equip)
                {
                    equipData.shieldData.equipShield.SetActive(true);
                }
                actEnd = true;
                state = PlayerState.Idle;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.34f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.54f)
            {
                anim.SetFloat("AttackMultiPlier", 2f);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.54f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
            {
                anim.SetFloat("AttackMultiPlier", 0.35f);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.88f)
            {
                anim.SetFloat("AttackMultiPlier", 1.3f);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.88f)
            {
                state = PlayerState.SwordIdle;
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.30f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            {
                anim.SetFloat("AttackMultiPlier", 2f);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.54f)
            {
                anim.SetFloat("AttackMultiPlier", 0.4f);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.54f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.88f)
            {
                anim.SetFloat("AttackMultiPlier", 1.3f);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.89f)
            {
                state = PlayerState.SwordIdle;
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.0f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.2f)
            {
                jumpState = JumpState.Attack;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.89f)
            {
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dizzy"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                anim.SetTrigger("Idle");
                state = PlayerState.SwordIdle;
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Evade"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.15f)
            {
                guardState = GuardState.Unbeatable;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.15f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.77f)
            {
                guardState = GuardState.Evade;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.77f)
            {
                guardState = GuardState.Idle;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.94f)
            {
                rigid.velocity = Vector3.zero;
                state = PlayerState.Idle;
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.79f)
            {
                if (!sword)
                {
                    state = PlayerState.Idle;
                }
                else if (sword)
                {
                    state = PlayerState.SwordIdle;
                }
                jumpState = JumpState.Ground;
                jump = false;
                isGround = true;
                actEnd = true;
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                if (!gameManager.gameOver)
                {
                    gameManager.gameOver = true;
                    gameManager.GameOver();
                }
            }
        }
    }
    #endregion
    /// <summary>
    /// 이동
    /// </summary>
    #region
    //이동
    public override void Walk(PlayerState walk)
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMove = move.magnitude != 0;

        if (isGround)
        {
            moveValue = 0;
            anim.SetFloat("MoveBlend", moveValue);
            anim.SetBool("Move", isMove);
            if (!isMove && state == PlayerState.Move)
            {
                if (!sword)
                {
                    state = PlayerState.Idle;
                }
                else if (sword)
                {
                    state = PlayerState.SwordIdle;
                }
            }
        }

        if (isMove)
        {
            if (isGround)
            {
                if (!audioSource.isPlaying)
                {
                    if (!isRun)
                    {
                        audioSource.pitch = 0.4f;
                    }
                    else if (isRun)
                    {
                        audioSource.pitch = 0.9f;
                    }
                    audioSource.PlayOneShot(stepSound);
                }
            }
            if (state != PlayerState.Air && jumpState == JumpState.Ground)
            {
                state = walk;
            }
            
            Vector3 lookForward = new Vector3(playerView.cameraObj.forward.x, 0f, playerView.cameraObj.forward.z).normalized; //카메라가 보는 정면의 지상과 수평인 방향
            Vector3 lookRight = new Vector3(playerView.cameraObj.right.x, 0f, playerView.cameraObj.right.z).normalized;
            Vector3 moveDir = lookForward * move.y + lookRight * move.x;
            if (isMove && !isRun)
            {
                speed = playerSetdata.stats.walkSpeed;
            }
            transform.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * speed;
        }
    }
    //점프
    public override void Jump(PlayerState player, JumpState air)
    {
        if (isGround && Input.GetKeyDown(KeyCode.F))
        {
            jumpTime = 3f;
            state = player;
            jumpState = air;
            isGround = false;
            jump = true;
            anim.SetTrigger("Jump");
        }
    }
    //달리기
    public override void Run(PlayerState run)
    {
        isRun = isMove && Input.GetKey(KeyCode.LeftShift) && stamina >= 1;
        if (isRun)
        {
            state = run;
            moveValue = 1;
            anim.SetFloat("MoveBlend", moveValue);
            speed = playerSetdata.stats.runSpeed;
        }
    }
    public override void Falling(PlayerState air, JumpState player)
    {
        if (!isfall)
        {
            jumpState = JumpState.Fall;
            isfall = true;
            state = air;
            jumpState = player;
            anim.SetBool("Falling", true);
        }
    }
    public override void Landing()
    {
        jumpTime = 3f;
        if (jumpState == JumpState.Fall)
        {
            anim.SetTrigger("HardLanding");
        }
        else if (jumpState == JumpState.JumpIng)
        {
            anim.SetTrigger("JumpLanding");
            anim.ResetTrigger("Jump");
        }
        state = PlayerState.Idle;
        jumpState = JumpState.Ground;
    }
    #endregion

    /// <summary>
    /// 스탯 관리
    /// </summary>
    public void StaminaManaged()
    {
        staminaSlider.curStamina = stamina;
        ConsumeStamina();
        RecoveryStamina();
    }
    #region
    //스테미나 소모
    public void ConsumeStamina()
    {
        //달리기 스테미나 소모
        if (isRun && state == PlayerState.Run)
        {
            useStamina = true;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                //초당 스테미나 20 소모
                stamina -= 2;
                time = 0.1f;
            }
        }
        //회피구르기 스테미나 소모
        if (stamina >= 25 && Input.GetKeyDown(KeyCode.Space) && state == PlayerState.Evade)
        {
            useStamina = true;
            stamina -= 25;
        }
        //강공격 스테미나 소모
        if (stamina >= 70 && Input.GetMouseButtonDown(1) && state == PlayerState.Skill) //강공격
        {
            useStamina = true;
            stamina -= 70;
        }

        if (stamina < 0)
        {
            stamina = 0;
        }
    }
    //스테미나 회복
    public void RecoveryStamina()
    {        
        if (stamina == 0)
        {
            rest = true;
            if (rest)
            {
                resttime -= Time.deltaTime;
                if (resttime <= 0)
                {
                    rest = false;
                    resttime = 5f;
                    stamina = 10;
                }
            }
        }
        if(!isRun && state != PlayerState.Evade && state != PlayerState.Skill && stamina < 100 && !rest)
        {
            useStamina = false;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                staminaSlider.preStamina = stamina;
                //초당 10 회복
                stamina += 1;
                time = 0.1f;
                if(stamina >= 100)
                {
                    stamina = 100;
                    staminaSlider.preStamina = stamina;
                }
            }
        }
    }
    //Hp 감소 - 몬스터 공격에서 참조하여 사용
    public void _DecreaseHp(int damage)
    {
        if(data.curHp > 0)
        {
            data.curHp -= damage;
            hpSlider.ChangeHP(data.curHp); //변동된 체력 값을 넘겨줌
            TakeDamage();
        }
    }
    //드레이크의 피로 체력 회복
    public override void RecoveryHp(PlayerState player)
    {
        if (equipData.consumeItemData.consumeItemInfo.iInfo.amount > 0)
        {
            state = player;
            anim.SetTrigger("Recovery");
            equipData.consumeItemData.equipItem.SetActive(true);
            if (equipData.shieldData.equip)
            {
                equipData.shieldData.equipShield.SetActive(false);
            }
            equipData.consumeItemData.UseItem();
        }
    }
    public void IncreaseHp(int hp)
    {
        data.curHp += hp;
        if (data.maxHp <= data.curHp)
        {
            data.curHp = data.maxHp;
        }
        hpSlider.ChangeHP(data.curHp);
    }
    #endregion

    /// <summary>
    /// 전투
    /// </summary>
    #region
    //칼 뽑기
    public override void WeaponOut(PlayerState player)
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            anim.SetBool("Sword", true);
            anim.SetTrigger("SwordOut");
            anim.SetBool("Active",true);
        }
        if (anim.GetCurrentAnimatorStateInfo(2).IsName("SwordOut"))
        {
            if (anim.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.5f && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.85f)
            {
                //등에 있는 검 오브젝트 비가시화
                //손에 있는 무기 오브젝트 가시화
                equipData.weaponData.visualize = true;
                equipData.weaponData.WeaponVisulalize();
            }
            else if (anim.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.85f)
            {
                anim.SetBool("Active", false);
                state = player;
                sword = true;
            }
        }
    }
    public override void WeaponIn(PlayerState player)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetBool("Sword", false);
            anim.SetTrigger("SwordIn");
            anim.SetBool("Active", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(2).IsName("SwordIn"))
        {
            if (anim.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.5f && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.85f)
            {
                //등에 있는 무기 오브젝트 가시화
                //손에 있는 무기 오브젝트 비가시화
                equipData.weaponData.visualize = false;
                equipData.weaponData.WeaponVisulalize();
            }
            else if (anim.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.85f)
            {
                anim.SetBool("Active", false);
                state = PlayerState.Idle;
                sword = false;
            }
        }
    }

    public void Attack(PlayerState attack, PlayerState skill)
    {
        if (jumpState != JumpState.Ground && jumpState != JumpState.Fall && Input.GetMouseButtonDown(0))
        {
            actEnd = false;
            anim.SetFloat("AttackMultiPlier", 1);
            anim.SetTrigger("JumpAttack");
        }
        //약공격
        if (jumpState == JumpState.Ground &&  Input.GetMouseButtonDown(0)) //좌클릭시
        {
            actEnd = false;
            state = attack;
            anim.SetFloat("AttackMultiPlier", 1);
            anim.SetTrigger("Attack");
        }
        //강공격
        else if (Input.GetMouseButtonDown(1) && stamina >= 70) //스태미나가 70 이상 있을 때 우클릭시
        {
            state = skill;
            actEnd = false;
            anim.SetFloat("AttackMultiPlier", 1);
            anim.SetTrigger("PowerAttack");
        }
    }
    public void AttackFunc()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(attackSound);
        equipData.attackObj.transform.localScale = new Vector3(playerSetdata.stats.atkRange, playerSetdata.stats.atkHeight, playerSetdata.stats.atkLength);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
        {
            equipData.attackObj.GetComponent<PlayerAttack>().attackPower = data.power;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack"))
        {
            equipData.attackObj.GetComponent<PlayerAttack>().attackPower = data.power * 2;
        }
        atkPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
        GameObject atk = Instantiate(equipData.attackObj, transform.position + transform.up + atkPoint * 1.4f, Quaternion.identity);
        atk.transform.parent = transform;
    }
    //패링
    public void Defend()
    {
        //패링
        if (Input.GetKeyDown(KeyCode.R) && equipData.shieldData.equip)
        {
            anim.SetTrigger("Parry"); //패링 액션
        }

        //가드
        guard = Input.GetKey(KeyCode.Q) && equipData.shieldData.equip; //Q 누르고 있는 동안 가드액션
        anim.SetBool("Guard", guard);
        if (Input.GetKeyDown(KeyCode.Q) && guard) //Q 입력시 가드 오브젝트 생성
        {
            DefendFunc();
        }
    }
    public void DefendFunc()
    {
        equipData.defendObj.transform.localScale = new Vector3(playerSetdata.stats.defendRange, playerSetdata.stats.defendHeight, playerSetdata.stats.defendLength);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Parry"))
        {
            equipData.defendObj.tag = "PlayerParry";
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Guard"))
        {
            equipData.defendObj.tag = "PlayerGuard";
        }
        guardPoint = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z).normalized;
        GameObject defend  =  Instantiate(equipData.defendObj, transform.position + transform.up + guardPoint * 0.5f, Quaternion.identity);
        defend.transform.forward = this.transform.forward;
        defend.transform.parent = transform;
    }

    //회피
    public override void Evade(PlayerState player)
    {
        state = player;
        anim.SetTrigger("Evade");
        rigid.AddForce(transform.forward * playerSetdata.stats.evadeSpeed, ForceMode.Impulse); //바라보는 방향으로 회피이동
    }
    #endregion

    /// <summary>
    /// 피격시
    /// </summary>
    #region
    //피격
    public void TakeDamage()
    {
        if (data.curHp > 0)
        {
            actEnd = false;
            anim.SetTrigger("TakeDamage");
            state = PlayerState.Damage;
        }
        else
        {
            state = PlayerState.Die;
            this.gameObject.layer = LayerMask.NameToLayer("Die");
            actEnd = false;
            Die();
        }
    }
    //그로기 상태 - 엘리트 몬스터 패링에 공격이 막혔을 때
    public void Groggy()
    {
        anim.SetTrigger("Dizzy");
    }
    #endregion

    /// <summary>
    /// 장비 관리
    /// </summary>
    #region
    //무기관리
    public void WeaponManage()
    {

    }

    //방패 관리
    public void ShieldManage()
    {

    }

    //장비 내구도 수리
    private void FixedEquipment()
    {

    }
    #endregion

    /// <summary>
    /// 상호작용
    /// </summary>
    #region
    //지면에 붙어 있는지 판단
    public void GroundJudgeMent()
    {
        if (state != PlayerState.Die)
        {
            leftRay = new Ray(transform.position - transform.right / 4, -transform.up);
            rightRay = new Ray(transform.position + transform.right / 4, -transform.up);
            frontRay = new Ray(transform.position + transform.forward / 4, -transform.up);
            backRay = new Ray(transform.position - transform.forward / 4, -transform.up);

            left = Physics.Raycast(leftRay, out lefthit, 0.4f, groundMask);
            right = Physics.Raycast(rightRay, out righthit, 0.4f, groundMask);
            front = Physics.Raycast(frontRay, out fronthit, 0.4f, groundMask);
            back = Physics.Raycast(backRay, out backhit, 0.4f, groundMask);
            //circle = Physics.SphereCast(transform.position, 0.4f, transform.position - transform.up, out sphereHit, 10, groundMask);

            //바닥에 착지 했을 때
            if (!isGround && (left || right || front || back) && 
                (state == PlayerState.Air && (jumpState == JumpState.JumpIng || jumpState == JumpState.Fall || jumpState == JumpState.Attack)))
            {
                if (state == PlayerState.Air && jumpState == JumpState.Attack)
                {
                    state = PlayerState.SwordIdle;
                    jumpState = JumpState.Ground;
                }
                if (lefthit.distance <= 0.2f || righthit.distance <= 0.2f || fronthit.distance <= 0.2f || backhit.distance <= 0.2f)
                {
                    jumpTime = 3f;
                    isGround = true;
                }
            }
            //바닥에서 추락했을 때
            else if (isGround && !left && !right && !front && !back &&  
                state != PlayerState.Air && state != PlayerState.Evade && jumpState == JumpState.Ground)
            {
                if (lefthit.distance > 0.2f || righthit.distance > 0.2f || fronthit.distance > 0.2f || backhit.distance > 0.2f)
                {
                    isGround = false;
                    Falling(PlayerState.Air, JumpState.Fall);
                }
            }
            //바닥에 착지 했을 때 상태 전환이 안되거나 추락 중이면
            /*else if(isGround && (state == PlayerState.Air || jumpState == JumpState.Jump || jumpState == JumpState.JumpIng || jumpState == JumpState.Fall))
            {
                state = PlayerState.Idle;
                jumpState = JumpState.Ground;
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
                {
                    anim.SetTrigger("HardLanding");
                }
            }*/
        }
    }
    public void InteractionSense()
    {
        bool scan = Physics.SphereCast(transform.position, senseRadius, playerView.cameraObj.forward.normalized + playerView.cameraObj.up, out interactionHit, 1, InteractionpLayerMask);
        interactionBtn.SetActive(scan && !interactive); //진입UI 비/활성화
        if (scan && Input.GetKeyDown(KeyCode.E))
        {
            interactive = true;
            if (interactionHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                state = PlayerState.Interaction;
                interactionHit.collider.transform.parent.transform.GetChild(1).gameObject.SetActive(false); //보물상자 빛 꺼주기
                //아이템 획득
                AcquireItem();
                interactive = false;
            }
            else if (interactionHit.collider.gameObject.layer == LayerMask.NameToLayer("BaseCamp"))
            {
                interactive = false;
                //베이스캠프 진입
                baseCamp.basecampTransform = interactionHit.collider.transform;
                baseCamp.ActiveBaseCampUI();
                uiManager.ActiveCursor();
                baseCamp.player = this;
                baseCamp.EnterBaseCamp();
            }
            else if (interactionHit.collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
            {
                NPCList();
            }
            else if (interactionHit.collider.gameObject.layer == LayerMask.NameToLayer("Portal"))
            {
                interactionHit.collider.gameObject.GetComponent<Portal>().SceneChange();
                interactive = false;
            }
            else if (interactionHit.collider.gameObject.layer == LayerMask.NameToLayer("Commodities"))
            {
                var _silver = interactionHit.collider.GetComponent<Commodities>().silver;
                var _acientMemorie = interactionHit.collider.GetComponent<Commodities>().acientMemorie;

                data.silver += _silver;
                data.acientMemorie += _acientMemorie;

                interactive = false;
                Destroy(interactionHit.collider.gameObject);
            }
            else
            {
                return;
            }
        }
    }
    private void NPCList()
    {
        
        if (interactionHit.collider.CompareTag("Dwarf"))
        {
            var dwarf = interactionHit.collider.gameObject.GetComponent<Dwarf>();
            dwarfUI.dwarf = dwarf;
            dwarfUI.dwarf.player = this;
            dwarfUI.player = this;
            dialogueManager.ActiveUI();
            dwarf.EnterdTalk();
        }
        else if (interactionHit.collider.CompareTag("OldElf"))
        {
            var oldElf = interactionHit.collider.gameObject.GetComponent<OldElf>();
            oldElf.player = this;
            dialogueManager.ActiveUI();
            oldElf.EnterdTalk();
        }
        else if (interactionHit.collider.CompareTag("ElfGuardCaptain"))
        {
            var elfGuardCaptain = interactionHit.collider.gameObject.GetComponent<ElfGuardCaptain>();
        }
    }
    public void OnDrawGizmosSelected()
    {
        leftRay = new Ray(transform.position - transform.right / 4, -transform.up);
        rightRay = new Ray(transform.position + transform.right / 4, -transform.up);
        frontRay = new Ray(transform.position + transform.forward / 4, -transform.up);
        backRay = new Ray(transform.position - transform.forward / 4, -transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position -transform.up * 0, 0.4f);
        Gizmos.DrawRay(leftRay);
        Gizmos.DrawRay(rightRay);
        Gizmos.DrawRay(frontRay);
        Gizmos.DrawRay(backRay);
    }
    //아이템 줍기
    public void AcquireItem()
    {
        ItemInfo itemInfo = interactionHit.collider.GetComponent<ItemInfo>();
        interactionHit.collider.gameObject.SendMessage("SetAmount", SendMessageOptions.DontRequireReceiver);
        anim.SetTrigger("Pickup");
        inventory.AddItem(itemInfo);
        interactionHit.collider.gameObject.SetActive(false);
    }
    public void UseItem()
    {
        if (equipData.consumeItemData.consumeItemInfo.iInfo.consumeKind == ConsumItem.ConsumeKind.Potion)
        {
            RecoveryHp(PlayerState.Recovery);
        }
        
    }
    #endregion

    //사망
    public void Die()
    {
        anim.SetTrigger("Die");
        actEnd = false; //액션을 취하지 못하도록
        data.acientMemorie = 0; //죽었을 때 고대의 기억을 전부 잃게함
    }
    //부활
    public void Revive()
    {
        anim.SetTrigger("Revive");
        if (!sword)
        {
            state = PlayerState.Idle;
        }
        else if (sword)
        {
            state = PlayerState.SwordIdle;
        }
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        hpSlider.SetHP();
        actEnd = true;
    }
}
