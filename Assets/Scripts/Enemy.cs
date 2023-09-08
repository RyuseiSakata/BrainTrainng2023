using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public enum EnemyAttackKinds
    {
        Normal,
        Obstacle,   //���ז��u���b�N
        First,  //�ŏ��̍U��
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

        private int attackChargedTurn; //���̍U���܂ł̎c��^�[����

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

        //�_���[�W�v�Z���s�����\�b�h
        public void damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (HpAmount <= 0f)
            {
                Debug.Log("�v���C���[�ɓ|���ꂽ");
                stage.GameOverFlag = true;  //�Q�[���I�[�o�[�̃t���O�𗧂Ă�
                StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
            }
        }

        //Player�ɍU�����s�����\�b�h
        public IEnumerator attack(Player target, EnemyAttackKinds attackKinds = EnemyAttackKinds.Normal)
        {

            //1���P��������Ă��Ȃ��Ȃ�
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
                yield return stage.createObstacleBlock("��n��n�[n��");

            }

            //�s���ł���Ȃ�
            if(AttackChargedTurn == 0)
            {
                switch (attackKinds)
                {
                    case EnemyAttackKinds.Normal:
                        float damageAmount = attackPower;
                        target.damage(damageAmount);
                        Debug.Log("�v���C���[��Normal Attack:" + damageAmount);
                        break;
                    case EnemyAttackKinds.Obstacle:
                        yield return stage.createObstacleBlock();
                        break;
                    case EnemyAttackKinds.DeleteRow:
                        yield return stage.rowLineDelete(6);
                        break;
                }
                AttackChargedTurn = attackChargeSpan; //�U���܂ł̃^�[�������X�V
            }

            yield break;
        }

        //���ԍs�����Ǘ�����R���[�`��
        private IEnumerator action()
        {
            while (player.HpAmount > 0f && HpAmount > 0f)
            {
                yield return new WaitForSeconds(5f);
                yield return attack(player, EnemyAttackKinds.Normal);
            }
            yield break;
        }

        //HP��ݒ�ł���
        public void Init(float init_hp = 10)
        {
            maxHp = init_hp;
            HpAmount = maxHp;    //Hp�̏�����
            attackPower = 1;  //�U���͂̏�����
            attackChargeSpan = 2; //�U���^�[���Ԋu
            AttackChargedTurn = attackChargeSpan;
        }
    }
}
