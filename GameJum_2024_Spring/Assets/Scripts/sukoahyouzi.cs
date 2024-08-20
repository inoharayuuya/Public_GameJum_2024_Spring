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

        // スコアを前の画面から受け取る
        int receivedScore = GetScoreFromPreviousScene();

        // 3秒後にスコアを表示するコルーチンを開始
        StartCoroutine(hyouzi(receivedScore));
    }

    // Update is called once per frame
    void Update()
    {
        // 何か処理があればここに記述
    }

    // スコアを前の画面から受け取るメソッド
    int GetScoreFromPreviousScene()
    {
        //ここをゲーム画面でとったスコアを受け取るようにする(この場合は100にしている)
        return 100;
    }

    // 3秒後にスコアを表示するコルーチン
    IEnumerator hyouzi(int score)
    {
        yield return new WaitForSeconds(1.0f);  // 1秒経ったら進む
        GetComponent<Text>().text = score.ToString();    // スコアを表示文字に変更
    }

    // タイトルボタンを推した時
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
