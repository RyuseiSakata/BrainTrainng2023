using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle {
    /*public enum EnemyAttackKinds
    {
        Normal,
        Obstacle,   //���ז��u���b�N
        First,  //�ŏ��̍U��
        DeleteRow,
    }*/

    public class Enemy : MonoBehaviour
    {
        [SerializeField] GameObject slimeObject;
        [SerializeField] GameObject minotaurosuObject;
        [SerializeField] GameObject dragonObject;
        
        private SlimeAnim slimeAnim;
        private MinotaurosuAnim minotaurosuAnim;
        private DragonAnim dragonAnim;

        [SerializeField]Text debugText;
        private EnemyType myType;
        [SerializeField] BattleUIManager battleUIManager;
        [SerializeField] Stage stage;

        [SerializeField] float hpAmount;
        public static float maxHp;

        private int actionCount = 0;
        private int nextActionCount = 0;//���̍U���܂ł̎c��^�[����

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

        public int NextActionCount{
            get => nextActionCount; 
            set
            {
                nextActionCount = value;
                battleUIManager.uiUpdate(Battle.UIKinds.NextActionCount, nextActionCount);
            }
        }


        [SerializeField] Player player;

        private void Start()
        {
            slimeAnim = slimeObject.GetComponent<SlimeAnim>();
            minotaurosuAnim = minotaurosuObject.GetComponent<MinotaurosuAnim>();
            dragonAnim = dragonObject.GetComponent<DragonAnim>();
        }

        private void Update()
        {
            debugText.text = "ACTION:" + actionCount.ToString("00");
        }

        //�_���[�W�v�Z���s�����\�b�h
        public IEnumerator damage(float damageAmount)
        {
            HpAmount -= damageAmount;

            if (damageAmount > 0)
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
                Debug.Log("�v���C���[�ɓ|���ꂽ");
                StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
            }


            yield break;
        }

        //�����̉񕜂��s�����\�b�h
        private void heal(float healAmount)
        {
            Debug.Log($"ENEMY:{healAmount}�񕜂���");
            HpAmount += healAmount;

            if (HpAmount > maxHp)
            {
                HpAmount = maxHp;
            }
        }

        //Player�ɍU�����s�����\�b�h
        /*
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
        }*/

        //�s�����s���R���[�`��
        public IEnumerator action(Player target)
        {
            
            //1���P��������Ă��Ȃ��Ȃ�
            if (stage.ComboNum <= 0)
            {
                NextActionCount -= 1;
                actionCount++;  //�s���񐔂�����

                switch (myType)
                {
                    case EnemyType.Slime:
                        yield return slimeAction(target);
                        break;
                    case EnemyType.Minotaurosu:
                        yield return minotaurosuAction(target);
                        break;
                    case EnemyType.Dragon:
                        yield return dragonAction(target);
                        break;
                }
            }

            
            yield break;
        }
        //HP��ݒ�ł���
        public void Init(EnemyType enemyType)
        {
            myType = enemyType;
            switch (enemyType)
            {
                case EnemyType.Slime:
                    battleUIManager.uiUpdate(UIKinds.EnemyName, "���炢��");
                    slimeObject.SetActive(true);
                    minotaurosuObject.SetActive(false);
                    dragonObject.SetActive(false);
                    maxHp = 7;
                    NextActionCount = 2;
                    break;
                case EnemyType.Minotaurosu:
                    battleUIManager.uiUpdate(UIKinds.EnemyName, "�݂̂����낷");
                    slimeObject.SetActive(false);
                    minotaurosuObject.SetActive(true);
                    dragonObject.SetActive(false);
                    maxHp = 14;
                    NextActionCount = 2;
                    break;
                case EnemyType.Dragon:
                    battleUIManager.uiUpdate(UIKinds.EnemyName, "�ǂ炲��");
                    slimeObject.SetActive(false);
                    minotaurosuObject.SetActive(false);
                    dragonObject.SetActive(true);
                    maxHp = 30;
                    NextActionCount =1;
                    break;
            };
            
            
            HpAmount = maxHp;    //Hp�̏�����
            actionCount = 0;    //�s���^�[���̏�����
        }

        //�ʏ�U��
        private IEnumerator normalAttack(Player target,float damageAmount)
        {
            yield return target.damage(damageAmount);
            Debug.Log("ENEMY;�ʏ�U���i1�j");
            yield return new WaitForSeconds(0.5f);
        }

        //�K���ȏꏊ�Ɏw��ȉ��̃����_���ȕ����̂��ז��u���b�N��u��
        public IEnumerator randomObstacleAttack(int num)
        {
            if(num < 0)
            {
                num = 0;
            }

            string format = "nnnnnnn";

            List<int> colList = new List<int>();

            for(int i = 0; i < num; i++)
            {
                int randNum = Random.Range(0, Config.maxCol);
                if(!colList.Contains(randNum)) colList.Add(randNum);
            }

            colList.ForEach(col =>
            {
                int randNum = Random.Range(0, Config.character.Length);
                
                format = format.Remove(col, 1).Insert(col, Config.character[randNum].ToString());
            });
            
            yield return stage.createObstacleBlock(format);
            Debug.Log($"ENEMY:���ז��u���b�N�i{format}�j");
            yield break;
        }

        //�t�F�[�Y1�̓G
        private IEnumerator slimeAction(Player target)
        {
            Debug.Log($"ENEMY:{actionCount}");
            switch (actionCount)
            {
                //�ʏ�U��(1)
                case 2:
                    slimeAnim.playAttackAnim();
                    yield return normalAttack(target, 1f);
                    NextActionCount = 3;
                    break;
                //�ʏ�U��(1)
                case 5:
                    slimeAnim.playAttackAnim();
                    yield return normalAttack(target, 1f);
                    NextActionCount = 4;
                    break;
                //�ʏ�U��(2)
                case 9:
                    slimeAnim.playAttackAnim();
                    yield return normalAttack(target, 1.5f);
                    NextActionCount = 2;
                    break;
                //���ז��u���b�N�i�����_��1�j
                case 11:
                    slimeAnim.playAttackAnim();
                    yield return randomObstacleAttack(3);
                    NextActionCount = 2;
                    actionCount = 0;
                    break;
                default:
                    Debug.Log("ENEMY;�x�e");
                    break;
            }
            yield break;
        }

        //�t�F�[�Y2�̓G
        private IEnumerator minotaurosuAction(Player target)
        {
            int randNum = Random.Range(0, 1);

            Debug.Log($"ENEMY:{actionCount}");
            switch (actionCount)
            {
                //�ʏ�U���i1�j�{10�s�ڍ폜
                case 2:
                    minotaurosuAnim.playAttackAnim();
                    yield return normalAttack(target, 3.5f);
                    yield return stage.rowLineDelete(10);
                    Debug.Log("ENEMY:10�s�ڂ��폜");
                    NextActionCount = 3;
                    break;
                //�ʏ�U���i1.3�j+���ז��u���b�N�i�����_��4�ȉ��j
                case 5:
                    minotaurosuAnim.playAttackAnim();
                    yield return normalAttack(target, 1.5f);
                    yield return randomObstacleAttack(4);
                    NextActionCount = 5;
                    break;
                //�ʏ�U(2.5�j�܂��͉�5
                case 10:
                    minotaurosuAnim.playAttackAnim();
                    if (randNum == 0) yield return normalAttack(target, 3f);
                    else heal(5);
                    NextActionCount = 2;
                    break;
                //���ז��u���b�N�i�����_��7�ȉ��j
                case 12:
                    minotaurosuAnim.playAttackAnim();
                    yield return randomObstacleAttack(7);
                    NextActionCount = 2;
                    actionCount = 0;
                    break;
                default:
                    Debug.Log("ENEMY;�x�e");
                    break;
            }
            yield break;
        }

        //�h���S���̍s��
        private IEnumerator dragonAction(Player target)
        {
            Debug.Log($"ENEMY:{actionCount}");
            switch (actionCount)
            {
                //�ʏ�U��(1)
                case 1:
                    dragonAnim.playAttackAnim();
                    int randNum = Random.Range(0, 3);
                    if (randNum == 0)
                    {
                        yield return stage.colLineDelete(2);
                        yield return stage.colLineDelete(4);
                    }
                    else if(randNum == 1)
                    {
                        yield return stage.colLineDelete(3);
                        yield return randomObstacleAttack(3);
                    }
                    else
                    {
                        yield return normalAttack(target, 2f);
                        yield return stage.colLineDelete(1);
                        yield return stage.colLineDelete(5);
                    }
                    
                    NextActionCount = 2;
                    break;
                //�ʏ�U��(1)
                case 3:
                    dragonAnim.playAttackAnim();
                    yield return normalAttack(target, 1.5f);
                    NextActionCount = 2;
                    break;
                //�ʏ�U��(2)
                case 5:
                    dragonAnim.playAttackAnim();
                    yield return normalAttack(target, 2f);
                    yield return randomObstacleAttack(1);
                    NextActionCount = 3;
                    break;
                //���ז��u���b�N�i�����_��1�j
                case 8:
                    dragonAnim.playAttackAnim();
                    yield return normalAttack(target, 1.5f);
                    yield return randomObstacleAttack(2);
                    NextActionCount = 3;
                    break;
                case 11:
                    dragonAnim.playAttackAnim();
                    yield return normalAttack(target, 1.25f);
                    yield return randomObstacleAttack(1);
                    NextActionCount = 3;
                    break;
                case 14:
                    dragonAnim.playAttackAnim();
                    float damageAmount = hpAmount >15f ? (30 - hpAmount) / 3f : 4f;
                    yield return normalAttack(target, damageAmount);
                    yield return stage.rowLineDelete(Random.Range(5, 11));
                    NextActionCount = 3;
                    break;
                case 16:
                    actionCount = 0;
                    break;
                default:
                    Debug.Log("ENEMY;�x�e");
                    break;
            }
            yield break;
        }

    }
}
