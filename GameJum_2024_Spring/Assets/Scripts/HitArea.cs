using Const;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HitArea : MonoBehaviour
{
    [Tooltip("移動速度")]
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private GameObject enemy;
    private GameObject hero;

    public Vector3 AttackVector { get; set; }  // このオブジェクトの方向ベクター
    public bool    IsActive     { get; set; }  // このオブジェクトが使われているかどうか
    public bool    IsFirst      { get; set; }  // IsActiveがtrueになって初めての処理かどうか

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //print("IsActive:" + IsActive);
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            if (IsFirst)
            {
                StartCoroutine(CountDown());
            }

            IsFirst = false;
            Move();
        }
    }

    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        AttackVector = -Vector3.up;
        IsActive = false;
        IsFirst = false;

        //enemy = GameObject.Find(Const.ENEMY_OBJ_NAME);
        hero = GameObject.Find(Common.HERO_OBJ_NAME);
    }

    public void SetPosition(Vector3 _position)
    {
        transform.position = _position;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        rb.velocity = AttackVector.normalized * speed;  // 一応正規化してから移動させる
    }

    public IEnumerator CountDown()
    {
        //print("CountDownが呼ばれました");
        yield return new WaitForSeconds(0.25f);
        //print("hitAreaを初期位置に戻す");
        transform.position = Common.INIT_HIT_AREA_POS;
        IsActive = false;
        IsFirst= false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったオブジェクトのタグがEnemyだった場合
        if (collision.CompareTag("Enemy"))
        {
            var name = collision.name;
            enemy = GameObject.Find(name);
            print("enemy:" + name);
            transform.position = Common.INIT_HIT_AREA_POS;
            IsActive = false;
            IsFirst = false;
            var tmp = enemy.GetComponent<Enemy>().Damage(5);
            print("勇者が敵に" + tmp + "のダメージを与えた");
        }
    }
}
