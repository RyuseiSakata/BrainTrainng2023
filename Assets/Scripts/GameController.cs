using System.Collections;
using UnityEngine;
using Battle;


public enum EnemyType
{
    Slime,
    Minotaurosu,
    Dragon,
    Tutorial,
}

public enum EndState
{
    WIN,
    LOSE,
    FILLED,
}


public class GameController : MonoBehaviour
{
    private int _tutorialPart = 1;

    public static string playerName = "";
    [SerializeField] UIManager uIManager;
    [SerializeField] BattleUIManager battleUIManager;
    [SerializeField] Stage stage;
    [SerializeField] AudioManager audioManager;

    [SerializeField] Transform canvas;
    private DiscriptionManager discriptionManager;
    private GameObject discriptionManagerObject;
    [SerializeField] GameObject discriptionManagerPrefab;

    //�o�g���V�[���p
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    private int numberOfTurns = 0;  //�^�[����
                                    
    //�^�[�����̃v���p�e�B
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    public static int score = 0;  //�X�R�A
    public static float playerAttack;   //�����������ɂ��v���C���[�̍U����

    public static float gameTime = 0; //�Q�[���̃v���C����
    public static bool isSetTimer = false;  //���Ԃ𑪒肷�邩�̃t���O

    public static int faseCount = 0;  //���݂̃t�F�[�Y��
    private bool isNormal = false;

    //�����N�i�K���j
    public static string rank; //���݂̃����N
    [SerializeField] private int[] rankBorderes;
    [SerializeField] private string[] rankNames;

    public int FaseCount
    {
        get => faseCount;
        set
        {
            faseCount = value;
            battleUIManager.uiUpdate(UIKinds.Fase,faseCount);
        }
    }

    private EnemyType[] enemyArray = { EnemyType.Slime, EnemyType.Minotaurosu, EnemyType.Dragon };    //�o�g���̓G�̕ϐ������ԂɊi�[����z��

    private bool gameEndFlag = false;    //�Q�[���I���̃t���O
    private EndState endState = EndState.FILLED;

    private void Start()
    {
        faseCount = 0;
        score = 0;
        gameTime = 0;
        isSetTimer = false;
        StartCoroutine("mainLoop");
    }

    private void Update()
    {
        //Timer�̑�����s��
        if (isSetTimer)
        {
            gameTime += Time.deltaTime;
        }

    }

    private IEnumerator mainLoop()
    {
        

        
        if (SceneChanger.getCurrentSceneName() == "MainScene")
        {
            uIManager.configInit(); //�ݒ�̔��f
            isNormal = true;
            yield return uIManager.showCountDown();
            audioManager.playBgm(AudioKinds.BGM_Main);

            while (!gameEndFlag)
            {
                yield return stage.fall();
                NumberOfTurns++;    //�^�[�����𑝉�
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            audioManager.playSeOneShot(AudioKinds.SE_Whistle);
            yield return uIManager.showPopUp();

            SceneChanger.changeTo(SceneType.NormalResult);

        }
        // �o�g�����[�h�Ȃ�
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
            uIManager.configInit(); //�ݒ�̔��f
            isNormal = false;
            //endGame���Ă΂�Q�[���I���ƂȂ�܂�
            while (!gameEndFlag)
            {
                yield return startBattle(); //�o�g���J�n

            }

            audioManager.stopBgm();
            switch (endState)
            {
                case EndState.WIN:
                    audioManager.playSeOneShot(AudioKinds.SE_WIN);
                    yield return uIManager.showResultPanel(EndState.WIN, 3.5f);
                    break;
                case EndState.LOSE:
                    audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                    yield return uIManager.showResultPanel(EndState.LOSE, 3.5f);
                    break;
                case EndState.FILLED:
                    audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                    yield return uIManager.showResultPanel(EndState.LOSE, 3.5f);
                    break;
            }

            SceneChanger.changeTo(SceneType.AdventureResult);
            
        }
        else if (SceneChanger.getCurrentSceneName() == "TutorialScene")
        {

            bool isFilled = false;  //�u���b�N���������ꂽ���ۂ�

            do
            {
                isNormal = false;
                gameEndFlag = false; 

                //endGame���Ă΂�Q�[���I���ƂȂ�܂�
                while (!gameEndFlag)
                {
                    yield return startTutorial(); //�o�g���J�n
                }

                audioManager.stopBgm();

                switch (endState)
                {
                    case EndState.WIN:
                        audioManager.playSeOneShot(AudioKinds.SE_WIN);
                        yield return uIManager.showResultPanel(EndState.WIN, 3.5f);
                        isFilled = false;
                        break;
                    case EndState.LOSE: //�����͒ʂ�Ȃ��͂�
                        audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                        yield return uIManager.showResultPanel(EndState.LOSE, 3.5f);
                        isFilled = false;
                        break;
                    case EndState.FILLED:
                        audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                        yield return uIManager.showResultPanel(EndState.LOSE, 3.5f);
                        yield return discriptionManager.playDiscription(5); //������x�`���[�g���A�������s
                        isFilled = true;
                        stage.restart();    //�ĊJ
                        Destroy(discriptionManagerObject);
                        break;
                }

            } while (isFilled);

            yield return discriptionManager.playDiscription(4); //���[�h�̉��

            SceneChanger.changeTo(SceneType.Title);
        }

       


        yield break;
    }

    public void endGame(EndState state)
    {
        Debug.Log("endGame");
        gameEndFlag = true;
        endState = state;
        stage.GameOverFlag = true;  //�Q�[���I�[�o�[�̃t���O�𗧂Ă�
    }

    public void calculateScore(int scorePerChain, int sameEraseNum)
    {
        score += scorePerChain* sameEraseNum;
        if(stage.ChainNum > 1)
        {
            score += (stage.ChainNum - 1) * 200;
        }

        if (score > 9999999)
        {
            uIManager.textUpdate(TextKinds.Score, 9999999);    
        }
        else
        {
            uIManager.textUpdate(TextKinds.Score, score);
        }

        //�m�[�}�����[�h�Ȃ�
        if (isNormal)
        {
            string rank = getRank();
            uIManager.textUpdate(TextKinds.Rank, rank);  //�X�R�A���烉���N�����߂�
        }

    }

    public void calculateDamage(int mode = 0, int damagePerChain = 0, int sameEraseNum = 0)
    {
        //���������ɂ��_���[�W���Z�̂�
        if(mode == 0)
        {
            //�������������������Ȃ�
            //1�A��������̓��������̋����@���@sumA�~�i1+(0.5�~(����������-1)�j)
            if (sameEraseNum > 0)
            {
                playerAttack += damagePerChain * (1+(0.5f* (sameEraseNum-1)));
            }
            else
            {
                playerAttack += damagePerChain;
            }
        }
        //�A���ɂ��_���[�W���Z��UI�X�V
        else
        {
            //�A�������������Ȃ�
            //�A���ɂ�鋭�� =�i�A�����[�P�j* 1.5+sumB
            if (stage.ChainNum > 1)
            {

                playerAttack += (stage.ChainNum-1)*1.5f;
            }
        }
    }

    //�o�g�����J�n���鏈��
    private IEnumerator initBattle(EnemyType type = EnemyType.Dragon)
    {
        numberOfTurns = 0;  //�^�[������0�ɂ���
        playerAttack = 0;
        player.Init();

        enemy.Init(type);

        yield break;
    }

    private IEnumerator startBattle()
    {
        EnemyType enemyType = enemyArray[FaseCount];
        yield return initBattle(enemyType);  //�o�g���J�n

        audioManager.pauseBgm();    //BGM���ꎞ��~
        yield return uIManager.showCountDown();
        audioManager.playBgm(AudioKinds.BGM_Main);
        

        isSetTimer = true;   //���Ԃ̌v�����J�n
        /*
         * ���Ԋu�ōU������Ȃ�ȉ��̃R�[�h
        //player.StartCoroutine("action");
        //enemy.StartCoroutine("action");
        */

        //yield return enemy.attack(player, EnemyAttackKinds.First);

        while (true)
        {
            yield return stage.fall();  //�u���b�N�̗�������

            NumberOfTurns++;    //�^�[�����𑝉�

            if (gameEndFlag)
            {
                break;
            }

            //�����̏�����@�܂��@�^�[���ɂ��U�����s���ꍇ
            yield return player.attack(enemy);

            //���񂾂��̏���
            if (enemy.HpAmount <= 0f)
            {
                yield return uIManager.showPopUp("You Win", 1.5f);  //�����̗]�C�ɐZ�鎞��
                FaseCount++;    //���̃t�F�[�Y��
                //���ׂĂ̓G�ɏ������Ȃ�
                if (enemyArray.Length <= FaseCount)
                {
                    endGame(EndState.WIN);
                }

                break;
            }

            yield return new WaitForSeconds(0.2f);
            yield return enemy.action(player);
            yield return player.attack(enemy);

            //���񂾂��̏���
            if (enemy.HpAmount <= 0f)
            {
                
                yield return uIManager.showPopUp("You Win", 1.5f);  //�����̗]�C�ɐZ�鎞��
                FaseCount++;    //���̃t�F�[�Y��
                //���ׂĂ̓G�ɏ������Ȃ�
                if (enemyArray.Length <= FaseCount)
                {
                    endGame(EndState.WIN);
                }

                break;
            }
            if (player.HpAmount <= 0f)
            {
                endGame(EndState.LOSE);
                break;

            }

        }
        isSetTimer = false; //���Ԃ̌v���𒆒f
        yield break;
    }

    private IEnumerator startTutorial()
    {
        //�����I�u�W�F�N�g�̐���
        discriptionManagerObject = Instantiate(discriptionManagerPrefab, canvas);
        discriptionManager = discriptionManagerObject.GetComponent<DiscriptionManager>();

        yield return initBattle(EnemyType.Tutorial);  //�o�g���J�n
        _tutorialPart = 1;
        gameEndFlag = false;

        audioManager.stopSe();
        audioManager.playBgm(AudioKinds.BGM_Main);
        

        while (true)
        {
            
            switch (_tutorialPart)
            {
                case 1:
                    yield return discriptionManager.playDiscription(1);
                    _tutorialPart = 2;
                    continue;

                case 2: //1���[�v�ҋ@
                    yield return stage.fallTutorial();  //�u���b�N�̗�������
                    _tutorialPart = 3;
                    break;

                case 3: //�P�����낤��\��
                    yield return discriptionManager.playDiscription(2);
                    _tutorialPart = 4;
                    continue;

                case 4: //�P�ꂪ������܂Ńv���C
                    yield return stage.fallTutorial();  //�u���b�N�̗�������
                    if (playerAttack > 0) _tutorialPart = 5;//�U�������� == �u���b�N��������
                    break;

                case 5: //�������ˁA�N���A���悤�̕\��
                    yield return discriptionManager.playDiscription(3);
                    _tutorialPart = 99;
                    continue;

                default:
                    yield return stage.fallTutorial();  //�u���b�N�̗�������
                    if (playerAttack > 0) _tutorialPart = 4;//�U�������� == �u���b�N��������
                    break;

            }

            NumberOfTurns++;    //�^�[�����𑝉�

            if (gameEndFlag)
            {
                break;
            }

            //�����̏�����@�܂��@�^�[���ɂ��U�����s���ꍇ
            yield return player.attack(enemy);

            //���񂾂��̏���
            if (enemy.HpAmount <= 0f)
            {
                //yield return uIManager.showPopUp("You Win", 1.5f);  //�����̗]�C�ɐZ�鎞��
                endGame(EndState.WIN);
                break;
            }

            yield return new WaitForSeconds(0.2f);
            yield return enemy.action(player);
            yield return player.attack(enemy);

            //���񂾂��̏���
            if (enemy.HpAmount <= 0f)
            {
                //yield return uIManager.showPopUp("You Win", 1.5f);  //�����̗]�C�ɐZ�鎞��
                endGame(EndState.WIN);
                break;
            }
            
            if (player.HpAmount <= 0f)
            {
                endGame(EndState.LOSE);
                break;
            }

        }
        
        yield break;
    }

    public string getRank()
    {
        string ret = rankNames[0];
        for(int i = 0; i < rankBorderes.Length; i++)
        {
            if (score >= rankBorderes[i])
            {
                ret = rankNames[i];
            }
            else
            {
                break;
            }
        }
        rank = ret;
        Debug.Log(ret + rank);
        return ret;
    }
}

