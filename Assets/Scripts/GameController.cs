using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public enum GameState {
    Playing,
    GameOver,
}


public class GameController : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] Stage stage;

    //�o�g���V�[���p
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    private int numberOfTurns = 0;  //�^�[����
    private GameState gameState = GameState.Playing;    //�Q�[���̏��

    private string finishText = "Finish";

    private int score = 0;
    public static int playerAttack;   //�����������ɂ��v���C���[�̍U����

    //�^�[�����̃v���p�e�B
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    private void Start()
    {
        score = 0;
        playerAttack = 0;
        StartCoroutine("mainLoop");
    }

    private IEnumerator mainLoop()
    {
        if(SceneChanger.getCurrentSceneName() == "MainScene")
        {
            yield return uiManager.showCountDown();

            while (gameState == GameState.Playing)
            {
                yield return stage.fall();
                NumberOfTurns++;    //�^�[�����𑝉�
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            yield return uiManager.showFinish();

            SceneChanger.changeTo(SceneType.Result);

        }
        // �o�g�����[�h�Ȃ�
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
            actorInit();

            yield return uiManager.showCountDown();

            /*
             * ���Ԋu�ōU������Ȃ�ȉ��̃R�[�h
            //player.StartCoroutine("action");
            //enemy.StartCoroutine("action");
            */

            while (gameState == GameState.Playing)
            {
                yield return stage.fall();  //�u���b�N�̗�������

                NumberOfTurns++;    //�^�[�����𑝉�
                Debug.Log("Turn:" + NumberOfTurns + ",Combo:" + stage.ComboNum);

                //�����̏�����@�܂��@�^�[���ɂ��U�����s���ꍇ
                yield return player.attack(enemy, PlayerAttackKinds.Word);
                yield return new WaitForSeconds(0.2f);
                yield return enemy.attack(player, EnemyAttackKinds.Obstacle);

                //���񂾂��̏���
                if(enemy.HpAmount <= 0f)
                {
                    finishText = "You Win";
                    gameOver();
                }
                if (player.HpAmount <= 0f)
                {
                    finishText = "You Lose";
                    gameOver();
                }
            }

            yield return uiManager.showFinish(finishText);

            SceneChanger.changeTo(SceneType.Result);
            
        }

        yield break;
    }

    public void gameOver()
    {
        Debug.Log("game over");
        gameState = GameState.GameOver;
    }


    //�v���C���[�ƓG�̏�����
    private void actorInit()
    {
        player.Init();
        enemy.Init(10);
    }

    public void calculateScore(int scorePerChain, int sameEraseNum)
    {
        score += scorePerChain* sameEraseNum;
        if(stage.ChainNum > 1)
        {
            score += (stage.ChainNum - 1) * 200;
        }
        
        uiManager.textUpdate(TextKinds.Score, score);
    }

    public void calculateDamage(int mode = 0, int damagePerChain = 0, int sameEraseNum = 0)
    {
        //���������ɂ��_���[�W���Z�̂�
        if(mode == 0)
        {
            //�������������������Ȃ�
            if (sameEraseNum > 1)
            {
                playerAttack += damagePerChain * 2;
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
            if(stage.ChainNum > 2)
            {
                playerAttack *= 2;
            }
        }
    }
}

