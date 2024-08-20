using UnityEngine;

namespace Const
{
    public enum Phase
    {
        COUNT_DOWN_PHASE,  // 各フェーズ開始前のカウントダウンのフェーズ
        BATTLE_PHASE,      // 防衛フェーズ
        STROLLING_PHASE,   // 探索フェーズ
        NONE,              // ゲーム終了時に空にするため使う
    };

    public enum Status
    {
        GAME_START,  // ゲーム開始
        GAME_CLEAR,  // ゲームクリア
        GAME_OVER,   // ゲームオーバー
        NONE,        // ゲーム終了時に空にするため使う
    };

    public static class Common
    {
        public static string KINGDOM_OBJ_NAME  = "Kingdom";   // 王国のオブジェクトの名前を設定
        public static string ENEMY_OBJ_NAME    = "Enemy";     // 敵のオブジェクトの名前を設定
        public static string HERO_OBJ_NAME     = "Hero";      // 勇者のオブジェクトの名前を設定
        public static string RESOURCES_PATH    = "Prefabs/";  // プレハブフォルダーのパスを設定
        public static string HIT_AREA_OBJ_NAME = "HitArea";   // 当たり判定を取るオブジェクトの名前を設定

        public static readonly string[] SCENE_NAMES = {  // 使用する各シーンの名前をセット
            "TitleScene",
            "GameScene",
            "ScoreScene"
        };

        public static readonly string[] BATTLE_PHASE_MESSAGES = {
            "防衛フェーズ",
            "攻めてくる敵から王国を守り、敵を倒して勇者のレベルを上げよう！"
        };

        public static readonly string[] STROLLING_PHASE_MESSAGES = {
            "探索フェーズ",
            "ランダムに配置されたアイテムを集めて王国や勇者のレベルを上げよう！\n" +
            "レアアイテムを拾えば勇者のステータスが大幅に上昇するよ！"
        };

        public static int MAX_FPS = 60;               // 最大フレーム数を設定
        public static int TIME_LIMT = 35;             // ゲーム中の制限時間を設定

        public static readonly int[] HERO_NEXT_LEVEL_EXPS = {
            50,
            100,
            250,
            500,
            1000
        };

        public static readonly int[] KINGDOM_NEXT_LEVEL_EXPS = {
            25,
            50,
            100,
            250,
            500
        };

        public static float HIT_TIME = 0.25f;  // カウントダウンの秒数を設定
        public static float ENEMY_MOVE_LIMIT = 1.25f;  // 城かプレイヤーとの距離を比較したときの限界の距離

        public static Vector3 INIT_HIT_AREA_POS = new Vector3(1000,1000,0);  // 当たり判定を取るオブジェクトの初期座標を設定
    }
}
