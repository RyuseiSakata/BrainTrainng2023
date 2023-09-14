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
        Word,   //単語削除によるダメ―ジ
    }

    public class Player : MonoBehaviour
    {
        private YushaAnim yushaAnim;
        [SerializeField] BattleUIManager battleUIManager;
        [SerializeField] Stage stage;

        [SerializeField] float hpAmount;
        [SerializeField] float attackPower;

        public static float maxHp = 25;

        public float HpAmount
        {
            get => hpAmount;
            set
            {
                hpAmount = value;
                if (hpAmount < 0) hpAmount = 0;
                battleUIManager.uiUpdate(Battle.UIKinds.PlayerHP, hpAmount);
            }
        }

        [SerializeField] Enemy enemy;

        private void Awake()
        {
            HpAmount = HpAmount;
            yushaAnim = transform.GetChild(0).GetComponent<YushaAnim>();
        }

        //ダメージ計算を行うメソッド
        public IEnumerator damage(float damageAmount)
        {
            HpAmount -= damageAmount;
            
            if(damageAmount > 0)
            {
                //点滅
                for (int i = 0; i < 3; i++)
                {
                    gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.1f);
                    gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (HpAmount <= 0f)
            {
                Debug.Log("敵に倒された");
                StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
            }

            yield break;
        }

        //Enemyに攻撃を行うメソッド
        public IEnumerator attack(Enemy target, PlayerAttackKinds attackKinds = PlayerAttackKinds.Normal)
        {
            float damageAmount;
            Debug.Log("dA:" + GameController.playerAttack);
            switch (attackKinds)
            {
                case PlayerAttackKinds.Normal:
                    yushaAnim.playAttackAnim();
                    damageAmount = attackPower;
                    yield return target.damage(damageAmount);
                    Debug.Log("敵にNormal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Combo:
                    yushaAnim.playAttackAnim();
                    damageAmount = attackPower * stage.ComboNum;
                    yield return target.damage(damageAmount);
                    Debug.Log("敵にNormal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Word:
                    damageAmount = attackPower * GameController.playerAttack;
                    if (damageAmount > 0)
                    {
                        yushaAnim.playAttackAnim();
                    }
                    yield return target.damage(damageAmount);                    
                    Debug.Log("単語による攻撃："+damageAmount);
                    GameController.playerAttack = 0;
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

        public void Init()
        {
            HpAmount = maxHp;    //Hpの初期化
            attackPower = 1;  //攻撃力の初期化
        }
    }
}

