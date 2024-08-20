using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Tooltip("各アイテムのプレハブをセット")]
    [SerializeField] private GameObject[] P_Items;

    [Tooltip("アイテム最大量")]
    [SerializeField] int Itemcnt;

    [Tooltip("アイテムオブジェクトを格納している親オブジェクトをセット")]
    [SerializeField] Transform itemObjParent;

    float[] RarityInfo = new float[5] {44,20,20,15,1};
    Vector2 spawnPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Init()
    {
        spawnPosition = Vector2.zero;
    }

    IEnumerator span()
    {
        //スポーン地点の設定
        //Vector2 spawnPosition = new Vector2(Random.Range(-11f, 11f), Random.Range(4.5f, -4.5f));

        for (int i = 0; i < Itemcnt; i++)
        {
            GetDropItem();
            yield return null;
        }

        for (int allcnt = 0; allcnt < Itemcnt; allcnt++)
        {//4回繰り返す
            yield return null;
        }

    }

    void GetDropItem()
    {
        //レア度の抽選
        int itemNumber = ChooseRarity();
        spawnPosition = new Vector2(Random.Range(-14f, 14f), Random.Range(9f, -9f));

        Instantiate(P_Items[itemNumber], spawnPosition, transform.rotation, itemObjParent);//(オブジェの設定,生成座標の設定,生成角度の設定)
        print("生成");

        Debug.Log("itemNumber" + itemNumber);
    }

    int ChooseRarity()
    {
        //確率の合計値を格納
        float total = 0;

        //確率を合計する
        for (int i = 0; i < RarityInfo.GetLength(0); i++)
        {
            total += RarityInfo[i];
        }

        //Random.valueでは0から1までのfloat値を返すので
        //そこにドロップ率の合計を掛ける
        float randomPoint = Random.value * total;

        //randomPointの位置に該当するキーを返す
        for (int i = 0; i < RarityInfo.GetLength(0); i++)
        {
            if (randomPoint < RarityInfo[i])
            {
                return i;
            }
            else
            {
                randomPoint -= RarityInfo[i];
            }
        }
        return 0;
    }

    /// <summary>
    /// アイテムの生成
    /// </summary>
    public void ItemInstance()
    {
        StartCoroutine("span");
    }
}
