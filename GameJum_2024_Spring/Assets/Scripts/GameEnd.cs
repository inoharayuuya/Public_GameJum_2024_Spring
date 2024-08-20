using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public Button button; // �F��ς���{�^��


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // ��������o
        {
            ChangeButtonColor(Color.white);  // �F�𔒂ɕύX
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // ���������o
        {
            ChangeButtonColor(Color.red);  // �F��ԂɕύX
        }
    }
    // �{�^���̐F��ύX���郁�\�b�h
    void ChangeButtonColor(Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
    public void QuitGame()
    {
        StartCoroutine(GameEndChange());
    }
    IEnumerator GameEndChange()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
