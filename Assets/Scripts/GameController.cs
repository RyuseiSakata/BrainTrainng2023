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

    private int score = 0;
    public static int playerAttack;   //消した文字によるプレイヤーの攻撃量

    //ターン数のプロパティ
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
                yield return player.attack(enemy, PlayerAttackKinds.Word);
                yield return new WaitForSeconds(0.2f);
                yield return enemy.attack(player, EnemyAttackKinds.Obstacle);

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
        //同時消しによるダメージ加算のみ
        if(mode == 0)
        {
            //同時消しが発生したなら
            if (sameEraseNum > 1)
            {
                playerAttack += damagePerChain * 2;
            }
            else
            {
                playerAttack += damagePerChain;
            }
        }
        //連鎖によるダメージ加算とUI更新
        else
        {
            //連鎖が発生したなら
            if(stage.ChainNum > 2)
            {
                playerAttack *= 2;
            }
        }
    }
}

