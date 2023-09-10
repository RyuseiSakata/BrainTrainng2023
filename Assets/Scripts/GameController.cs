using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;


public enum EnemyType
{
    Fase1,
    Fase2,
    Dragon,
}


public class GameController : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] Stage stage;

    //�o�g���V�[���p
    [SerializeField] Battle.Player player;
    [SerializeField] Battle.Enemy enemy;

    private int numberOfTurns = 0;  //�^�[����

    private string popUpText = "Finish";

    private int score = 0;
    public static float playerAttack;   //�����������ɂ��v���C���[�̍U����

    //�^�[�����̃v���p�e�B
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    private int faseCount = 0;  //���݂̃t�F�[�Y��
    private EnemyType[] enemyArray = { EnemyType.Fase1, EnemyType.Fase2, EnemyType.Dragon };    //�o�g���̓G�̕ϐ������ԂɊi�[����z��

    private bool gameEndFlag = false;    //�Q�[���I���̃t���O
    private void Start()
    {
        score = 0;
        
        StartCoroutine("mainLoop");
    }

    private IEnumerator mainLoop()
    {
        if(SceneChanger.getCurrentSceneName() == "MainScene")
        {
            yield return uIManager.showCountDown();

            while (!gameEndFlag)
            {
                yield return stage.fall();
                NumberOfTurns++;    //�^�[�����𑝉�
                Debug.Log("turn:" + numberOfTurns);
                yield return new WaitForSeconds(0.3f);
            }

            yield return uIManager.showPopUp();

            SceneChanger.changeTo(SceneType.Result);

        }
        // �o�g�����[�h�Ȃ�
        else if(SceneChanger.getCurrentSceneName() == "BattleScene")
        {
            //endGame���Ă΂�Q�[���I���ƂȂ�܂�
            while (!gameEndFlag)
            {
                yield return startBattle(); //�o�g���J�n
                
            }
            
            yield return uIManager.showPopUp(popUpText);

            SceneChanger.changeTo(SceneType.Result);
            
        }

        yield break;
    }

    public void endGame()
    {
        Debug.Log("endGame");
        gameEndFlag = true;
        stage.GameOverFlag = true;  //�Q�[���I�[�o�[�̃t���O�𗧂Ă�
    }

    public void calculateScore(int scorePerChain, int sameEraseNum)
    {
        score += scorePerChain* sameEraseNum;
        if(stage.ChainNum > 1)
        {
            score += (stage.ChainNum - 1) * 200;
        }

        if (score > 999999)
        {
            uIManager.textUpdate(TextKinds.Score, 999999);
        }
        else
        {
            uIManager.textUpdate(TextKinds.Score, score);
        }
        
    }

    public void calculateDamage(int mode = 0, int damagePerChain = 0, int sameEraseNum = 0)
    {
        //���������ɂ��_���[�W���Z�̂�
        if(mode == 0)
        {
            //�������������������Ȃ�
            //1�A��������̓��������̋����@���@sumA�~�i1+(0.5�~�����������j)
            if (sameEraseNum > 0)
            {
                playerAttack += damagePerChain * (1+(0.5f* sameEraseNum));
            }
            else
            {
                playerAttack += damagePerChain;
            }
        }
        //�A���ɂ��_���[�W���Z��UI�X�V
        else
        {
            //�A�������������Ȃ�
            //�A���ɂ�鋭�� =�i�A�����[�P�j* 1.5+sumB
            if (stage.ChainNum > 1)
            {

                playerAttack += (stage.ChainNum-1)*1.5f;
            }
        }
    }

    //�o�g�����J�n���鏈��
    private IEnumerator initBattle(EnemyType fase = EnemyType.Dragon)
    {
        numberOfTurns = 0;  //�^�[������0�ɂ���
        playerAttack = 0;
        player.Init();

        switch (fase)
        {
            case EnemyType.Fase1:
                enemy.Init(5);
                break;
            case EnemyType.Fase2:
                enemy.Init(10);
                break;
            case EnemyType.Dragon:
                enemy.Init(15);
                break;
        }

        yield break;
    }

    private IEnumerator startBattle()
    {
        EnemyType enemyType = enemyArray[faseCount];
        yield return initBattle(enemyType);  //�o�g���J�n

        yield return uIManager.showCountDown();

        /*
         * ���Ԋu�ōU������Ȃ�ȉ��̃R�[�h
        //player.StartCoroutine("action");
        //enemy.StartCoroutine("action");
        */

        //yield return enemy.attack(player, EnemyAttackKinds.First);

        while (true)
        {
            yield return stage.fall();  //�u���b�N�̗�������

            NumberOfTurns++;    //�^�[�����𑝉�

            //�����̏�����@�܂��@�^�[���ɂ��U�����s���ꍇ
            yield return player.attack(enemy, PlayerAttackKinds.Word);
            yield return new WaitForSeconds(0.2f);
            yield return enemy.attack(player, EnemyAttackKinds.Normal);
            yield return player.attack(enemy, PlayerAttackKinds.Word);

            //���񂾂��̏���
            if (enemy.HpAmount <= 0f)
            {
                faseCount++;    //���̃t�F�[�Y��
                Debug.Log("fase:" + faseCount);
                yield return uIManager.showPopUp("You Win", 1.5f);  //�����̗]�C�ɐZ�鎞��

                //���ׂĂ̓G�ɏ������Ȃ�
                if (enemyArray.Length <= faseCount)
                {
                    popUpText = "Clear";
                    endGame();
                }

                break;
            }
            if (player.HpAmount <= 0f)
            {
                endGame();
                popUpText = "You Lose";

            }

            if (gameEndFlag)
            {
                break;
            }
        }

        yield break;
    }
}

