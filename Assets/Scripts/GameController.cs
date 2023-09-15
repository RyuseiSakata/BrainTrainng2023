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

    //バトルシーン用
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    private int numberOfTurns = 0;  //ターン数
                                    
    //ターン数のプロパティ
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    private int score = 0;  //スコア
    public static float playerAttack;   //消した文字によるプレイヤーの攻撃量

    public static float gameTime = 0; //ゲームのプレイ時間
    public static bool isSetTimer = false;  //時間を測定するかのフラグ

    private int faseCount = 0;  //現在のフェーズ数
    public int FaseCount
    {
        get => faseCount;
        set
        {
            faseCount = value;
            battleUIManager.uiUpdate(UIKinds.Fase,faseCount);
        }
    }

    private EnemyType[] enemyArray = { EnemyType.Slime, EnemyType.Minotaurosu, EnemyType.Dragon };    //バトルの敵の変数を順番に格納する配列

    private bool gameEndFlag = false;    //ゲーム終了のフラグ
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
        //Timerの測定を行う
        if (isSetTimer)
        {
            gameTime += Time.deltaTime;
        }
    }

    private IEnumerator mainLoop()
    {
        uIManager.configInit(); //設定の反映

        if (SceneChanger.getCurrentSceneName() == "MainScene")
        {
            yield return uIManager.showCountDown();
            audioManager.playBgm(AudioKinds.BGM_Main);

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
            //1連鎖当たりの同時消しの強化　＝　sumA×（1+(0.5×(同時消し数-1)）)
            if (sameEraseNum > 0)
            {
                playerAttack += damagePerChain * (1+(0.5f* (sameEraseNum-1)));
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
    private IEnumerator initBattle(EnemyType type = EnemyType.Dragon)
    {
        numberOfTurns = 0;  //ターン数を0にする
        playerAttack = 0;
        player.Init();

        enemy.Init(type);

        yield break;
    }

    private IEnumerator startBattle()
    {
        EnemyType enemyType = enemyArray[FaseCount];
        yield return initBattle(enemyType);  //バトル開始

        audioManager.pauseBgm();    //BGMを一時停止
        yield return uIManager.showCountDown();
        audioManager.playBgm(AudioKinds.BGM_Main);
        

        isSetTimer = true;   //時間の計測を開始
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

            if (gameEndFlag)
            {
                break;
            }

            //文字の消え具合　また　ターンにより攻撃を行う場合
            yield return player.attack(enemy);

            //死んだかの処理
            if (enemy.HpAmount <= 0f)
            {
                yield return uIManager.showPopUp("You Win", 1.5f);  //勝利の余韻に浸る時間
                FaseCount++;    //次のフェーズへ
                //すべての敵に勝ったなら
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

            //死んだかの処理
            if (enemy.HpAmount <= 0f)
            {
                
                yield return uIManager.showPopUp("You Win", 1.5f);  //勝利の余韻に浸る時間
                FaseCount++;    //次のフェーズへ
                //すべての敵に勝ったなら
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
        isSetTimer = false; //時間の計測を中断
        yield break;
    }

}

