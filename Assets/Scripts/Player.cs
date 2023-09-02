using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public enum PlayerAttackKinds
    {
        Normal,
        Combo,  //コンボ数によるダメージ
    }

    public class Player : MonoBehaviour
    {
        [SerializeField] BattleUIManager battleUIManager;
        [SerializeField] Stage stage;

        [SerializeField] float hpAmount;
        [SerializeField] float attackPower;

        public float HpAmount
        {
            get => hpAmount;
            set
            {
                hpAmount = value;
                battleUIManager.textUpdate(Battle.TextKinds.PlayerHP, hpAmount);
            }
        }

        [SerializeField] Enemy enemy;

        private void Awake()
        {
            HpAmount = HpAmount;
        }

        //ダメージ計算を行うメソッド
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("敵に倒された");
                stage.GameOverFlag = true;  //ゲームオーバーのフラグを立てる
                StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
            }
        }

        //Enemyに攻撃を行うメソッド
        public IEnumerator attack(Enemy target, PlayerAttackKinds attackKinds = PlayerAttackKinds.Normal)
        {
            float damageAmount;

            switch (attackKinds)
            {
                case PlayerAttackKinds.Normal:
                    damageAmount = attackPower;
                    target.damage(damageAmount);
                    Debug.Log("敵にNormal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Combo:
                    damageAmount = attackPower * stage.ComboNum;
                    target.damage(damageAmount);
                    Debug.Log("敵にNormal Attack:" + damageAmount);
                    break;
            }

            yield break;
        }

        //時間行動を管理するコルーチン
        private IEnumerator action()
        {
            while (enemy.HpAmount > 0f && HpAmount > 0f)
            {
                yield return new WaitForSeconds(3f);
                yield return attack(enemy, PlayerAttackKinds.Normal);
            }
            yield break;
        }

    }
}

