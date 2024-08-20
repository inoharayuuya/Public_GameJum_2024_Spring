using Const;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Kingdom : CharacterBase
{
    [Tooltip("�L�����N�^�[�̏����o���l")]
    [SerializeField] private int exp;

    [Tooltip("�L�����N�^�[�̎��̃��x���܂ł̕K�v�o���l��")]
    [SerializeField] private int nextLevelExp;

    private float distance;
    private Slider hpSlider;

    // Start is called before the first frame update
    public override void Start()
    {
        hpSlider = GameObject.Find("SliderKingdomHp").GetComponent<Slider>();
        distance = hpSlider.transform.position.y - transform.position.y;
        nextLevelExp = Common.KINGDOM_NEXT_LEVEL_EXPS[level - 1];
        hpSlider.maxValue = hp;
        maxHp = hp;
    }

    // Update is called once per frame
    public override void Update()
    {
        hpSlider.value = hp;
        var pos = new Vector2(transform.position.x, transform.position.y + distance);
        hpSlider.transform.position = pos;

        ExpCheck();
    }

    /// <summary>
    /// ���ݏ������Ă���o���l�������x���A�b�v�ɕK�v�Ȍo���l���𒴂��Ă��邩���m�F
    /// </summary>
    private void ExpCheck()
    {
        // �K�v�o���l�����i�[���Ă���z��̗v�f�����ő僌�x��
        if (level >= Common.KINGDOM_NEXT_LEVEL_EXPS.Length)
        {
            return;
        }

        if (exp >= nextLevelExp)
        {
            print("���x���A�b�v");
            level++;  // ���x���A�b�v
            exp -= nextLevelExp;  // �����o���l����K�v�o���l�����}�C�i�X����
            nextLevelExp = Common.KINGDOM_NEXT_LEVEL_EXPS[level - 1];  // �K�v�o���l�����X�V

            // ���x���ɉ����ăX�e�[�^�X�㏸
            switch (level)
            {
                case 2:
                    def += 1;
                    break;

                case 3:
                    def += 1;
                    //var changeMaxHp = ((float)hp / (float)maxHp);
                    //print("changeMaxHp:" + changeMaxHp);
                    //var _maxHp = maxHp + 50;
                    //print("_maxHp:" + _maxHp);
                    //hpSlider.maxValue = _maxHp;
                    //hp = (int)(_maxHp * changeMaxHp);
                    //hpSlider.value = hp;
                    //print("hp:" + hp);
                    break;

                case 4:
                    def += 1;
                    break;

                case 5:
                    def += 1;
                    break;
            }
        }
    }

    /// <summary>
    /// ���HP���擾����
    /// </summary>
    public int GetHp()
    {
        return hp;
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

        if (hp >= 100)
        {
            hp = 100;
        }
    }

    /// <summary>
    /// �h��͋���
    /// </summary>
    public void EnhanceDef(int _defEnhance)
    {
        def += _defEnhance;
    }
}
