using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public enum EnemyAttackKinds
    {
        Normal,
        Obstacle,   //お邪魔ブロック
        First,  //最初の攻撃
        DeleteRow,
    }

    public class Enemy : MonoBehaviour
    {
        [SerializeField] BattleUIManager battleUIManager;
        [SerializeField] Stage stage;

        [SerializeField] float hpAmount;
        [SerializeField] float attackPower;
        [SerializeField] int attackChargeSpan = 2;
        public static float maxHp;

        private int attackChargedTurn; //次の攻撃までの残りターン数

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

        public int AttackChargedTurn {
            get => attackChargedTurn; 
            set
            {
                attackChargedTurn = value;
                battleUIManager.uiUpdate(Battle.UIKinds.AttackChargedTurn, attackChargedTurn);
            }
        }


        [SerializeField] Player player;

        private void Awake()
        {
            Init();
        }

        //ダメージ計算を行うメソッド
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("プレイヤーに倒された");
                stage.GameOverFlag = true;  //ゲームオーバーのフラグを立てる
                StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
            }
        }

        //Playerに攻撃を行うメソッド
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
        }

        //時間行動を管理するコルーチン
        private IEnumerator action()
        {
            while (player.HpAmount > 0f && HpAmount > 0f)
            {
                yield return new WaitForSeconds(5f);
                yield return attack(player, EnemyAttackKinds.Normal);
            }
            yield break;
        }

        //HPを設定できる
        public void Init(float init_hp = 10)
        {
            maxHp = init_hp;
            HpAmount = maxHp;    //Hpの初期化
            attackPower = 1;  //攻撃力の初期化
            attackChargeSpan = 2; //攻撃ターン間隔
            AttackChargedTurn = attackChargeSpan;
        }
    }
}
