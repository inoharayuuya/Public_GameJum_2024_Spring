using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Const;

public class sukoahyouzi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "";

        // �X�R�A��O�̉�ʂ���󂯎��
        int receivedScore = GetScoreFromPreviousScene();

        // 3�b��ɃX�R�A��\������R���[�`�����J�n
        StartCoroutine(hyouzi(receivedScore));
    }

    // Update is called once per frame
    void Update()
    {
        // ��������������΂����ɋL�q
    }

    // �X�R�A��O�̉�ʂ���󂯎�郁�\�b�h
    int GetScoreFromPreviousScene()
    {
        //�������Q�[����ʂłƂ����X�R�A���󂯎��悤�ɂ���(���̏ꍇ��100�ɂ��Ă���)
        return 100;
    }

    // 3�b��ɃX�R�A��\������R���[�`��
    IEnumerator hyouzi(int score)
    {
        yield return new WaitForSeconds(1.0f);  // 1�b�o������i��
        GetComponent<Text>().text = score.ToString();    // �X�R�A��\�������ɕύX
    }

    // �^�C�g���{�^���𐄂�����
    public void PushTitleButton()
    {
        StartCoroutine(SceneChange());
    }
    IEnumerator SceneChange()
    {
        //GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(Common.SCENE_NAMES[0]);
    }
}
