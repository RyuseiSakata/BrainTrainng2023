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

        //�_���[�W�v�Z���s�����\�b�h
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("�G�ɓ|���ꂽ");
                stage.gameOver();
                StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
            }
        }

        //Enemy�ɍU�����s�����\�b�h
        private IEnumerator attack(Enemy target, AttackKinds attackKinds = AttackKinds.Normal)
        {
            switch (attackKinds)
            {
                case AttackKinds.Normal:
                    Debug.Log("�G��Normal Attack");
                    float damageAmount = attackPower;
                    target.damage(damageAmount);
                    break;
            }

            yield break;
        }

        //�s�����Ǘ�����R���[�`��
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

