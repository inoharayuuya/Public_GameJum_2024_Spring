using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    [Header("強化値")]
    [Tooltip("HP回復(実数値)")]
    [SerializeField] private int hpHeel;

    [Tooltip("攻撃力強化値(実数値)")]
    [SerializeField] private int atkEnhance;

    [Tooltip("防御力強化値(実数値)")]
    [SerializeField] private int defEnhance;

    [Tooltip("クールタイム時間短縮")]
    [SerializeField] private float coolTimeEnhance;

    [Tooltip("移動速度強化値(実数値)")]
    [SerializeField] private float speed;

    [Tooltip("アイテム取得時の獲得経験値数(実数値)")]
    [SerializeField] private int exp;

    [Tooltip("勇者用のアイテムかどうか")]
    [SerializeField] private bool isHero;

    private GameObject heroObj;
    private Hero hero;
    private GameObject kingdomObj;
    private Kingdom kingdom;

    // Start is called before the first frame update
    void Start()
    {
        heroObj = GameObject.Find(Common.HERO_OBJ_NAME);
        hero = heroObj.GetComponent<Hero>();
        kingdomObj = GameObject.Find(Common.KINGDOM_OBJ_NAME);
        kingdom = kingdomObj.GetComponent<Kingdom>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Hero")
        {
            if (isHero)
            {
                hero.EnhanceAtk(atkEnhance);
                hero.EnhanceSpeed(speed);
                hero.EnhanceCoolTime(coolTimeEnhance);
            }
            else
            {
                hero.EnhanceDef(defEnhance);
                kingdom.EnhanceDef(defEnhance);
                hero.HpHeel(hpHeel);
                kingdom.HpHeel(hpHeel);
            }
            hero.RewardExp(exp);
            kingdom.RewardExp(exp);

            Destroy(gameObject);
        }
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
