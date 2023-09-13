using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle {
    /*public enum EnemyAttackKinds
    {
        Normal,
        Obstacle,   //お邪魔ブロック
        First,  //最初の攻撃
        DeleteRow,
    }*/

    public class Enemy : MonoBehaviour
    {
        [SerializeField]Text debugText;
        private EnemyType myType;
        [SerializeField] BattleUIManager battleUIManager;
        [SerializeField] Stage stage;

        [SerializeField] float hpAmount;
        public static float maxHp;

        private int actionCount = 0;
        private int nextActionCount = 0;//次の攻撃までの残りターン数

        public float HpAmount
        {
            get => hpAmount;
            set
            {
                hpAmount = value;
                if (hpAmount < 0) hpAmount = 0;

                battleUIManager.uiUpdate(Battle.UIKinds.EnemyHP, hpAmount);
            }
        }

        public int NextActionCount{
            get => nextActionCount; 
            set
            {
                nextActionCount = value;
                battleUIManager.uiUpdate(Battle.UIKinds.NextActionCount, nextActionCount);
            }
        }


        [SerializeField] Player player;

        private void Update()
        {
            debugText.text = "ACTION:" + actionCount.ToString("00");
        }

        //ダメージ計算を行うメソッド
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("プレイヤーに倒された");
                StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
            }
        }

        //自分の回復を行うメソッド
        private void heal(float healAmount)
        {
            Debug.Log($"ENEMY:{healAmount}回復した");
            HpAmount += healAmount;

            if (HpAmount > maxHp)
            {
                HpAmount = maxHp;
            }
        }

        //Playerに攻撃を行うメソッド
        /*
        public IEnumerator attack(Player target, EnemyAttackKinds attackKinds = EnemyAttackKinds.Normal)
        {

            //1つも単語を消せていないなら
            if(stage.ComboNum <= 0)
            {
                AttackChargedTurn -= 1;
            }
            else
            {
                AttackChargedTurn += stage.ComboNum / 3;
            }

            if (attackKinds == EnemyAttackKinds.First)
            {
                yield return stage.createObstacleBlock("すnたnーnと");

            }

            //行動できるなら
            if(AttackChargedTurn == 0)
            {
                switch (attackKinds)
                {
                    case EnemyAttackKinds.Normal:
                        float damageAmount = attackPower;
                        target.damage(damageAmount);
                        Debug.Log("プレイヤーにNormal Attack:" + damageAmount);
                        break;
                    case EnemyAttackKinds.Obstacle:
                        yield return stage.createObstacleBlock();
                        break;
                    case EnemyAttackKinds.DeleteRow:
                        yield return stage.rowLineDelete(6);
                        break;
                }
                AttackChargedTurn = attackChargeSpan; //攻撃までのターン数を更新
            }

            yield break;
        }*/

        //行動を行うコルーチン
        public IEnumerator action(Player target)
        {
            
            //1つも単語を消せていないなら
            if (stage.ComboNum <= 0)
            {
                NextActionCount -= 1;
                actionCount++;  //行動回数が増加

                switch (myType)
                {
                    case EnemyType.Fase1:
                        yield return fase1Action(target);
                        break;
                    case EnemyType.Fase2:
                        yield return fase2Action(target);
                        break;
                    case EnemyType.Dragon:
                        yield return dragonAction(target);
                        break;
                }
            }

            
            yield break;
        }
        //HPを設定できる
        public void Init(EnemyType enemyType)
        {
            myType = enemyType;
            switch (enemyType)
            {
                case EnemyType.Fase1:
                    maxHp = 7;
                    NextActionCount = 2;
                    break;
                case EnemyType.Fase2:
                    maxHp = 14;
                    NextActionCount = 2;
                    break;
                case EnemyType.Dragon:
                    maxHp = 30;
                    NextActionCount =1;
                    break;
            };
            
            
            HpAmount = maxHp;    //Hpの初期化
            actionCount = 0;    //行動ターンの初期化
        }

        //通常攻撃
        private IEnumerator normalAttack(Player target,float damageAmount)
        {
            target.damage(damageAmount);
            Debug.Log("ENEMY;通常攻撃（1）");
            yield return new WaitForSeconds(0.5f);
        }

        //適当な場所に指定個以下のランダムな文字のお邪魔ブロックを置く
        public IEnumerator randomObstacleAttack(int num)
        {
            if(num < 0)
            {
                num = 0;
            }

            string format = "nnnnnnn";

            List<int> colList = new List<int>();

            for(int i = 0; i < num; i++)
            {
                int randNum = Random.Range(0, Config.maxCol);
                if(!colList.Contains(randNum)) colList.Add(randNum);
            }

            colList.ForEach(col =>
            {
                int randNum = Random.Range(0, Config.character.Length);
                
                format = format.Remove(col, 1).Insert(col, Config.character[randNum].ToString());
            });
            
            yield return stage.createObstacleBlock(format);
            Debug.Log($"ENEMY:お邪魔ブロック（{format}）");
            yield break;
        }

        //フェーズ1の敵
        private IEnumerator fase1Action(Player target)
        {
            Debug.Log($"ENEMY:{actionCount}");
            switch (actionCount)
            {
                //通常攻撃(1)
                case 2:
                    yield return normalAttack(target, 1.5f);
                    NextActionCount = 3;
                    break;
                //通常攻撃(1)
                case 5:
                    yield return normalAttack(target, 1.5f);
                    NextActionCount = 4;
                    break;
                //通常攻撃(2)
                case 9:
                    yield return normalAttack(target, 3f);
                    NextActionCount = 2;
                    break;
                //お邪魔ブロック（ランダム1個）
                case 11:
                    yield return randomObstacleAttack(3);
                    NextActionCount = 2;
                    actionCount = 0;
                    break;
                default:
                    Debug.Log("ENEMY;休憩");
                    break;
            }
            yield break;
        }

        //フェーズ2の敵
        private IEnumerator fase2Action(Player target)
        {
            int randNum = Random.Range(0, 1);

            Debug.Log($"ENEMY:{actionCount}");
            switch (actionCount)
            {
                //通常攻撃（1）＋10行目削除
                case 2:
                    yield return normalAttack(target, 1f);
                    yield return stage.rowLineDelete(10);
                    Debug.Log("ENEMY:10行目を削除");
                    NextActionCount = 3;
                    break;
                //通常攻撃（1.3）+お邪魔ブロック（ランダム4個以下）
                case 5:
                    yield return normalAttack(target, 1.3f);
                    yield return randomObstacleAttack(4);
                    NextActionCount = 5;
                    break;
                //通常攻(2.5）またはplayerの回復5
                case 10:
                    if (randNum == 0) yield return normalAttack(target, 2.5f);
                    else heal(5);
                    NextActionCount = 2;
                    break;
                //お邪魔ブロック（ランダム7個以下）
                case 12:
                    yield return randomObstacleAttack(7);
                    NextActionCount = 2;
                    actionCount = 0;
                    break;
                default:
                    Debug.Log("ENEMY;休憩");
                    break;
            }
            yield break;
        }

        //ドラゴンの行動
        private IEnumerator dragonAction(Player target)
        {
            Debug.Log($"ENEMY:{actionCount}");
            switch (actionCount)
            {
                //通常攻撃(1)
                case 1:
                    yield return stage.colLineDelete(2);
                    yield return stage.colLineDelete(3);
                    yield return stage.colLineDelete(4);
                    NextActionCount = 2;
                    break;
                //通常攻撃(1)
                case 3:
                    yield return normalAttack(target, 0.5f);
                    NextActionCount = 2;
                    break;
                //通常攻撃(2)
                case 5:
                    yield return normalAttack(target, 1f);
                    yield return randomObstacleAttack(1);
                    NextActionCount = 3;
                    break;
                //お邪魔ブロック（ランダム1個）
                case 8:
                    yield return normalAttack(target, 0.8f);
                    yield return randomObstacleAttack(2);
                    NextActionCount = 3;
                    break;
                case 11:
                    yield return normalAttack(target, 1.2f);
                    yield return randomObstacleAttack(1);
                    NextActionCount = 3;
                    break;
                case 14:
                    yield return stage.rowLineDelete(Random.Range(5, 11));
                    NextActionCount = 3;
                    break;
                case 16:
                    actionCount = 0;
                    break;
                default:
                    Debug.Log("ENEMY;休憩");
                    break;
            }
            yield break;
        }

    }
}
