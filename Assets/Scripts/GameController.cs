using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;


public enum EnemyType
{
    Slime,
    Minotaurosu,
    Dragon,
}

public enum EndState
{
    WIN,
    LOSE,
    FILLED,
}


public class GameController : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] BattleUIManager battleUIManager;
    [SerializeField] Stage stage;
    [SerializeField] AudioManager audioManager;

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

    private int score = 0;  //�X�R�A
    public static float playerAttack;   //�����������ɂ��v���C���[�̍U����

    public static float gameTime = 0; //�Q�[���̃v���C����
    public static bool isSetTimer = false;  //���Ԃ𑪒肷�邩�̃t���O

    private int faseCount = 0;  //���݂̃t�F�[�Y��
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
        uIManager.configInit(); //�ݒ�̔��f

        if (SceneChanger.getCurrentSceneName() == "MainScene")
        {
            yield return uIManager.showCountDown();
            audioManager.playBgm(AudioKinds.BGM_Main);

            while (!gameEndFlag)
            {
                yield return stage.fall();
                NumberOfTurns++;    //�^�[�����𑝉�
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            yield return uIManager.showPopUp();

            SceneChanger.changeTo(SceneType.Result);

        }
        // �o�g�����[�h�Ȃ�
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
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
                    yield return uIManager.showPopUp("CLEAR", 3.5f);
                    break;
                case EndState.LOSE:
                    audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                    yield return uIManager.showPopUp("MISS", 3.5f);
                    break;
                case EndState.FILLED:
                    audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                    yield return uIManager.showPopUp("MISS", 3.5f);
                    break;
            }

            SceneChanger.changeTo(SceneType.Result);
            
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

        if (score > 999999)
        {
            uIManager.textUpdate(TextKinds.Score, 999999);
        }
        else
        {
            uIManager.textUpdate(TextKinds.Score, score);
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
                    FaseCount--;
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
                    FaseCount--;
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

}

