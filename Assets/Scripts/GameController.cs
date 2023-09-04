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

    //バトルシーン用
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    private int numberOfTurns = 0;  //ターン数
    private GameState gameState = GameState.Playing;    //ゲームの状態

    private string finishText = "Finish";

    //ターン数のプロパティ
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
                NumberOfTurns++;    //ターン数を増加
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            yield return uiManager.showFinish();

            SceneChanger.changeTo(SceneType.Result);

        }
        // バトルモードなら
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
            actorInit();

            yield return uiManager.showCountDown();

            /*
             * 一定間隔で攻撃するなら以下のコード
            //player.StartCoroutine("action");
            //enemy.StartCoroutine("action");
            */

            while (gameState == GameState.Playing)
            {
                yield return stage.fall();  //ブロックの落下処理

                NumberOfTurns++;    //ターン数を増加
                Debug.Log("Turn:" + NumberOfTurns + ",Combo:" + stage.ComboNum);

                //文字の消え具合　また　ターンにより攻撃を行う場合
                yield return player.attack(enemy, PlayerAttackKinds.Combo);
                yield return new WaitForSeconds(0.2f);
                yield return enemy.attack(player, EnemyAttackKinds.Normal);

                //死んだかの処理
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


    //プレイヤーと敵の初期化
    private void actorInit()
    {
        player.Init();
        enemy.Init(Random.Range(3,10));
    }
}

