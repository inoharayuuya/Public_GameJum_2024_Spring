using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    [Header("�����l")]
    [Tooltip("HP��(�����l)")]
    [SerializeField] private int hpHeel;

    [Tooltip("�U���͋����l(�����l)")]
    [SerializeField] private int atkEnhance;

    [Tooltip("�h��͋����l(�����l)")]
    [SerializeField] private int defEnhance;

    [Tooltip("�N�[���^�C�����ԒZ�k")]
    [SerializeField] private float coolTimeEnhance;

    [Tooltip("�ړ����x�����l(�����l)")]
    [SerializeField] private float speed;

    [Tooltip("�A�C�e���擾���̊l���o���l��(�����l)")]
    [SerializeField] private int exp;

    [Tooltip("�E�җp�̃A�C�e�����ǂ���")]
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
