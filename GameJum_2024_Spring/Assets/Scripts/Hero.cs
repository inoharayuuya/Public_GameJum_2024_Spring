using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;
using UnityEngine.UI;

public class Hero : CharacterBase
{
    [Tooltip("キャラクターの所持経験値")]
    [SerializeField] private int exp;

    [Tooltip("キャラクターの次のレベルまでの必要経験値数")]
    [SerializeField] private int nextLevelExp;

    [Tooltip("HitAreaObjの親オブジェクトをセット")]
    [SerializeField] private GameObject hitAreaParent;

    [Tooltip("HP表示用スライダーをセット")]
    [SerializeField] private Slider hpSlider;

    private Rigidbody2D rb;   // Rigidbodyを取得
    private HitArea hitArea;
    private Animator animator;
    private bool isAttack;    // 攻撃が可能かどうか
    private bool isUp, isDown, isLeft, isRight;  // 方向ごとのアニメーションを管理するフラグ
    public bool IsMoving { get; set; }  // 移動可能かどうか
    
    // Start is called before the first frame update
    public override void Start()
    {
        Init();  // 初期化処理
        //GameObject enemy = GameObject.Find(Const.ENEMY_OBJ_NAME);
        //var tmp = enemy.GetComponent<Enemy>().Damage(atk);
        //print("勇者が敵に" + tmp + "のダメージを与えた");
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
    /// 初期化関数
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
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // 入力ベクトルを取得
        float moveX = Input.GetAxis("Horizontal");  // 横方向の入力(同時入力でストッピングができない)
        float moveY = Input.GetAxis("Vertical");    // 縦方向の入力(同時入力でストッピングができない)

        // 移動処理
        Vector3 position = new Vector3(moveX, moveY, 0);
        rb.velocity = position.normalized * speed;

        // 移動方向によって角度を変更する
        ChangeRotate();
    }

    /// <summary>
    /// 角度を変更する
    /// </summary>
    private void ChangeRotate()
    {
        //Vector3 attackVector = -Vector3.up;  // 勇者の初期の向きは下向き
        var scale = transform.localScale;

        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.x > 0.5f)
            {
                //print("右向き");
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
                //print("左向き");
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
                print("ストップ");
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
                //print("上向き");
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
                //print("下向き");
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
                print("ストップ");
                animator.SetBool("RIGHT", false);
                animator.SetBool("LEFT" , false);
                animator.SetBool("UP"   , false);
                animator.SetBool("DOWN" , false);
            }
        }
        else
        {
            // Idle
            print("ストップ");
            animator.SetBool("RIGHT", false);
            animator.SetBool("LEFT", false);
            animator.SetBool("UP", false);
            animator.SetBool("DOWN", false);
        }

        transform.localScale = scale;
    }

    /// <summary>
    /// マウスの入力を確認する
    /// </summary>
    private void InputMouseCheck()
    {
        // 左クリックで攻撃
        if (Input.GetMouseButtonDown(0) && isAttack)
        {
            Attack();
        }

        if (!isAttack)
        {
            //print("クールダウン中です");
        }
    }

    /// <summary>
    /// 攻撃したときの処理
    /// </summary>
    private void Attack()
    {
        //print("攻撃");
        StartCoroutine(CoolTimeCount());
        //print("hitArea:" + hitArea.AttackVector);
        //print("IsActive:" + hitArea.name);
        hitArea.SetPosition(transform.position);
        hitArea.IsFirst = true;
        hitArea.IsActive = true;
    }

    // todo ゲームオーバーを作る

    /// <summary>
    /// 勇者の攻撃力を取得する
    /// </summary>
    public int GetAtk()
    {
        return atk;
    }

    /// <summary>
    /// 勇者のHPを取得する
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
    /// 現在所持している経験値数がレベルアップに必要な経験値数を超えているかを確認
    /// </summary>
    private void ExpCheck()
    {
        // 必要経験値数を格納している配列の要素数が最大レベル
        if (level >= Common.HERO_NEXT_LEVEL_EXPS.Length)
        {
            return;
        }

        if (exp >= nextLevelExp)
        {
            print("レベルアップ");
            level++;  // レベルアップ
            exp -= nextLevelExp;  // 所持経験値数を必要経験値数分マイナスする
            nextLevelExp = Common.HERO_NEXT_LEVEL_EXPS[level - 1];  // 必要経験値数を更新

            // レベルに応じてステータス上昇
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
    /// 獲得経験値を反映
    /// </summary>
    public void RewardExp(int _reward)
    {
        exp += _reward;
    }

    /// <summary>
    /// HP回復
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
    /// 攻撃力強化
    /// </summary>
    public void EnhanceAtk(int _atkEnhance)
    {
        atk += _atkEnhance;
    }

    /// <summary>
    /// 防御力強化
    /// </summary>
    public void EnhanceDef(int _defEnhance)
    {
        def += _defEnhance;
    }

    /// <summary>
    /// 移動速度アップ
    /// </summary>
    public void EnhanceSpeed(float _speedEnhance)
    {
        speed += _speedEnhance;
    }

    /// <summary>
    /// 攻撃速度アップ
    /// </summary>
    public void EnhanceCoolTime(float _coolTimeEnhance)
    {
        coolTime -= _coolTimeEnhance;
    }
}
