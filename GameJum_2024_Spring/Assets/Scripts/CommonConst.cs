using UnityEngine;

namespace Const
{
    public enum Phase
    {
        COUNT_DOWN_PHASE,  // �e�t�F�[�Y�J�n�O�̃J�E���g�_�E���̃t�F�[�Y
        BATTLE_PHASE,      // �h�q�t�F�[�Y
        STROLLING_PHASE,   // �T���t�F�[�Y
        NONE,              // �Q�[���I�����ɋ�ɂ��邽�ߎg��
    };

    public enum Status
    {
        GAME_START,  // �Q�[���J�n
        GAME_CLEAR,  // �Q�[���N���A
        GAME_OVER,   // �Q�[���I�[�o�[
        NONE,        // �Q�[���I�����ɋ�ɂ��邽�ߎg��
    };

    public static class Common
    {
        public static string KINGDOM_OBJ_NAME  = "Kingdom";   // �����̃I�u�W�F�N�g�̖��O��ݒ�
        public static string ENEMY_OBJ_NAME    = "Enemy";     // �G�̃I�u�W�F�N�g�̖��O��ݒ�
        public static string HERO_OBJ_NAME     = "Hero";      // �E�҂̃I�u�W�F�N�g�̖��O��ݒ�
        public static string RESOURCES_PATH    = "Prefabs/";  // �v���n�u�t�H���_�[�̃p�X��ݒ�
        public static string HIT_AREA_OBJ_NAME = "HitArea";   // �����蔻������I�u�W�F�N�g�̖��O��ݒ�

        public static readonly string[] SCENE_NAMES = {  // �g�p����e�V�[���̖��O���Z�b�g
            "TitleScene",
            "GameScene",
            "ScoreScene"
        };

        public static readonly string[] BATTLE_PHASE_MESSAGES = {
            "�h�q�t�F�[�Y",
            "�U�߂Ă���G���牤�������A�G��|���ėE�҂̃��x�����グ�悤�I"
        };

        public static readonly string[] STROLLING_PHASE_MESSAGES = {
            "�T���t�F�[�Y",
            "�����_���ɔz�u���ꂽ�A�C�e�����W�߂ĉ�����E�҂̃��x�����グ�悤�I\n" +
            "���A�A�C�e�����E���ΗE�҂̃X�e�[�^�X���啝�ɏ㏸�����I"
        };

        public static int MAX_FPS = 60;               // �ő�t���[������ݒ�
        public static int TIME_LIMT = 35;             // �Q�[�����̐������Ԃ�ݒ�

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

        public static float HIT_TIME = 0.25f;  // �J�E���g�_�E���̕b����ݒ�
        public static float ENEMY_MOVE_LIMIT = 1.25f;  // �邩�v���C���[�Ƃ̋������r�����Ƃ��̌��E�̋���

        public static Vector3 INIT_HIT_AREA_POS = new Vector3(1000,1000,0);  // �����蔻������I�u�W�F�N�g�̏������W��ݒ�
    }
}
