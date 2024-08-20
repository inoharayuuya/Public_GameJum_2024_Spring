using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class Enemy : CharacterBase
{
    [Tooltip("倒したときにもらえる経験値数")]
    [SerializeField] protected int rewardExp;

    private Kingdom kingdom;            // kingdomゲームオブジェクトの取得  
    private bool isMoving = false;      // 移動中かどうかのフラグ
    private bool isKingdomAtk = false;  // 王国に攻撃可能かどうかのフラグ
    private bool isHeroAtk = false;     // 勇者に攻撃可能かどうかのフラグ
    private bool isDeath = false;       // 敵が死亡したかどうかのフラグ
    private Vector2 kingdomPos;         // 目標地点のTransform
    private float coolTimecnt;          // クールタイムのカウント
    private GameObject heroObj;         // 勇者のオブジェクトをセット
    private Hero hero;
    private Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        kingdom = GameObject.Find(Common.KINGDOM_OBJ_NAME).GetComponent<Kingdom>();

        // 目標地点の座標を設定
        if (kingdom != null)
        {
            kingdomPos = new Vector2(kingdom.transform.position.x, kingdom.transform.position.y);
        }

        heroObj = GameObject.Find(Common.HERO_OBJ_NAME);
        hero = heroObj.GetComponent<Hero>();
        animator = GetComponent<Animator>();

        //GameObject kingdom = GameObject.Find(Const.KINGDOM_OBJ_NAME);
        //var tmp = kingdom.GetComponent<Kingdom>().Damage(atk);
        //print("敵が城に" + tmp + "のダメージを与えた");
    }

    // Update is called once per frame
    public override void Update()
    {
        coolTimecnt -= Time.deltaTime;
        if (kingdom != null && !isMoving && !isDeath)
        {
            // 勇者との距離が設定した値以下になると勇者の方を追うようになる
            if (Mathf.Abs(Vector2.Distance(heroObj.transform.position, transform.position)) < 2.5f)
            {
                // 勇者へ移動
                MoveToHeroPosition();
            }
            else
            {
                // 城へ移動
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
            // 敵が死んでいた場合当たり判定を即座に無効化する
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        //print("HP:" + hp);

        HpCheck();

        isKingdomAtk = false;
        isHeroAtk = false;
    }

    public override int Damage(int _atk)
    {
        var tmp = _atk - def;  // 受けたダメージを計算

        // 受けたダメージが0以上だったら攻撃を受ける
        if (tmp < 0)
        {
            return 0;
        }

        hp -= tmp;  // 受けるダメージ分HPから引く

        animator.SetTrigger("hit");

        return tmp;  // 受けたダメージ量を返す
    }

    void MoveToKingdomPosition()
    {
        isMoving = true; // 移動中フラグを立てる

        // 目標地点の座標を使ってVector2型の目標位置を作成
        Vector2 targetPosition = kingdomPos;

        if (Mathf.Abs(Vector2.Distance(kingdomPos, transform.position)) >= Common.ENEMY_MOVE_LIMIT)
        {
            // 敵を目標座標に向かって動かす
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            isKingdomAtk = true;//攻撃開始のフラグを立てる
        }

        isMoving = false; // 移動終了後、移動中フラグを解除する
    }

    void MoveToHeroPosition()
    {
        isMoving = true; // 移動中フラグを立てる

        // 目標地点の座標を使ってVector2型の目標位置を作成
        Vector2 targetPosition = heroObj.transform.position;

        if (Mathf.Abs(Vector2.Distance(heroObj.transform.position, transform.position)) >= Common.ENEMY_MOVE_LIMIT)
        {
            // 敵を目標座標に向かって動かす
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            isHeroAtk = true;//攻撃開始のフラグを立てる
        }

        isMoving = false; // 移動終了後、移動中フラグを解除する
    }

    // 敵が生存しているかを確認
    private void HpCheck()
    {
        if (hp <= 0)
        {
            // 勇者に獲得経験値を渡す
            hero.RewardExp(rewardExp);

            animator.SetTrigger("death");

            isDeath = true;

            // 自分自身を削除
            //Destroy(gameObject);
            Invoke("DestroyEnemy", 0.85f);
        }
    }

    public void DestroyEnemy()
    {
        // 自分自身を削除
        Destroy(gameObject);
    }
}