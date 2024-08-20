using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    bool SceneChangeflg = false;

    [SerializeField] private AudioSource StartEnd;//使用するAudioSourceコンポーネントをアタッチ
    [SerializeField] private AudioSource cursor;//使用するAudioSourceコンポーネントをアタッチ
    [SerializeField] private AudioClip StartGame;// 使用するAudioClipをアタッチ
    [SerializeField] private AudioClip EndGame;//使用するAudioClipをアタッチ
    [SerializeField] private AudioClip Cursor;//使用するAudioClipをアタッチ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) // 下矢印キーを検出
        {
            cursor.PlayOneShot(Cursor);
            SceneChangeflg = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 上矢印キーを検出
        {
            cursor.PlayOneShot(Cursor);
            SceneChangeflg = false;
        }

        if (Input.GetKeyDown(KeyCode.Return)) // エンターキーを検出
        {
            if (SceneChangeflg == false)
            {
                StartCoroutine(ChangeScene());// シーンを変更
            }
            else
            {
                StartCoroutine(QuitGame());//ゲームの終了
            }
        }
    }
    // シーンを変更するメソッド
    IEnumerator ChangeScene()
    {
        StartEnd.PlayOneShot(StartGame);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }

    //ゲームを終了するメゾット
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
