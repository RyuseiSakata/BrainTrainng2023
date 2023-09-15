using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public enum PlayerAttackKinds
    {
        Word,   //�P��폜�ɂ��_���\�W
    }

    public class Player : MonoBehaviour
    {
        private YushaAnim yushaAnim;
        [SerializeField] BattleUIManager battleUIManager;
        [SerializeField] Stage stage;
        [SerializeField] AudioManager audioManager;

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
        public IEnumerator attack(Enemy target)
        {
            float damageAmount;

            damageAmount = attackPower * GameController.playerAttack;
            if (damageAmount > 0)
            {
                yushaAnim.playAttackAnim();
                audioManager.playSeOneShot(AudioKinds.SE_PlayerAttack);
            }
            yield return target.damage(damageAmount);                    
            Debug.Log("�P��ɂ��U���F"+damageAmount);
            GameController.playerAttack = 0;

            yield break;
        }

        public void Init()
        {
            HpAmount = maxHp;    //Hp�̏�����
            attackPower = 1;  //�U���͂̏�����
        }
    }
}

