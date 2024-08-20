using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Button button; // �F��ς���{�^��
    private AudioSource buttonsound;//�{�^���̃T�E���h
    private void Start()
    {
        buttonsound = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // ��������o
        {
            ChangeButtonColor(Color.red);  // �F��ԂɕύX
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // ���������o
        {
            ChangeButtonColor(Color.white);  // �F�𔒂ɕύX
        }
    }
 
    public void change_button()
    {
        StartCoroutine(SceneChange());
    }

    // �{�^���̐F��ύX���郁�\�b�h
    void ChangeButtonColor(Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
    IEnumerator SceneChange()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }
}
