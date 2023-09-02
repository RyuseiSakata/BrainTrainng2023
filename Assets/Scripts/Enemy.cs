using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public enum AttackKinds
    {
        Normal,
    }

    public class Enemy : MonoBehaviour
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
                battleUIManager.textUpdate(Battle.TextKinds.EnemyHP, hpAmount);
            }
        }

        [SerializeField] Player player;

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
                Debug.Log("プレイヤーに倒された");
                stage.GameOverFlag = true;  //ゲームオーバーのフラグを立てる
                StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
            }
        }

        //Playerに攻撃を行うメソッド
        private IEnumerator attack(Player target, AttackKinds attackKinds = AttackKinds.Normal)
        {
            switch (attackKinds)
            {
                case AttackKinds.Normal:
                    Debug.Log("プレイヤーにNormal Attack");
                    float damageAmount = attackPower;
                    target.damage(damageAmount);
                    break;
            }

            yield break;
        }

        //行動を管理するコルーチン
        private IEnumerator action()
        {
            while (player.HpAmount > 0f && HpAmount > 0f)
            {
                yield return new WaitForSeconds(5f);
                yield return attack(player, AttackKinds.Normal);
            }
            yield break;
        }
    }
}
