using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;
using UnityEngine.UI;

public class Hero : CharacterBase
{
    [Tooltip("�L�����N�^�[�̏����o���l")]
    [SerializeField] private int exp;

    [Tooltip("�L�����N�^�[�̎��̃��x���܂ł̕K�v�o���l��")]
    [SerializeField] private int nextLevelExp;

    [Tooltip("HitAreaObj�̐e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject hitAreaParent;

    [Tooltip("HP�\���p�X���C�_�[���Z�b�g")]
    [SerializeField] private Slider hpSlider;

    private Rigidbody2D rb;   // Rigidbody���擾
    private HitArea hitArea;
    private Animator animator;
    private bool isAttack;    // �U�����\���ǂ���
    private bool isUp, isDown, isLeft, isRight;  // �������Ƃ̃A�j���[�V�������Ǘ�����t���O
    public bool IsMoving { get; set; }  // �ړ��\���ǂ���
    
    // Start is called before the first frame update
    public override void Start()
    {
        Init();  // ����������
        //GameObject enemy = GameObject.Find(Const.ENEMY_OBJ_NAME);
        //var tmp = enemy.GetComponent<Enemy>().Damage(atk);
        //print("�E�҂��G��" + tmp + "�̃_���[�W��^����");
    }

    // Update is called once per frame
    public override void Update()
    {
        if (IsMoving)
        {
            InputMouseCheck();
        }

        hpSlider.value = hp;

        ExpCheck();
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// �������֐�
    /// </summary>
    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        hitArea = hitAreaParent.GetComponentInChildren<HitArea>();
        animator = GetComponent<Animator>();
        isAttack = true;
        IsMoving = false;
        hpSlider.maxValue = hp;
        nextLevelExp = Common.HERO_NEXT_LEVEL_EXPS[level - 1];
        maxHp = hp;
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        // ���̓x�N�g�����擾
        float moveX = Input.GetAxis("Horizontal");  // �������̓���(�������͂ŃX�g�b�s���O���ł��Ȃ�)
        float moveY = Input.GetAxis("Vertical");    // �c�����̓���(�������͂ŃX�g�b�s���O���ł��Ȃ�)

        // �ړ�����
        Vector3 position = new Vector3(moveX, moveY, 0);
        rb.velocity = position.normalized * speed;

        // �ړ������ɂ���Ċp�x��ύX����
        ChangeRotate();
    }

    /// <summary>
    /// �p�x��ύX����
    /// </summary>
    private void ChangeRotate()
    {
        //Vector3 attackVector = -Vector3.up;  // �E�҂̏����̌����͉�����
        var scale = transform.localScale;

        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.x > 0.5f)
            {
                //print("�E����");
                if (!hitArea.IsActive)
                {
                    hitArea.AttackVector = Vector3.right;
                }
                animator.SetBool("RIGHT", true);
                animator.SetBool("LEFT" , false);
                animator.SetBool("UP"   , false);
                animator.SetBool("DOWN" , false);
                //scale.x = -1;
            }
            else if(rb.velocity.x < 0.5f)
            {
                //print("������");
                if (!hitArea.IsActive)
                {
                    hitArea.AttackVector = -Vector3.right;
                }
                animator.SetBool("RIGHT", false);
                animator.SetBool("LEFT" , true);
                animator.SetBool("UP"   , false);
                animator.SetBool("DOWN" , false);
                //scale.x = 1;
            }
            else
            {
                // Idle
                print("�X�g�b�v");
                animator.SetBool("RIGHT", false);
                animator.SetBool("LEFT" , false);
                animator.SetBool("UP"   , false);
                animator.SetBool("DOWN" , false);
            }
        }
        else if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.y > 0.5f)
            {
                //print("�����");
                if (!hitArea.IsActive)
                {
                    hitArea.AttackVector = Vector3.up;
                }
                animator.SetBool("RIGHT", false);
                animator.SetBool("LEFT" , false);
                animator.SetBool("UP"   , true);
                animator.SetBool("DOWN" , false);
                //scale.y = 1;
            }
            else if(rb.velocity.y < 0.5f)
            {
                //print("������");
                if (!hitArea.IsActive)
                {
                    hitArea.AttackVector = -Vector3.up;
                }
                animator.SetBool("RIGHT", false);
                animator.SetBool("LEFT" , false);
                animator.SetBool("UP"   , false);
                animator.SetBool("DOWN" , true);
                //scale.y = -1;
            }
            else
            {
                // Idle
                print("�X�g�b�v");
                animator.SetBool("RIGHT", false);
                animator.SetBool("LEFT" , false);
                animator.SetBool("UP"   , false);
                animator.SetBool("DOWN" , false);
            }
        }
        else
        {
            // Idle
            print("�X�g�b�v");
            animator.SetBool("RIGHT", false);
            animator.SetBool("LEFT", false);
            animator.SetBool("UP", false);
            animator.SetBool("DOWN", false);
        }

        transform.localScale = scale;
    }

    /// <summary>
    /// �}�E�X�̓��͂��m�F����
    /// </summary>
    private void InputMouseCheck()
    {
        // ���N���b�N�ōU��
        if (Input.GetMouseButtonDown(0) && isAttack)
        {
            Attack();
        }

        if (!isAttack)
        {
            //print("�N�[���_�E�����ł�");
        }
    }

    /// <summary>
    /// �U�������Ƃ��̏���
    /// </summary>
    private void Attack()
    {
        //print("�U��");
        StartCoroutine(CoolTimeCount());
        //print("hitArea:" + hitArea.AttackVector);
        //print("IsActive:" + hitArea.name);
        hitArea.SetPosition(transform.position);
        hitArea.IsFirst = true;
        hitArea.IsActive = true;
    }

    // todo �Q�[���I�[�o�[�����

    /// <summary>
    /// �E�҂̍U���͂��擾����
    /// </summary>
    public int GetAtk()
    {
        return atk;
    }

    /// <summary>
    /// �E�҂�HP���擾����
    /// </summary>
    public int GetHp()
    {
        return hp;
    }

    private IEnumerator CoolTimeCount()
    {
        isAttack = false;
        yield return new WaitForSeconds(coolTime);
        isAttack = true;
    }

    /// <summary>
    /// ���ݏ������Ă���o���l�������x���A�b�v�ɕK�v�Ȍo���l���𒴂��Ă��邩���m�F
    /// </summary>
    private void ExpCheck()
    {
        // �K�v�o���l�����i�[���Ă���z��̗v�f�����ő僌�x��
        if (level >= Common.HERO_NEXT_LEVEL_EXPS.Length)
        {
            return;
        }

        if (exp >= nextLevelExp)
        {
            print("���x���A�b�v");
            level++;  // ���x���A�b�v
            exp -= nextLevelExp;  // �����o���l����K�v�o���l�����}�C�i�X����
            nextLevelExp = Common.HERO_NEXT_LEVEL_EXPS[level - 1];  // �K�v�o���l�����X�V

            // ���x���ɉ����ăX�e�[�^�X�㏸
            switch (level)
            {
                case 2:
                    atk += 2;
                    break;

                case 3:
                    def += 1;
                    break;

                case 4:
                    atk += 3;
                    break;
                
                case 5:
                    atk += 2;
                    def += 1;
                    break;
            }
        }
    }

    /// <summary>
    /// �l���o���l�𔽉f
    /// </summary>
    public void RewardExp(int _reward)
    {
        exp += _reward;
    }

    /// <summary>
    /// HP��
    /// </summary>
    public void HpHeel(int _hpHeel)
    {
        hp += _hpHeel;

        if (hp >= 50)
        {
            hp = 50;
        }
    }

    /// <summary>
    /// �U���͋���
    /// </summary>
    public void EnhanceAtk(int _atkEnhance)
    {
        atk += _atkEnhance;
    }

    /// <summary>
    /// �h��͋���
    /// </summary>
    public void EnhanceDef(int _defEnhance)
    {
        def += _defEnhance;
    }

    /// <summary>
    /// �ړ����x�A�b�v
    /// </summary>
    public void EnhanceSpeed(float _speedEnhance)
    {
        speed += _speedEnhance;
    }

    /// <summary>
    /// �U�����x�A�b�v
    /// </summary>
    public void EnhanceCoolTime(float _coolTimeEnhance)
    {
        coolTime -= _coolTimeEnhance;
    }
}
