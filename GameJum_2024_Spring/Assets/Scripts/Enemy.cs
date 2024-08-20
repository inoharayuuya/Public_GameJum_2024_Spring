using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class Enemy : CharacterBase
{
    [Tooltip("�|�����Ƃ��ɂ��炦��o���l��")]
    [SerializeField] protected int rewardExp;

    private Kingdom kingdom;            // kingdom�Q�[���I�u�W�F�N�g�̎擾  
    private bool isMoving = false;      // �ړ������ǂ����̃t���O
    private bool isKingdomAtk = false;  // �����ɍU���\���ǂ����̃t���O
    private bool isHeroAtk = false;     // �E�҂ɍU���\���ǂ����̃t���O
    private bool isDeath = false;       // �G�����S�������ǂ����̃t���O
    private Vector2 kingdomPos;         // �ڕW�n�_��Transform
    private float coolTimecnt;          // �N�[���^�C���̃J�E���g
    private GameObject heroObj;         // �E�҂̃I�u�W�F�N�g���Z�b�g
    private Hero hero;
    private Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        kingdom = GameObject.Find(Common.KINGDOM_OBJ_NAME).GetComponent<Kingdom>();

        // �ڕW�n�_�̍��W��ݒ�
        if (kingdom != null)
        {
            kingdomPos = new Vector2(kingdom.transform.position.x, kingdom.transform.position.y);
        }

        heroObj = GameObject.Find(Common.HERO_OBJ_NAME);
        hero = heroObj.GetComponent<Hero>();
        animator = GetComponent<Animator>();

        //GameObject kingdom = GameObject.Find(Const.KINGDOM_OBJ_NAME);
        //var tmp = kingdom.GetComponent<Kingdom>().Damage(atk);
        //print("�G�����" + tmp + "�̃_���[�W��^����");
    }

    // Update is called once per frame
    public override void Update()
    {
        coolTimecnt -= Time.deltaTime;
        if (kingdom != null && !isMoving && !isDeath)
        {
            // �E�҂Ƃ̋������ݒ肵���l�ȉ��ɂȂ�ƗE�҂̕���ǂ��悤�ɂȂ�
            if (Mathf.Abs(Vector2.Distance(heroObj.transform.position, transform.position)) < 2.5f)
            {
                // �E�҂ֈړ�
                MoveToHeroPosition();
            }
            else
            {
                // ��ֈړ�
                MoveToKingdomPosition();
            }
        }
        
        if (isKingdomAtk && coolTimecnt <= 0 && !isDeath)
        {
            if (kingdom != null )
            {
                Kingdom kingdomComponent = kingdom;
                if (kingdomComponent != null)
                {
                    kingdomComponent.Damage(atk);
                    print("HIT");
                }
                kingdom = kingdomComponent;
            }
            coolTimecnt = coolTime;
        }

        if (isHeroAtk && coolTimecnt <= 0 && !isDeath)
        {
            if (hero != null)
            {
                //Hero HeroComponent = hero;
                //if (HeroComponent != null)
                //{

                //}
                hero.Damage(atk);
                print("HIT");
                print("hero:" + hero);
                //hero = HeroComponent;
            }
            coolTimecnt = coolTime;
        }

        if (isDeath)
        {
            print("death");
            // �G������ł����ꍇ�����蔻��𑦍��ɖ���������
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        //print("HP:" + hp);

        HpCheck();

        isKingdomAtk = false;
        isHeroAtk = false;
    }

    public override int Damage(int _atk)
    {
        var tmp = _atk - def;  // �󂯂��_���[�W���v�Z

        // �󂯂��_���[�W��0�ȏゾ������U�����󂯂�
        if (tmp < 0)
        {
            return 0;
        }

        hp -= tmp;  // �󂯂�_���[�W��HP�������

        animator.SetTrigger("hit");

        return tmp;  // �󂯂��_���[�W�ʂ�Ԃ�
    }

    void MoveToKingdomPosition()
    {
        isMoving = true; // �ړ����t���O�𗧂Ă�

        // �ڕW�n�_�̍��W���g����Vector2�^�̖ڕW�ʒu���쐬
        Vector2 targetPosition = kingdomPos;

        if (Mathf.Abs(Vector2.Distance(kingdomPos, transform.position)) >= Common.ENEMY_MOVE_LIMIT)
        {
            // �G��ڕW���W�Ɍ������ē�����
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            isKingdomAtk = true;//�U���J�n�̃t���O�𗧂Ă�
        }

        isMoving = false; // �ړ��I����A�ړ����t���O����������
    }

    void MoveToHeroPosition()
    {
        isMoving = true; // �ړ����t���O�𗧂Ă�

        // �ڕW�n�_�̍��W���g����Vector2�^�̖ڕW�ʒu���쐬
        Vector2 targetPosition = heroObj.transform.position;

        if (Mathf.Abs(Vector2.Distance(heroObj.transform.position, transform.position)) >= Common.ENEMY_MOVE_LIMIT)
        {
            // �G��ڕW���W�Ɍ������ē�����
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            isHeroAtk = true;//�U���J�n�̃t���O�𗧂Ă�
        }

        isMoving = false; // �ړ��I����A�ړ����t���O����������
    }

    // �G���������Ă��邩���m�F
    private void HpCheck()
    {
        if (hp <= 0)
        {
            // �E�҂Ɋl���o���l��n��
            hero.RewardExp(rewardExp);

            animator.SetTrigger("death");

            isDeath = true;

            // �������g���폜
            //Destroy(gameObject);
            Invoke("DestroyEnemy", 0.85f);
        }
    }

    public void DestroyEnemy()
    {
        // �������g���폜
        Destroy(gameObject);
    }
}