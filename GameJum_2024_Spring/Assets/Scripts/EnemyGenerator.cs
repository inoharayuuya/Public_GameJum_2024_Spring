using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("enemy�̃X�|�[���J�n����")]
    [SerializeField] private float spawntime01 = 1f;
    [Tooltip("enemy�̃X�|�[���Ԋu")]
    [SerializeField] public float spantime01  = 5f;
    [Tooltip("enemy���i�[���Ă���e�I�u�W�F�N�g")]
    [SerializeField] private Transform enemyParent;

    private float tmpSpanTime = 0f;
    public GameObject P_Enemy;//enemy�̃I�u�W�F�N�g�̎擾
    // Start is called before the first frame update
    void Start()
    {
        print("�G�̐���");
        tmpSpanTime = spawntime01;
        //InvokeRepeating("spawn", spawntime01, spantime01);//enemy�̐���
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
