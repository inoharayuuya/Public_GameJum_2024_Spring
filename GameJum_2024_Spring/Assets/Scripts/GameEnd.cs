using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public Button button; // 色を変えるボタン


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 上矢印を検出
        {
            ChangeButtonColor(Color.white);  // 色を白に変更
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 下矢印を検出
        {
            ChangeButtonColor(Color.red);  // 色を赤に変更
        }
    }
    // ボタンの色を変更するメソッド
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
