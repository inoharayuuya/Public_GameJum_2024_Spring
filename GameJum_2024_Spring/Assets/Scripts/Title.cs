using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Button button; // 色を変えるボタン
    private AudioSource buttonsound;//ボタンのサウンド
    private void Start()
    {
        buttonsound = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 上矢印を検出
        {
            ChangeButtonColor(Color.red);  // 色を赤に変更
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 下矢印を検出
        {
            ChangeButtonColor(Color.white);  // 色を白に変更
        }
    }
 
    public void change_button()
    {
        StartCoroutine(SceneChange());
    }

    // ボタンの色を変更するメソッド
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
