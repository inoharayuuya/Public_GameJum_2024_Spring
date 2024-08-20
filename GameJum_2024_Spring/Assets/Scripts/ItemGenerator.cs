using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Tooltip("�e�A�C�e���̃v���n�u���Z�b�g")]
    [SerializeField] private GameObject[] P_Items;

    [Tooltip("�A�C�e���ő��")]
    [SerializeField] int Itemcnt;

    [Tooltip("�A�C�e���I�u�W�F�N�g���i�[���Ă���e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] Transform itemObjParent;

    float[] RarityInfo = new float[5] {44,20,20,15,1};
    Vector2 spawnPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void Init()
    {
        spawnPosition = Vector2.zero;
    }

    IEnumerator span()
    {
        //�X�|�[���n�_�̐ݒ�
        //Vector2 spawnPosition = new Vector2(Random.Range(-11f, 11f), Random.Range(4.5f, -4.5f));

        for (int i = 0; i < Itemcnt; i++)
        {
            GetDropItem();
            yield return null;
        }

        for (int allcnt = 0; allcnt < Itemcnt; allcnt++)
        {//4��J��Ԃ�
            yield return null;
        }

    }

    void GetDropItem()
    {
        //���A�x�̒��I
        int itemNumber = ChooseRarity();
        spawnPosition = new Vector2(Random.Range(-14f, 14f), Random.Range(9f, -9f));

        Instantiate(P_Items[itemNumber], spawnPosition, transform.rotation, itemObjParent);//(�I�u�W�F�̐ݒ�,�������W�̐ݒ�,�����p�x�̐ݒ�)
        print("����");

        Debug.Log("itemNumber" + itemNumber);
    }

    int ChooseRarity()
    {
        //�m���̍��v�l���i�[
        float total = 0;

        //�m�������v����
        for (int i = 0; i < RarityInfo.GetLength(0); i++)
        {
            total += RarityInfo[i];
        }

        //Random.value�ł�0����1�܂ł�float�l��Ԃ��̂�
        //�����Ƀh���b�v���̍��v���|����
        float randomPoint = Random.value * total;

        //randomPoint�̈ʒu�ɊY������L�[��Ԃ�
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
    /// �A�C�e���̐���
    /// </summary>
    public void ItemInstance()
    {
        StartCoroutine("span");
    }
}
