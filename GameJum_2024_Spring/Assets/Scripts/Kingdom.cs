using Const;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Kingdom : CharacterBase
{
    [Tooltip("キャラクターの所持経験値")]
    [SerializeField] private int exp;

    [Tooltip("キャラクターの次のレベルまでの必要経験値数")]
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
    /// 現在所持している経験値数がレベルアップに必要な経験値数を超えているかを確認
    /// </summary>
    private void ExpCheck()
    {
        // 必要経験値数を格納している配列の要素数が最大レベル
        if (level >= Common.KINGDOM_NEXT_LEVEL_EXPS.Length)
        {
            return;
        }

        if (exp >= nextLevelExp)
        {
            print("レベルアップ");
            level++;  // レベルアップ
            exp -= nextLevelExp;  // 所持経験値数を必要経験値数分マイナスする
            nextLevelExp = Common.KINGDOM_NEXT_LEVEL_EXPS[level - 1];  // 必要経験値数を更新

            // レベルに応じてステータス上昇
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
    /// 城のHPを取得する
    /// </summary>
    public int GetHp()
    {
        return hp;
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

        if (hp >= 100)
        {
            hp = 100;
        }
    }

    /// <summary>
    /// 防御力強化
    /// </summary>
    public void EnhanceDef(int _defEnhance)
    {
        def += _defEnhance;
    }
}
