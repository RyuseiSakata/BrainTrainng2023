using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public enum PlayerAttackKinds
    {
        Normal,
        Combo,  //�R���{���ɂ��_���[�W
        Word,   //�P��폜�ɂ��_���\�W
    }

    public class Player : MonoBehaviour
    {
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
        }

        //�_���[�W�v�Z���s�����\�b�h
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("�G�ɓ|���ꂽ");
                StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
            }
        }

        //Enemy�ɍU�����s�����\�b�h
        public IEnumerator attack(Enemy target, PlayerAttackKinds attackKinds = PlayerAttackKinds.Normal)
        {
            float damageAmount;
            Debug.Log("dA:" + GameController.playerAttack);
            switch (attackKinds)
            {
                case PlayerAttackKinds.Normal:
                    damageAmount = attackPower;
                    target.damage(damageAmount);
                    Debug.Log("�G��Normal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Combo:
                    damageAmount = attackPower * stage.ComboNum;
                    target.damage(damageAmount);
                    Debug.Log("�G��Normal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Word:
                    damageAmount = attackPower * GameController.playerAttack;
                    target.damage(damageAmount);
                    Debug.Log("�P��ɂ��U���F"+damageAmount);
                    GameController.playerAttack = 0;
                    break;
            }

            yield break;
        }

        //���ԍs�����Ǘ�����R���[�`��
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
            HpAmount = maxHp;    //Hp�̏�����
            attackPower = 1;  //�U���͂̏�����
        }
    }
}

