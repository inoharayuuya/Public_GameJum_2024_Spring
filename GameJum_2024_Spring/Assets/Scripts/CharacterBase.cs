using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class CharacterBase : MonoBehaviour
{
    [Header("キャラクターのステータス")]
    [Tooltip("キャラクターのHP")]
    [SerializeField] protected int hp;

    [Tooltip("キャラクターの攻撃力")]
    [SerializeField] protected int atk;

    [Tooltip("キャラクターの防御力")]
    [SerializeField] protected int def;

    [Tooltip("キャラクターの攻撃のクールタイム")]
    [SerializeField] protected float coolTime;

    [Tooltip("キャラクターの移動速度")]
    [SerializeField] protected float speed;

    [Tooltip("キャラクターのレベル")]
    [SerializeField] protected int level;

    protected float hitEffectNum;  // 攻撃を受けた時のリアクション用
    protected bool  isRed;  // 現在のカラーが赤かどうか
    protected bool  isHit;
    protected int   maxHp;  // 最大HP

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //if (isHit)
        //{
        //    HitEffect();
        //}

        //print("isHit:" + isHit);
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    public virtual int Damage(int _atk)
    {
        //print("ダメージ");
        var tmp = _atk - def;  // 受けたダメージを計算

        // 受けたダメージが0以上だったら攻撃を受ける
        if (tmp < 0)
        {
            return 0;
        }

        hp -= tmp;  // 受けるダメージ分HPから引く

        StartCoroutine(HitEffectTime());

        return tmp;  // 受けたダメージ量を返す
    }

    /// <summary>
    /// リアクションをする秒数
    /// </summary>
    private IEnumerator HitEffectTime()
    {
        isHit = true;

        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = Color.white;
        
        isHit = false;
    }
}
