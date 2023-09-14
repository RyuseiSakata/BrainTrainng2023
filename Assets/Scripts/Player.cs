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

        //�_���[�W�v�Z���s�����\�b�h
        public IEnumerator damage(float damageAmount)
        {
            HpAmount -= damageAmount;
            
            if(damageAmount > 0)
            {
                //�_��
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
                Debug.Log("�G�ɓ|���ꂽ");
                StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
            }

            yield break;
        }

        //Enemy�ɍU�����s�����\�b�h
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
                    Debug.Log("�G��Normal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Combo:
                    yushaAnim.playAttackAnim();
                    damageAmount = attackPower * stage.ComboNum;
                    yield return target.damage(damageAmount);
                    Debug.Log("�G��Normal Attack:" + damageAmount);
                    break;
                case PlayerAttackKinds.Word:
                    damageAmount = attackPower * GameController.playerAttack;
                    if (damageAmount > 0)
                    {
                        yushaAnim.playAttackAnim();
                    }
                    yield return target.damage(damageAmount);                    
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

