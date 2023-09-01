using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
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

        private void Start()
        {
            HpAmount = HpAmount;
            StartCoroutine("action");
        }

        //ダメージ計算を行うメソッド
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("敵に倒された");
                stage.gameOver();
                StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
            }
        }

        //Enemyに攻撃を行うメソッド
        private IEnumerator attack(Enemy target, AttackKinds attackKinds = AttackKinds.Normal)
        {
            switch (attackKinds)
            {
                case AttackKinds.Normal:
                    Debug.Log("敵にNormal Attack");
                    float damageAmount = attackPower;
                    target.damage(damageAmount);
                    break;
            }

            yield break;
        }

        //行動を管理するコルーチン
        private IEnumerator action()
        {
            while (enemy.HpAmount > 0f && HpAmount > 0f)
            {
                yield return new WaitForSeconds(3f);
                yield return attack(enemy, AttackKinds.Normal);
            }
            yield break;
        }

    }
}

