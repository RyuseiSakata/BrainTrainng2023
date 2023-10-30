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

    public static int score = 0;  //スコア
    public static float playerAttack;   //消した文字によるプレイヤーの攻撃量

    public static float gameTime = 0; //ゲームのプレイ時間
    public static bool isSetTimer = false;  //時間を測定するかのフラグ

    public static int faseCount = 0;  //現在のフェーズ数
    private bool isNormal = false;

    //ランク（階級）
    public static string rank; //現在のランク
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

    private EnemyType[] enemyArray = { EnemyType.Slime, EnemyType.Minotaurosu, EnemyType.Dragon };    //バトルの敵の変数を順番に格納する配列

    private bool gameEndFlag = false;    //ゲーム終了のフラグ
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
        //Timerの測定を行う
        if (isSetTimer)
        {
            gameTime += Time.deltaTime;
        }

    }

    private IEnumerator mainLoop()
    {
        

        
        if (SceneChanger.getCurrentSceneName() == "MainScene")
        {
            uIManager.configInit(); //設定の反映
            isNormal = true;
            yield return uIManager.showCountDown();
            audioManager.playBgm(AudioKinds.BGM_Main);

            while (!gameEndFlag)
            {
                yield return stage.fall();
                NumberOfTurns++;    //ターン数を増加
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            audioManager.playSeOneShot(AudioKinds.SE_Whistle);
            yield return uIManager.showPopUp();

            SceneChanger.changeTo(SceneType.NormalResult);

        }
        // バトルモードなら
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
            uIManager.configInit(); //設定の反映
            isNormal = false;
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

            bool isFilled = false;  //ブロックが満たされたか否か

            do
            {
                isNormal = false;
                gameEndFlag = false; 

                //endGameが呼ばれゲーム終了となるまで
                while (!gameEndFlag)
                {
                    yield return startTutorial(); //バトル開始
                }

                audioManager.stopBgm();

                switch (endState)
                {
                    case EndState.WIN:
                        audioManager.playSeOneShot(AudioKinds.SE_WIN);
                        yield return uIManager.showResultPanel(EndState.WIN, 3.5f);
                        isFilled = false;
                        break;
                    case EndState.LOSE: //ここは通らないはず
                        audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                        yield return uIManager.showResultPanel(EndState.LOSE, 3.5f);
                        isFilled = false;
                        break;
                    case EndState.FILLED:
                        audioManager.playSeOneShot(AudioKinds.SE_LOSE);
                        yield return uIManager.showResultPanel(EndState.LOSE, 3.5f);
                        yield return discriptionManager.playDiscription(5); //もう一度チュートリアルを実行
                        isFilled = true;
                        stage.restart();    //再開
                        Destroy(discriptionManagerObject);
                        break;
                }

            } while (isFilled);

            yield return discriptionManager.playDiscription(4); //モードの解説

            SceneChanger.changeTo(SceneType.Title);
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

        if (score > 9999999)
        {
            uIManager.textUpdate(TextKinds.Score, 9999999);    
        }
        else
        {
            uIManager.textUpdate(TextKinds.Score, score);
        }

        //ノーマルモードなら
        if (isNormal)
        {
            string rank = getRank();
            uIManager.textUpdate(TextKinds.Rank, rank);  //スコアからランクを求める
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

    private IEnumerator startTutorial()
    {
        //説明オブジェクトの生成
        discriptionManagerObject = Instantiate(discriptionManagerPrefab, canvas);
        discriptionManager = discriptionManagerObject.GetComponent<DiscriptionManager>();

        yield return initBattle(EnemyType.Tutorial);  //バトル開始
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

                case 2: //1ループ待機
                    yield return stage.fallTutorial();  //ブロックの落下処理
                    _tutorialPart = 3;
                    break;

                case 3: //単語を作ろうを表示
                    yield return discriptionManager.playDiscription(2);
                    _tutorialPart = 4;
                    continue;

                case 4: //単語が消えるまでプレイ
                    yield return stage.fallTutorial();  //ブロックの落下処理
                    if (playerAttack > 0) _tutorialPart = 5;//攻撃がある == ブロックが消えた
                    break;

                case 5: //消えたね、クリアしようの表示
                    yield return discriptionManager.playDiscription(3);
                    _tutorialPart = 99;
                    continue;

                default:
                    yield return stage.fallTutorial();  //ブロックの落下処理
                    if (playerAttack > 0) _tutorialPart = 4;//攻撃がある == ブロックが消えた
                    break;

            }

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
                //yield return uIManager.showPopUp("You Win", 1.5f);  //勝利の余韻に浸る時間
                endGame(EndState.WIN);
                break;
            }

            yield return new WaitForSeconds(0.2f);
            yield return enemy.action(player);
            yield return player.attack(enemy);

            //死んだかの処理
            if (enemy.HpAmount <= 0f)
            {
                //yield return uIManager.showPopUp("You Win", 1.5f);  //勝利の余韻に浸る時間
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

