using Const;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HitArea : MonoBehaviour
{
    [Tooltip("�ړ����x")]
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private GameObject enemy;
    private GameObject hero;

    public Vector3 AttackVector { get; set; }  // ���̃I�u�W�F�N�g�̕����x�N�^�[
    public bool    IsActive     { get; set; }  // ���̃I�u�W�F�N�g���g���Ă��邩�ǂ���
    public bool    IsFirst      { get; set; }  // IsActive��true�ɂȂ��ď��߂Ă̏������ǂ���

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
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        rb.velocity = AttackVector.normalized * speed;  // �ꉞ���K�����Ă���ړ�������
    }

    public IEnumerator CountDown()
    {
        //print("CountDown���Ă΂�܂���");
        yield return new WaitForSeconds(0.25f);
        //print("hitArea�������ʒu�ɖ߂�");
        transform.position = Common.INIT_HIT_AREA_POS;
        IsActive = false;
        IsFirst= false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������I�u�W�F�N�g�̃^�O��Enemy�������ꍇ
        if (collision.CompareTag("Enemy"))
        {
            var name = collision.name;
            enemy = GameObject.Find(name);
            print("enemy:" + name);
            transform.position = Common.INIT_HIT_AREA_POS;
            IsActive = false;
            IsFirst = false;
            var tmp = enemy.GetComponent<Enemy>().Damage(5);
            print("�E�҂��G��" + tmp + "�̃_���[�W��^����");
        }
    }
}
