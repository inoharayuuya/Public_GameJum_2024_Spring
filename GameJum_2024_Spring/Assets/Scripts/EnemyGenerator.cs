using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("enemyのスポーン開始時間")]
    [SerializeField] private float spawntime01 = 1f;
    [Tooltip("enemyのスポーン間隔")]
    [SerializeField] public float spantime01  = 5f;
    [Tooltip("enemyを格納している親オブジェクト")]
    [SerializeField] private Transform enemyParent;

    private float tmpSpanTime = 0f;
    public GameObject P_Enemy;//enemyのオブジェクトの取得
    // Start is called before the first frame update
    void Start()
    {
        print("敵の生成");
        tmpSpanTime = spawntime01;
        //InvokeRepeating("spawn", spawntime01, spantime01);//enemyの生成
    }

    void Update()
    {
        tmpSpanTime -= Time.deltaTime;

        if (tmpSpanTime <= 0)
        {
            spawn();
            tmpSpanTime = spantime01;
        }
    }

    void spawn()
    {
        Vector3 spawnPosition = new Vector2(UnityEngine.Random.Range(-9.5f, 9.5f), UnityEngine.Random.Range(-5.5f, -9.5f));
        Instantiate(P_Enemy, spawnPosition, transform.rotation, enemyParent);
    }
}
