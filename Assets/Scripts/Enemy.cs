using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public enum EnemyAttackKinds
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
                Debug.Log("�v���C���[�ɓ|���ꂽ");
                stage.GameOverFlag = true;  //�Q�[���I�[�o�[�̃t���O�𗧂Ă�
                StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
            }
        }

        //Player�ɍU�����s�����\�b�h
        public IEnumerator attack(Player target, EnemyAttackKinds attackKinds = EnemyAttackKinds.Normal)
        {
            switch (attackKinds)
            {
                case EnemyAttackKinds.Normal:
                    float damageAmount = attackPower;
                    target.damage(damageAmount);
                    Debug.Log("�v���C���[��Normal Attack:"+damageAmount);
                    break;
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
    }
}
