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

    //�^�[�����̃v���p�e�B
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    private void Start()
    {
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
            }

            yield return uiManager.showFinish();

            SceneChanger.changeTo(SceneType.Result);

        }
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
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
                yield return player.attack(enemy, PlayerAttackKinds.Combo);
                yield return enemy.attack(player, EnemyAttackKinds.Normal);

                //���񂾂��̏���
            }

            yield return uiManager.showFinish();


            SceneChanger.changeTo(SceneType.Result);
            
        }

        yield break;
    }

    public void gameOver()
    {
        Debug.Log("game over");
        gameState = GameState.GameOver;
    }
}

