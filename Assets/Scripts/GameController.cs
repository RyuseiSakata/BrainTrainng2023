using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;


public enum EnemyType
{
    Fase1,
    Fase2,
    Dragon,
}


public class GameController : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] Stage stage;

    //バトルシーン用
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    private int numberOfTurns = 0;  //ターン数

    private string popUpText = "Finish";

    private int score = 0;
    public static float playerAttack;   //消した文字によるプレイヤーの攻撃量

    //ターン数のプロパティ
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    private int faseCount = 0;  //現在のフェーズ数
    private EnemyType[] enemyArray = { EnemyType.Fase1, EnemyType.Fase2, EnemyType.Dragon };    //バトルの敵の変数を順番に格納する配列

    private bool gameEndFlag = false;    //ゲーム終了のフラグ
    private void Start()
    {
        score = 0;
        
        StartCoroutine("mainLoop");
    }

    private IEnumerator mainLoop()
    {
        if(SceneChanger.getCurrentSceneName() == "MainScene")
        {
            yield return uIManager.showCountDown();

            while (!gameEndFlag)
            {
                yield return stage.fall();
                NumberOfTurns++;    //ターン数を増加
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            yield return uIManager.showPopUp();

            SceneChanger.changeTo(SceneType.Result);

        }
        // バトルモードなら
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
            //endGameが呼ばれゲーム終了となるまで
            while (!gameEndFlag)
            {
                yield return startBattle(); //バトル開始
                
            }
            
            yield return uIManager.showPopUp(popUpText);

            SceneChanger.changeTo(SceneType.Result);
            
        }

        yield break;
    }

    public void endGame()
    {
        Debug.Log("endGame");
        gameEndFlag = true;
        stage.GameOverFlag = true;  //ゲームオーバーのフラグを立てる
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
        //同時消しによるダメージ加算のみ
        if(mode == 0)
        {
            //同時消しが発生したなら
            //1連鎖当たりの同時消しの強化　＝　sumA×（1+(0.5×同時消し数）)
            if (sameEraseNum > 0)
            {
                playerAttack += damagePerChain * (1+(0.5f* sameEraseNum));
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
            //連鎖による強化 =（連鎖数ー１）* 1.5+sumB
            if (stage.ChainNum > 1)
            {

                playerAttack += (stage.ChainNum-1)*1.5f;
            }
        }
    }

    //バトルを開始する処理
    private IEnumerator initBattle(EnemyType fase = EnemyType.Dragon)
    {
        numberOfTurns = 0;  //ターン数を0にする
        playerAttack = 0;
        player.Init();

        switch (fase)
        {
            case EnemyType.Fase1:
                enemy.Init(5);
                break;
            case EnemyType.Fase2:
                enemy.Init(10);
                break;
            case EnemyType.Dragon:
                enemy.Init(15);
                break;
        }

        yield break;
    }

    private IEnumerator startBattle()
    {
        EnemyType enemyType = enemyArray[faseCount];
        yield return initBattle(enemyType);  //バトル開始

        yield return uIManager.showCountDown();

        /*
         * 一定間隔で攻撃するなら以下のコード
        //player.StartCoroutine("action");
        //enemy.StartCoroutine("action");
        */

        //yield return enemy.attack(player, EnemyAttackKinds.First);

        while (true)
        {
            yield return stage.fall();  //ブロックの落下処理

            NumberOfTurns++;    //ターン数を増加

            //文字の消え具合　また　ターンにより攻撃を行う場合
            yield return player.attack(enemy, PlayerAttackKinds.Word);
            yield return new WaitForSeconds(0.2f);
            yield return enemy.attack(player, EnemyAttackKinds.Normal);
            yield return player.attack(enemy, PlayerAttackKinds.Word);

            //死んだかの処理
            if (enemy.HpAmount <= 0f)
            {
                faseCount++;    //次のフェーズへ
                Debug.Log("fase:" + faseCount);
                yield return uIManager.showPopUp("You Win", 1.5f);  //勝利の余韻に浸る時間

                //すべての敵に勝ったなら
                if (enemyArray.Length <= faseCount)
                {
                    popUpText = "Clear";
                    endGame();
                }

                break;
            }
            if (player.HpAmount <= 0f)
            {
                endGame();
                popUpText = "You Lose";

            }

            if (gameEndFlag)
            {
                break;
            }
        }

        yield break;
    }
}

