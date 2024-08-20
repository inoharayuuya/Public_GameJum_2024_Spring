using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class CharacterBase : MonoBehaviour
{
    [Header("�L�����N�^�[�̃X�e�[�^�X")]
    [Tooltip("�L�����N�^�[��HP")]
    [SerializeField] protected int hp;

    [Tooltip("�L�����N�^�[�̍U����")]
    [SerializeField] protected int atk;

    [Tooltip("�L�����N�^�[�̖h���")]
    [SerializeField] protected int def;

    [Tooltip("�L�����N�^�[�̍U���̃N�[���^�C��")]
    [SerializeField] protected float coolTime;

    [Tooltip("�L�����N�^�[�̈ړ����x")]
    [SerializeField] protected float speed;

    [Tooltip("�L�����N�^�[�̃��x��")]
    [SerializeField] protected int level;

    protected float hitEffectNum;  // �U�����󂯂����̃��A�N�V�����p
    protected bool  isRed;  // ���݂̃J���[���Ԃ��ǂ���
    protected bool  isHit;
    protected int   maxHp;  // �ő�HP

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
    /// �U�����󂯂����̏���
    /// </summary>
    public virtual int Damage(int _atk)
    {
        //print("�_���[�W");
        var tmp = _atk - def;  // �󂯂��_���[�W���v�Z

        // �󂯂��_���[�W��0�ȏゾ������U�����󂯂�
        if (tmp < 0)
        {
            return 0;
        }

        hp -= tmp;  // �󂯂�_���[�W��HP�������

        StartCoroutine(HitEffectTime());

        return tmp;  // �󂯂��_���[�W�ʂ�Ԃ�
    }

    /// <summary>
    /// ���A�N�V����������b��
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
