using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Const;
using System.Threading;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Tooltip("HitAreaObjの親オブジェクトをセット")]
    [SerializeField] private GameObject hitAreaParent;

    [Tooltip("カウントダウンで使うオブジェクトをセット")]
    [SerializeField] private GameObject countDownObj;

    [Tooltip("防衛フェーズで使うオブジェクトをセット")]
    [SerializeField] private GameObject battleObj;

    [Tooltip("探索フェーズで使うオブジェクトをセット")]
    [SerializeField] private GameObject strollingObj;

    [Tooltip("メッセージ表示用テキストをセット")]
    [SerializeField] private GameObject dispMessageObj;

    [Tooltip("敵のジェネレーターをセット")]
    [SerializeField] private GameObject enemyGenerator;

    [Tooltip("アイテムのジェネレーターをセット")]
    [SerializeField] private GameObject itemGenerator;

    [Tooltip("敵オブジェクトを格納しているオブジェクトをセット")]
    [SerializeField] private GameObject enemyObj;

    [Tooltip("アイテムオブジェクトを格納しているオブジェクトをセット")]
    [SerializeField] private GameObject itemObj;

    [Tooltip("各フェーズの開始前に表示するテキストをセット")]
    [SerializeField] private Text[] texts;

    [Tooltip("防衛フェーズ中に表示するテキストをセット")]
    [SerializeField] private Text[] battleTexts;

    [Tooltip("探索フェーズ中に表示するテキストをセット")]
    [SerializeField] private Text[] strollingTexts;

    [Tooltip("ゲームクリアかゲームオーバーを表示するパネルをセット")]
    [SerializeField] private GameObject[] resultPanels;

    private GameObject hitArea;  // 攻撃範囲のオブジェクト
    private Phase phase;  // フェーズを管理する為の変数
    private Status status;
    private float countDownTime = Common.TIME_LIMT;
    private Text countDownText;
    private Text battleText;
    private Text strollingText;
    private int phaseCnt;
    private bool isBattlePhase;  // 戦闘フェーズかどうか
    private GameObject heroObj;
    private Hero hero;
    private GameObject kingdomObj;
    private Kingdom kingdom;
    private bool isFirst;
    private bool isCursorLocked;  // マウスカーソルが固定されているかどうか

    private void Awake()
    {
        // マウスカーソルを固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 最大フレームレートを固定
        Application.targetFrameRate = Common.MAX_FPS;

        // 当たり判定を取るオブジェクトを読み込んで生成
        hitArea = (GameObject)Resources.Load(Common.RESOURCES_PATH + Common.HIT_AREA_OBJ_NAME);
        Instantiate(hitArea, Common.INIT_HIT_AREA_POS, Quaternion.identity, hitAreaParent.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();  // 初期化
    }

    // Update is called once per frame
    void Update()
    {
        StatusCheck();  // 現在のステータスを確認

        KeyCheck();  // 入力されたキーを確認

        HpCheck();  // 勇者と王国のHPを確認
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Init()
    {
        phase = Phase.BATTLE_PHASE;
        countDownText = countDownObj.GetComponentInChildren<Text>();
        battleText = battleObj.GetComponentInChildren<Text>();
        strollingText = strollingObj.GetComponentInChildren<Text>();
        countDownObj.SetActive(false);
        battleObj.SetActive(false);
        strollingObj.SetActive(false);
        dispMessageObj.SetActive(false);
        phaseCnt = 1;
        isBattlePhase = true;
        status = Status.GAME_START;  // ゲームが開始するのでステータスをゲームスタートに変更
        enemyGenerator.SetActive(false);
        heroObj = GameObject.Find(Common.HERO_OBJ_NAME);
        hero = heroObj.GetComponent<Hero>();
        kingdomObj = GameObject.Find(Common.KINGDOM_OBJ_NAME);
        kingdom = kingdomObj.GetComponent<Kingdom>();
        isFirst = true;
        isCursorLocked = true;

        // パネルを全て非表示
        foreach (var panel in resultPanels)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// 現在のフェーズを確認する
    /// </summary>
    private void PhaseCheck()
    {
        switch (phase)
        {
            case Phase.COUNT_DOWN_PHASE:
                print("カウントダウンフェーズです");
                //CountDown();  // カウントダウン開始
                break;

            case Phase.BATTLE_PHASE:
                print("防衛フェーズです");
                BattleManager();  // 戦闘開始
                break;

            case Phase.STROLLING_PHASE:
                print("探索フェーズです");
                StrollingManager();  // 散策開始
                break;
        }

        print("phaseCnt:" + phaseCnt);
    }

    /// <summary>
    /// 現在のステータスを確認する
    /// </summary>
    private void StatusCheck()
    {
        switch (status)
        {
            case Status.GAME_START:
                print("ゲーム開始");
                PhaseCheck();   // 現在のフェーズを確認
                break;

            case Status.GAME_CLEAR:
                print("ゲームクリア");
                StartCoroutine(GameClear());
                break;

            case Status.GAME_OVER:
                print("ゲームオーバー");
                StartCoroutine(GameOver());
                break;
        }
    }

    /// <summary>
    /// キーが入力されているかを確認
    /// </summary>
    private void KeyCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorLocked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isCursorLocked = false;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isCursorLocked = true;
            }
        }
    }

    /// <summary>
    /// 勇者と王国のHPを確認する
    /// </summary>
    private void HpCheck()
    {
        var heroHp = hero.GetHp();
        var kingdomHp = kingdom.GetHp();

        // 勇者か王国のHPが0になったらゲームオーバーにする
        if (heroHp <= 0 || kingdomHp <= 0)
        {
            status = Status.GAME_OVER;  // ステータスをゲームオーバーに変更
        }
    }

    /// <summary>
    /// カウントダウンをする
    /// </summary>
    //private void CountDown()
    //{
    //    countDownTime -= Time.deltaTime;

    //    if (isFirst)
    //    {
    //        StartCoroutine(DispMessage(isBattlePhase));
    //        isFirst = false;
    //    }

    //    if (countDownTime > 1)
    //    {
    //        countDownText.text = ((int)countDownTime).ToString();
    //    }
    //    else if (countDownTime > 0)
    //    {
    //        countDownText.text = "スタート";
    //    }
    //    else
    //    {
    //        countDownObj.SetActive(false);
    //        countDownTime = Const.TIME_LIMT;  // カウントダウンを初期化
    //        battleObj.SetActive(true);
    //        phaseCnt++;
    //    }
    //}

    /// <summary>
    /// 防衛フェーズを管理する
    /// </summary>
    private void BattleManager()
    {
        countDownTime -= Time.deltaTime;

        if (isFirst)
        {
            StartCoroutine(DispMessage(isBattlePhase));
            isFirst = false;
        }

        if (countDownTime > 6)
        {
            Color color = Color.white;
            battleText.color = color;
            battleText.text = ((int)countDownTime).ToString();
        }
        else if(countDownTime > 1)
        {
            Color color = Color.red;
            battleText.color = color;
            battleText.text = ((int)countDownTime).ToString();
        }
        //else if (countDownTime > 0)
        //{
        //    battleText.text = "終了";
        //}
        else
        {
            battleObj.SetActive(false);
            countDownTime = Common.TIME_LIMT;  // カウントダウンを初期化
            enemyGenerator.SetActive(false);
            DestroyEnemy();  // ステージ内に残った敵を一掃
            
            if (phaseCnt >= 3)
            {
                battleObj.SetActive(false);
                enemyGenerator.SetActive(false);
                phase = Phase.NONE;
                status = Status.GAME_CLEAR;  // 設定した回数ループしたらゲーム終了
                return;
            }

            itemGenerator.GetComponent<ItemGenerator>().ItemInstance();
            isBattlePhase = false;
            isFirst = true;
            phase = Phase.STROLLING_PHASE;  // フェーズが終了したのでフェーズを進行させる
        }
    }

    /// <summary>
    /// 探索フェーズを管理する
    /// </summary>
    private void StrollingManager()
    {
        countDownTime -= Time.deltaTime;

        if (isFirst)
        {
            StartCoroutine(DispMessage(isBattlePhase));
            isFirst = false;
        }

        if (countDownTime > 6)
        {
            Color color = Color.white;
            strollingText.color = color;
            strollingText.text = ((int)countDownTime).ToString();
        }
        else if (countDownTime > 1)
        {
            Color color = Color.red;
            strollingText.color = color;
            strollingText.text = ((int)countDownTime).ToString();
        }
        //else if (countDownTime > 0)
        //{
        //    strollingText.text = "終了";
        //}
        else
        {
            strollingObj.SetActive(false);
            countDownTime = Common.TIME_LIMT;  // カウントダウンを初期化
            phase = Phase.BATTLE_PHASE;  // フェーズが終了したのでフェーズを進行させる
            DestroyItem();
            phaseCnt++;
            isBattlePhase = true;
            isFirst = true;
        }
    }

    /// <summary>
    /// 各フェーズの開始前に表示するメッセージ
    /// </summary>
    private IEnumerator DispMessage(bool isBattlePhase)
    {
        hero.IsMoving = false;
        dispMessageObj.SetActive(true);

        if (isBattlePhase)
        {
            // 防衛フェーズの開始前の場合
            for (int i = 0; i < texts.Length; i++)
            {
                if (i == 0)
                {
                    texts[i].color = Color.red;
                    battleTexts[i].color = Color.red;
                }
                texts[i].text = Common.BATTLE_PHASE_MESSAGES[i];
                battleTexts[i].text = Common.BATTLE_PHASE_MESSAGES[i];
            }

            phase = Phase.BATTLE_PHASE;
        }
        else
        {
            // 探索フェーズの開始前の場合
            for (int i = 0; i < texts.Length; i++)
            {
                if (i == 0)
                {
                    texts[i].color = Color.green;
                    strollingTexts[i].color = Color.green;
                }
                texts[i].text = Common.STROLLING_PHASE_MESSAGES[i];
                strollingTexts[i].text = Common.STROLLING_PHASE_MESSAGES[i];
            }

            phase = Phase.STROLLING_PHASE;
        }

        yield return new WaitForSeconds(4);

        dispMessageObj.SetActive(false);

        countDownObj.SetActive(true);
        countDownText.text = "スタート";

        yield return new WaitForSeconds(1);

        hero.IsMoving = true;
        countDownObj.SetActive(false);

        if (isBattlePhase)
        {
            battleObj.SetActive(true);
            enemyGenerator.SetActive(true);
        }
        else
        {
            strollingObj.SetActive(true);
        }
    }

    /// <summary>
    /// 防衛フェーズが終了した瞬間に敵を消す
    /// </summary>
    private void DestroyEnemy()
    {
        print("敵を一掃");
        var enemyObjects = enemyObj.GetComponentsInChildren<Enemy>();
        foreach (var enemyObject in enemyObjects)
        {
            enemyObject.DestroyEnemy();
        }
    }

    /// <summary>
    /// 探索フェーズが終了した瞬間にアイテムを消す
    /// </summary>
    private void DestroyItem()
    {
        print("アイテム全消し");
        var itemObjects = itemObj.GetComponentsInChildren<Item>();
        foreach (var itemObject in itemObjects)
        {
            itemObject.DestroyItem();
        }
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    private IEnumerator GameClear()
    {
        resultPanels[0].SetActive(true);

        yield return new WaitForSeconds(3);
        
        // マウスカーソルを表示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // シーンチェンジ
        SceneManager.LoadScene(Common.SCENE_NAMES[2]);
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    private IEnumerator GameOver()
    {
        resultPanels[1].SetActive(true);

        yield return new WaitForSeconds(3);

        // マウスカーソルを表示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // シーンチェンジ
        SceneManager.LoadScene(Common.SCENE_NAMES[2]);
    }
}
