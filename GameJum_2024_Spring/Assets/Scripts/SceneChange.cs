using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    bool SceneChangeflg = false;

    [SerializeField] private AudioSource StartEnd;//�g�p����AudioSource�R���|�[�l���g���A�^�b�`
    [SerializeField] private AudioSource cursor;//�g�p����AudioSource�R���|�[�l���g���A�^�b�`
    [SerializeField] private AudioClip StartGame;// �g�p����AudioClip���A�^�b�`
    [SerializeField] private AudioClip EndGame;//�g�p����AudioClip���A�^�b�`
    [SerializeField] private AudioClip Cursor;//�g�p����AudioClip���A�^�b�`
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) // �����L�[�����o
        {
            cursor.PlayOneShot(Cursor);
            SceneChangeflg = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) // ����L�[�����o
        {
            cursor.PlayOneShot(Cursor);
            SceneChangeflg = false;
        }

        if (Input.GetKeyDown(KeyCode.Return)) // �G���^�[�L�[�����o
        {
            if (SceneChangeflg == false)
            {
                StartCoroutine(ChangeScene());// �V�[����ύX
            }
            else
            {
                StartCoroutine(QuitGame());//�Q�[���̏I��
            }
        }
    }
    // �V�[����ύX���郁�\�b�h
    IEnumerator ChangeScene()
    {
        StartEnd.PlayOneShot(StartGame);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }

    //�Q�[�����I�����郁�]�b�g
    IEnumerator QuitGame()
    {
        StartEnd.PlayOneShot(EndGame);
        yield return new WaitForSeconds(1);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
