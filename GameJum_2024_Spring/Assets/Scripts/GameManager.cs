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
    [Tooltip("HitAreaObj�̐e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject hitAreaParent;

    [Tooltip("�J�E���g�_�E���Ŏg���I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject countDownObj;

    [Tooltip("�h�q�t�F�[�Y�Ŏg���I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject battleObj;

    [Tooltip("�T���t�F�[�Y�Ŏg���I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject strollingObj;

    [Tooltip("���b�Z�[�W�\���p�e�L�X�g���Z�b�g")]
    [SerializeField] private GameObject dispMessageObj;

    [Tooltip("�G�̃W�F�l���[�^�[���Z�b�g")]
    [SerializeField] private GameObject enemyGenerator;

    [Tooltip("�A�C�e���̃W�F�l���[�^�[���Z�b�g")]
    [SerializeField] private GameObject itemGenerator;

    [Tooltip("�G�I�u�W�F�N�g���i�[���Ă���I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject enemyObj;

    [Tooltip("�A�C�e���I�u�W�F�N�g���i�[���Ă���I�u�W�F�N�g���Z�b�g")]
    [SerializeField] private GameObject itemObj;

    [Tooltip("�e�t�F�[�Y�̊J�n�O�ɕ\������e�L�X�g���Z�b�g")]
    [SerializeField] private Text[] texts;

    [Tooltip("�h�q�t�F�[�Y���ɕ\������e�L�X�g���Z�b�g")]
    [SerializeField] private Text[] battleTexts;

    [Tooltip("�T���t�F�[�Y���ɕ\������e�L�X�g���Z�b�g")]
    [SerializeField] private Text[] strollingTexts;

    [Tooltip("�Q�[���N���A���Q�[���I�[�o�[��\������p�l�����Z�b�g")]
    [SerializeField] private GameObject[] resultPanels;

    private GameObject hitArea;  // �U���͈͂̃I�u�W�F�N�g
    private Phase phase;  // �t�F�[�Y���Ǘ�����ׂ̕ϐ�
    private Status status;
    private float countDownTime = Common.TIME_LIMT;
    private Text countDownText;
    private Text battleText;
    private Text strollingText;
    private int phaseCnt;
    private bool isBattlePhase;  // �퓬�t�F�[�Y���ǂ���
    private GameObject heroObj;
    private Hero hero;
    private GameObject kingdomObj;
    private Kingdom kingdom;
    private bool isFirst;
    private bool isCursorLocked;  // �}�E�X�J�[�\�����Œ肳��Ă��邩�ǂ���

    private void Awake()
    {
        // �}�E�X�J�[�\�����Œ�
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // �ő�t���[�����[�g���Œ�
        Application.targetFrameRate = Common.MAX_FPS;

        // �����蔻������I�u�W�F�N�g��ǂݍ���Ő���
        hitArea = (GameObject)Resources.Load(Common.RESOURCES_PATH + Common.HIT_AREA_OBJ_NAME);
        Instantiate(hitArea, Common.INIT_HIT_AREA_POS, Quaternion.identity, hitAreaParent.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();  // ������
    }

    // Update is called once per frame
    void Update()
    {
        StatusCheck();  // ���݂̃X�e�[�^�X���m�F

        KeyCheck();  // ���͂��ꂽ�L�[���m�F

        HpCheck();  // �E�҂Ɖ�����HP���m�F
    }

    /// <summary>
    /// ����������
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
        status = Status.GAME_START;  // �Q�[�����J�n����̂ŃX�e�[�^�X���Q�[���X�^�[�g�ɕύX
        enemyGenerator.SetActive(false);
        heroObj = GameObject.Find(Common.HERO_OBJ_NAME);
        hero = heroObj.GetComponent<Hero>();
        kingdomObj = GameObject.Find(Common.KINGDOM_OBJ_NAME);
        kingdom = kingdomObj.GetComponent<Kingdom>();
        isFirst = true;
        isCursorLocked = true;

        // �p�l����S�Ĕ�\��
        foreach (var panel in resultPanels)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// ���݂̃t�F�[�Y���m�F����
    /// </summary>
    private void PhaseCheck()
    {
        switch (phase)
        {
            case Phase.COUNT_DOWN_PHASE:
                print("�J�E���g�_�E���t�F�[�Y�ł�");
                //CountDown();  // �J�E���g�_�E���J�n
                break;

            case Phase.BATTLE_PHASE:
                print("�h�q�t�F�[�Y�ł�");
                BattleManager();  // �퓬�J�n
                break;

            case Phase.STROLLING_PHASE:
                print("�T���t�F�[�Y�ł�");
                StrollingManager();  // �U��J�n
                break;
        }

        print("phaseCnt:" + phaseCnt);
    }

    /// <summary>
    /// ���݂̃X�e�[�^�X���m�F����
    /// </summary>
    private void StatusCheck()
    {
        switch (status)
        {
            case Status.GAME_START:
                print("�Q�[���J�n");
                PhaseCheck();   // ���݂̃t�F�[�Y���m�F
                break;

            case Status.GAME_CLEAR:
                print("�Q�[���N���A");
                StartCoroutine(GameClear());
                break;

            case Status.GAME_OVER:
                print("�Q�[���I�[�o�[");
                StartCoroutine(GameOver());
                break;
        }
    }

    /// <summary>
    /// �L�[�����͂���Ă��邩���m�F
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
    /// �E�҂Ɖ�����HP���m�F����
    /// </summary>
    private void HpCheck()
    {
        var heroHp = hero.GetHp();
        var kingdomHp = kingdom.GetHp();

        // �E�҂�������HP��0�ɂȂ�����Q�[���I�[�o�[�ɂ���
        if (heroHp <= 0 || kingdomHp <= 0)
        {
            status = Status.GAME_OVER;  // �X�e�[�^�X���Q�[���I�[�o�[�ɕύX
        }
    }

    /// <summary>
    /// �J�E���g�_�E��������
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
    //        countDownText.text = "�X�^�[�g";
    //    }
    //    else
    //    {
    //        countDownObj.SetActive(false);
    //        countDownTime = Const.TIME_LIMT;  // �J�E���g�_�E����������
    //        battleObj.SetActive(true);
    //        phaseCnt++;
    //    }
    //}

    /// <summary>
    /// �h�q�t�F�[�Y���Ǘ�����
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
        //    battleText.text = "�I��";
        //}
        else
        {
            battleObj.SetActive(false);
            countDownTime = Common.TIME_LIMT;  // �J�E���g�_�E����������
            enemyGenerator.SetActive(false);
            DestroyEnemy();  // �X�e�[�W���Ɏc�����G����|
            
            if (phaseCnt >= 3)
            {
                battleObj.SetActive(false);
                enemyGenerator.SetActive(false);
                phase = Phase.NONE;
                status = Status.GAME_CLEAR;  // �ݒ肵���񐔃��[�v������Q�[���I��
                return;
            }

            itemGenerator.GetComponent<ItemGenerator>().ItemInstance();
            isBattlePhase = false;
            isFirst = true;
            phase = Phase.STROLLING_PHASE;  // �t�F�[�Y���I�������̂Ńt�F�[�Y��i�s������
        }
    }

    /// <summary>
    /// �T���t�F�[�Y���Ǘ�����
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
        //    strollingText.text = "�I��";
        //}
        else
        {
            strollingObj.SetActive(false);
            countDownTime = Common.TIME_LIMT;  // �J�E���g�_�E����������
            phase = Phase.BATTLE_PHASE;  // �t�F�[�Y���I�������̂Ńt�F�[�Y��i�s������
            DestroyItem();
            phaseCnt++;
            isBattlePhase = true;
            isFirst = true;
        }
    }

    /// <summary>
    /// �e�t�F�[�Y�̊J�n�O�ɕ\�����郁�b�Z�[�W
    /// </summary>
    private IEnumerator DispMessage(bool isBattlePhase)
    {
        hero.IsMoving = false;
        dispMessageObj.SetActive(true);

        if (isBattlePhase)
        {
            // �h�q�t�F�[�Y�̊J�n�O�̏ꍇ
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
            // �T���t�F�[�Y�̊J�n�O�̏ꍇ
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
        countDownText.text = "�X�^�[�g";

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
    /// �h�q�t�F�[�Y���I�������u�ԂɓG������
    /// </summary>
    private void DestroyEnemy()
    {
        print("�G����|");
        var enemyObjects = enemyObj.GetComponentsInChildren<Enemy>();
        foreach (var enemyObject in enemyObjects)
        {
            enemyObject.DestroyEnemy();
        }
    }

    /// <summary>
    /// �T���t�F�[�Y���I�������u�ԂɃA�C�e��������
    /// </summary>
    private void DestroyItem()
    {
        print("�A�C�e���S����");
        var itemObjects = itemObj.GetComponentsInChildren<Item>();
        foreach (var itemObject in itemObjects)
        {
            itemObject.DestroyItem();
        }
    }

    /// <summary>
    /// �Q�[���N���A����
    /// </summary>
    private IEnumerator GameClear()
    {
        resultPanels[0].SetActive(true);

        yield return new WaitForSeconds(3);
        
        // �}�E�X�J�[�\����\��
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // �V�[���`�F���W
        SceneManager.LoadScene(Common.SCENE_NAMES[2]);
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    private IEnumerator GameOver()
    {
        resultPanels[1].SetActive(true);

        yield return new WaitForSeconds(3);

        // �}�E�X�J�[�\����\��
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // �V�[���`�F���W
        SceneManager.LoadScene(Common.SCENE_NAMES[2]);
    }
}
