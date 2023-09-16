using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridState
{
    Active,
    Disactive,
    OutStage,
    Null,
}

public class Stage : MonoBehaviour
{
    public float fallBoost = 1; 

    [SerializeField] GameObject blockPrefab;
    [SerializeField] UIManager uIManager;
    [SerializeField] GameController gameController;
    [SerializeField] PlayerInput playerInput;

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow + 1, Config.maxCol];  //�X�e�[�W�S�̂̃u���b�N�z��
    public List<Block> activeBlockList = new List<Block>(); //��������u���b�N�̃��X�g
    private List<Block> judgeTargetList = new List<Block>(); //������s���Ώ�
    List<Block> destroyList = new List<Block>();    //�폜����u���b�N�̃��X�g

    private List<Block>[] nextBlock = { new List<Block>(), new List<Block>() }; //��(0)�Ƃ��̎�(1)�̃u���b�N���i�[
    [SerializeField] Transform[] SpawnPos;  //���̏o���ʒu(0)�Ƃ��̎��̏o���ʒu(1)

    private bool isChained;  //�A������������\���t���O�i�����ł����A���͏��������x�����������s���A�ēx�����鏈�����s��ꂽ�񐔂ł���j
    private int comboNum = 0;   //�R���{��
    private int chainNum = 0;   //�A����
    public static int maxComboNum = 0;    //�ő�R���{��

    public bool CanUserOperate { get; set; } = false;   //���[�U������ł��邩�ۂ��̃t���O

    [SerializeField] AudioManager audioManager;

    [SerializeField] WordList wordList;  //�������P�ꃊ�X�g

    private int sameEraseNum = 0;   //�����������i1�A�����j
    private int scorePerChain = 0;    //�P�A�����̃X�R�A�̍��v�@��j��񂲁A�����@-> 100+100
    public int damagePerChain = 0;    //�U���_���[�W��

    public bool GameOverFlag { get; set; } = false; //�Q�[���I�[�o�[����p�̃t���O�@true�ɂȂ��fall�R���[�`�����I��

    //�R���{���̃v���p�e�B
    public int ComboNum
    {
        get => comboNum;
        set
        {
            comboNum = value;
            uIManager.textUpdate(TextKinds.Combo, comboNum);  //�R���{����UI�X�V
        }
    }//�ő�R���{���̃v���p�e�B
    public int MaxComboNum
    {
        get => maxComboNum;
        set
        {
            maxComboNum = value;
            uIManager.textUpdate(TextKinds.MaxCombo, maxComboNum);  //�R���{����UI�X�V
        }
    }

    //�A�����̃v���p�e�B
    public int ChainNum
    {
        get => chainNum;
        set
        {
            chainNum = value;
        }
    }

    private void Awake()
    {
        //�N�����x���d�ݍ��v���v�Z���Ă��Ȃ��Ȃ�
        if (!Config.isCaluculatedSum)
        {
            //�d�݂̍��v�l���i�[
            foreach (var value in Config.probability)
            {
                Config.sumProbability += value;
            }
            Config.isCaluculatedSum = true;
        }

        wordList.CollectList.Clear();   //�������P�ꃊ�X�g��S�폜
        maxComboNum = 0;
        firstSetBlock(); //�u���b�N�̏����z�u


        /*
            StartCoroutine("fall");
        */
    }

    private void Update()
    {
        //BlockArray�̒��g��\���@�i�f�o�b�O�p�j
        if (Input.GetKeyDown(KeyCode.I))
        {

            string s = "\n";

            for (int i = 0; i < BlockArray.GetLength(0); i++)
            {
                for (int j = 0; j < Config.maxCol; j++)
                {
                    if (BlockArray[i, j] == null)
                    {
                        s += "  ,";
                    }
                    else
                    {
                        s += BlockArray[i, j].chara + ',';
                    }
                }
                s += '\n';
            }
            Debug.Log(s);
        }

    }

    //�w�肵��(row,col)�̏�Ԃ��l������
    public GridState checkState(int row, int col)
    {
        //�����̐��m���̃`�F�b�N
        if (!(0 <= row && row < BlockArray.GetLength(0) && 0 <= col && col < Config.maxCol))
        {
            return GridState.OutStage;
        }

        //�ő�s������������,���̗v�f����Ȃ�
        if (BlockArray[row, col] == null)
        {
            return GridState.Null;
        }

        //�u���b�N�̏�Ԃ��A�N�e�B�u����A�N�e�B�u��
        if (BlockArray[row, col].BlockState)
        {
            return GridState.Active;
        }
        else
        {
            return GridState.Disactive;
        }
    }

    //Start�ŌĂяo��,nextBlock�Ƀu���b�N���Z�b�g����i���Ƃ��̎��̃u���b�N��ݒ�j
    private void firstSetBlock()
    {
        for (int i = 0; i < 2; i++)
        {
            int randNum = Random.Range(0, 2);
            if (randNum == 0)
            {
                var instance = Instantiate(blockPrefab, SpawnPos[i].transform.position, Quaternion.identity, SpawnPos[i].transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);
                Block block = instance.GetComponent<Block>();
                block.stage = this;
                block.init(decideCharacter(), 0, 2);
                nextBlock[i].Add(block);

            }
            else
            {
                var instance = Instantiate(blockPrefab, SpawnPos[i].transform.position, Quaternion.identity, SpawnPos[i].transform);
                instance.transform.localPosition = new Vector3(-(1f / Config.maxCol / 2f), 0f, 0f);
                instance.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);
                Block block = instance.GetComponent<Block>();
                block.stage = this;
                block.init(decideCharacter(), 0, 2);
                nextBlock[i].Add(block);

                var instance2 = Instantiate(blockPrefab, SpawnPos[i].transform.position, Quaternion.identity, SpawnPos[i].transform);
                instance2.transform.localPosition = new Vector3(1f / Config.maxCol / 2f, 0f, 0f);
                instance2.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);
                Block block2 = instance2.GetComponent<Block>();
                block2.stage = this;
                block2.init(decideCharacter(), 0, 3);
                nextBlock[i].Add(block2);
            }
        }
    }


    //�u���b�N�𐶐����A��ʏ� <- �� <- ���̎��@�̂悤�ɂ��炷����
    private void spawnBlock()
    {
        //nextBlock[0]��Active�ɂ���
        foreach (var block in nextBlock[0])
        {
            block.transform.SetParent(this.gameObject.transform);
            block.callActive();
            activeBlockList.Add(block);
            judgeTargetList.Add(block);
        }

        //nextBlock[1]��nextBlock[0]�Ɉڂ�
        nextBlock[0].Clear();
        foreach (var block in nextBlock[1])
        {
            nextBlock[0].Add(block);
            block.transform.SetParent(SpawnPos[0].transform);
            block.transform.localPosition += SpawnPos[0].localPosition - SpawnPos[1].localPosition;
        }

        //nextBlock[1]�ɐV�����u���b�N�𐶐�
        nextBlock[1].Clear();
        int randNum = Random.Range(0, 2);
        if (randNum == 0)
        {
            var instance = Instantiate(blockPrefab, SpawnPos[1].transform.position, Quaternion.identity, SpawnPos[1].transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);
            Block block = instance.GetComponent<Block>();
            block.stage = this;
            block.init(decideCharacter(), 0, 2);
            nextBlock[1].Add(block);
        }
        else
        {
            var instance = Instantiate(blockPrefab, SpawnPos[1].transform.position, Quaternion.identity, SpawnPos[1].transform);
            instance.transform.localPosition = new Vector3(-(1f / Config.maxCol / 2f), 0f, 0f);
            instance.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);
            Block block = instance.GetComponent<Block>();
            block.stage = this;
            block.init(decideCharacter(), 0, 2);
            nextBlock[1].Add(block);

            var instance2 = Instantiate(blockPrefab, SpawnPos[1].transform.position, Quaternion.identity, SpawnPos[1].transform);
            instance2.transform.localPosition = new Vector3(1f / Config.maxCol / 2f, 0f, 0f);
            instance2.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);
            Block block2 = instance2.GetComponent<Block>();
            block2.stage = this;
            block2.init(decideCharacter(), 0, 3);
            nextBlock[1].Add(block2);
        }
    }

    //���������肷��
    private string decideCharacter()
    {
        float randomNum = Random.Range(0f, Config.sumProbability);

        int ret;    //�ԋp���镶���̗v�f�ԍ�
        int sum = 0;    //���[�v���܂ł̍��v�l
        for (ret = 0; ret < Config.probability.Length; ret++)
        {
            sum += Config.probability[ret];
            if (randomNum < sum)
            {
                break;
            }
        }


        //Debug.Log("LOG:" + Config.character[ret].ToString() + "�̏o���m��:" + (100f * Config.probability[ret] / Config.sumProbability).ToString("") + "%");
        return Config.character[ret].ToString();
    }

    //�u���b�N�̗����������s���R���[�`�� spawnFlag��false�ɂ���ƃX�|�[���������s��Ȃ�
    public IEnumerator fall(bool spawnFlag = true)
    {
        CanUserOperate = true;  //���[�U�̑�����\��
        ComboNum = 0;
        ChainNum = 0;

        if (spawnFlag)
        {
            spawnBlock();   //�u���b�N�̐���
        }
        else
        {
            CanUserOperate = false;  //���[�U�̑����s�\��
        }

        decideDestination();    //�ڕW�s���̌���



        //�����鏈��(���ׂė��������遁���ׂĂ�BlockState=false�ɂȂ�܂�)
        while (activeBlockList.Count != activeBlockList.FindAll(x => x.BlockState == false).Count)
        {
            activeBlockList.ForEach(b =>
            {
                if (b.BlockState)
                {
                    b.MoveDown();
                }
            });

            yield return new WaitForEndOfFrame();
        }

        audioManager.playSeOneShot(AudioKinds.SE_BlockMove);


        yield return fallBottom();   //�����т̃p�^�[���ɂ����Ē��n�܂ŉ�������


        //Debug.Log("���n");

        isChained = true;   //�A���̃t���O�@true�̌��葱���Ă���
        while (isChained)
        {
            yield return judgeAndDelete();
            
            gameController.calculateScore(scorePerChain, sameEraseNum); //�X�R�A�v�Z
            gameController.calculateDamage(0,damagePerChain, sameEraseNum);   //�v���C���[�̍U���ʂ��v�Z
            sameEraseNum = 0;   //�����������̃��Z�b�g
            scorePerChain = 0;  //��A��������̃X�R�A�����Z�b�g
            damagePerChain = 0;  //��A��������̃v���C���[�̍U���ʂ����Z�b�g
            ChainNum += 1;
        }
        ChainNum -= 1;//�A�����̒���

        //�ő�R���{���̍X�V
        if(ComboNum > MaxComboNum)
        {
            MaxComboNum = ComboNum;
        }

        yield return fallBottom();   //��̏ꍇ�ɉ��܂ŉ�������


        // 0�s�ڂɃu���b�N������Ȃ�Q�[���I�[�o�[�ɂ���
        for (int i = 0; i < BlockArray.GetLength(1); i++)
        {
            if (BlockArray[0, i] != null)
            {
                GameOverFlag = true;
                break;
            }
        }

        //�Q�[���I�[�o�[�̔���
        if (GameOverFlag)
        {
            gameController.endGame(EndState.FILLED);
            yield break;
        }

        gameController.calculateDamage(1, damagePerChain, sameEraseNum);   //�v���C���[�̍U���ʂ��v�Z
        CanUserOperate = false;  //���[�U�̑����s�\��
        playerInput.updateTapPosition();

        
        //destinationRow��currentRow���قȂ�Ƃ��ɏC��,������~�o�O��h��(Debug�p���[�v)
        for (int i = 0; i < BlockArray.GetLength(0); i++)
        {
            for (int j = 0; j < BlockArray.GetLength(1); j++)
            {/*
                if (BlockArray[i, j] != null)
                {
                    BlockArray[i, j].DestinationRow = BlockArray[i, j].CurrentRow;
                    Debug.Log($"destination�̏C��({i},{j},{BlockArray[i, j].chara}->{BlockArray[i, j].DestinationRow},{BlockArray[i, j].CurrentRow}");
                }*/
                if (BlockArray[i, j] != null && BlockArray[i, j].DestinationRow != BlockArray[i, j].CurrentRow)
                {
                    Debug.Log("!!�Ⴄ");
                }

            }
        }
        yield break;
    }

    private IEnumerator fallBottom()
    {

        List<Block> targetList = new List<Block>();
        foreach (var block in activeBlockList)
        {
            if (checkState(block.CurrentRow + 1, block.CurrentCol) == GridState.Null) targetList.Add(block);
        }

        do
        {

            List<Block> deleteList = new List<Block>();
            targetList.ForEach(e =>
            {
                if (checkState(e.CurrentRow + 1, e.CurrentCol) != GridState.Null) deleteList.Add(e);
            });
            deleteList.ForEach(e =>
            {
                targetList.Remove(e);
            });


            activeBlockList.Clear();    //activeBlockList�̗v�f��S�폜

            if (targetList.Count > 0)
            {
                targetList.ForEach(t =>
                {
                    BlockArray[t.CurrentRow, t.CurrentCol] = null;    //���݈ʒu�̍폜
                    t.BlockState = true;   //Active��Ԃ�
                    activeBlockList.Add(t);

                    decideDestination();

                    //�����鏈��(���ׂė��������遁���ׂĂ�BlockState=false�ɂȂ�܂�)
                    while (activeBlockList.Count != activeBlockList.FindAll(x => x.BlockState == false).Count)
                    {
                        activeBlockList.ForEach(b =>
                        {
                            if (b.BlockState)
                            {
                                b.MoveDown();
                            }
                        });
                    }
                    audioManager.playSeOneShot(AudioKinds.SE_BlockMove);
                });
            }


            //destinationRow��currentRow���قȂ�Ƃ��ɏC��,������~�o�O��h��
            activeBlockList.ForEach(block =>
            {
                var fallBlock = BlockArray[block.CurrentRow, block.CurrentCol];
                if (fallBlock != null)
                {
                    fallBlock.DestinationRow = fallBlock.CurrentRow;
                    //Debug.Log($"�ύX�F{fallBlock.CurrentRow},{fallBlock.CurrentCol},{fallBlock.chara}");
                }
            });

            activeBlockList.Clear();    //activeBlockList�̗v�f��S�폜

        } while (targetList.Count > 0);


        yield break;
    }

    //activeBlockList�̃u���b�N�Q�����s�ڂ܂ōs���������߂�
    private void decideDestination()
    {
        //�����u���b�N�̌���2��,������̎��i�c���уu���b�N(2��)�j
        if (activeBlockList.Count == 2 && (activeBlockList[0].CurrentCol == activeBlockList[1].CurrentCol))
        {
            Block upper, lower;
            int col = activeBlockList[0].CurrentCol;    //���ʂ̗�ԍ�
            int row;    //�����̍s�ԍ�

            if (activeBlockList[0].CurrentRow <= activeBlockList[1].CurrentRow)
            {
                upper = activeBlockList[0];
                lower = activeBlockList[1];
            }
            else
            {
                upper = activeBlockList[1];
                lower = activeBlockList[0];
            }

            row = lower.CurrentRow; //�����̍s�ԍ����i�[

            //�ڕW�̗�ԍ��̌���
            for (int r = row + 1; r <= BlockArray.GetLength(0); r++)
            {
                if (checkState(r, col) != GridState.Null)
                {
                    if (r == 0) Debug.Log("-1��������");

                    lower.DestinationRow = r - 1;
                    break;
                };
            }
            upper.DestinationRow = lower.DestinationRow - 1;
        }
        else if(activeBlockList.Count > 0)
        {
            int row = -100; //�ړI�̍s��(�����l-100)

            int min_row = activeBlockList.Min(x => x.CurrentRow);

            //�ڕW�̗�ԍ��̌���
            for (int r = min_row + 1; r <= BlockArray.GetLength(0); r++)
            {
                foreach (var block in activeBlockList)
                {
                    if (checkState(r, block.CurrentCol) != GridState.Null)
                    {
                        if (r == 0) Debug.Log("-1��������");

                        row = r - 1;

                        break;
                    }
                }

                if (row != -100) break; //�ڕW�̍s������܂���
            }

            if (row == -100) Debug.Log("�ڕW����܂�Ȃ���");
            else
            {
                foreach (var block in activeBlockList)
                {
                    block.DestinationRow = row;
                }
            }
        }
    }

    //����E�����E�����������s���@�ԋp�l��true�Ȃ�A���I��
    private IEnumerator judgeAndDelete()
    { 
        /***  �c�����̕�����̔���  ���ׂ��̕�������擾  ***/
        List<int> targetRow = new List<int>();  //���ׂ��s���i�[
        List<int> targetCol = new List<int>();  //���ׂ�����i�[
        foreach (var block in judgeTargetList)
        {
            if (!targetCol.Contains(block.CurrentCol))  //�܂����ׂĂ��Ȃ���Ȃ�Ύ��s
            {
                int head = 0, end = 0;  //�c����������̐擪�s�ԍ��ƏI�[�s�ԍ�
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);

                string str = getStringFromRow(targetRow.Last(), targetCol.Last(), ref head, ref end);   //�Ώۂ̗�̕�������擾���擪���[�̔��f

                //3�����ȏ�
                if (str.Length >= 3)
                {
                    //List<string> findList = new List<string>() { "���", "�����", "����", "����", "���育��" };
                    Jage jage = new Jage();
                    IEnumerable<string> findList = jage.Check(str);     //�擾����������istr�j�Ɋ܂܂��P�����������擾��findList�ɑ��
                    List<string> formalList = jage.Get();     //�擾����������istr�j�Ɋ܂܂��P��̐����\������������擾��formalList�ɑ��

                    Debug.Log("List:�c������F" + str);
                    var s = "List:�܂ߒP��F";
                    findList.ToList().ForEach(e =>
                    {
                        s += e + ",";

                    });
                    Debug.Log(s);

                    foreach (var (word, wIndex) in findList.Select((value, wIndex) => (value, wIndex)))
                    {
                        int index = -1; //str����word�̏o���ʒu

                        //str���Ɋ܂܂��word�����ׂĒT��
                        while (true)
                        {
                            index = str.IndexOf(word, index + 1);
                            //str����word���܂܂�Ȃ��Ƃ�
                            if (index == -1)
                            {
                                break;
                            }
                            else
                            {
                                //�R���N�V�������X�g�ɒǉ�
                                WordData addWord = new WordData(word, formalList[wIndex],"");
                                wordList.CollectList.Add(addWord);  //���������t���X�g�ɒǉ�
                                uIManager.addWordView();

                                //�{�̕\��
                                yield return uIManager.updateBook(word, formalList[wIndex]);

                                for (int i = index + head; i < index + head + word.Length; i++)
                                {
                                    /*
                                     * �Â��A�j���[�V����
                                    Block b = BlockArray[i, targetCol.Last()];
                                    b.lightUp();
                                    if (!destroyList.Contains(b)) destroyList.Add(b);
                                    yield return new WaitForSeconds(0.3f);
                                    */
                                    Block b = BlockArray[i, targetCol.Last()];
                                    b.emphasize();  //�A�j���[�V�����Đ�
                                    if (!destroyList.Contains(b)) destroyList.Add(b);
                                }
                                float pitch = 0.6f + comboNum * 0.4f;
                                audioManager.playSeOneShot(AudioKinds.SE_FindWord, pitch);
                                if (word.Length > 2) scorePerChain += 100 * (int)Mathf.Pow(2, word.Length - 3); //1�P�ꓖ����̃X�R�A 100*2^(������-3)
                                if (word.Length > 2) damagePerChain += word.Length - 2;    //������-2
                                sameEraseNum++; //�����������̉��Z
                                Debug.Log($"SCORE:len:{word.Length}:{word} -> {100 * Mathf.Pow(2,word.Length - 3)} : {sameEraseNum}");
                                Debug.Log($"DAMAGE:len:{word.Length}:{word} -> {damagePerChain} : {sameEraseNum}");
                                ComboNum++;  //�R���{���̒ǉ�
                                yield return new WaitForSeconds(1.5f);
                            }
                        }
                    }
                }
            }
        }

        /*** �������̕�����̔���  ���ׂ�s�̕�������擾 ***/
        targetRow = new List<int>();    //���ׂ��s��������
        targetCol = new List<int>();    //���ׂ����������
        foreach (var block in judgeTargetList)
        {
            if (!targetRow.Contains(block.CurrentRow))  //�܂����ׂĂ��Ȃ��s�Ȃ�Ύ��s
            {
                int head = 0, end = 0;  //������������̐擪��ԍ��ƏI�[��ԍ�
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);

                string str = getStringFromCol(targetRow.Last(), targetCol.Last(), ref head, ref end);   //�Ώۂ̍s�̕�������擾���擪���[�̔��f

                //3�����ȏ�
                if (str.Length >= 3)
                {
                    //List<string> findList = new List<string>() { "���", "�����", "����", "����", "���育��" };
                    Jage jage = new Jage();
                    IEnumerable<string> findList = jage.Check(str);     //�擾����������istr�j�Ɋ܂܂��P�����������擾��findList�ɑ��
                    List<string> formalList = jage.Get();     //�擾����������istr�j�Ɋ܂܂��P��̐����\������������擾��formalList�ɑ��

                    Debug.Log("List:��������F" + str);
                    var s = "List:�܂ߒP��F";
                    findList.ToList().ForEach(e =>
                    {
                        s += e + ",";

                    });
                    Debug.Log(s);
                    foreach (var (word, wIndex) in findList.Select((value, wIndex) => (value, wIndex)))
                    {
                        int index = -1; //str����word�̏o���ʒu

                        //str���Ɋ܂܂��word�����ׂĒT��
                        while (true)
                        {
                            index = str.IndexOf(word, index + 1);
                            //str����word���܂܂�Ȃ��Ƃ�
                            if (index == -1)
                            {
                                break;
                            }
                            else
                            {
                                //�R���N�V�������X�g�ɒǉ�
                                WordData addWord = new WordData(word, formalList[wIndex], "");
                                wordList.CollectList.Add(addWord);  //���������t���X�g�ɒǉ�
                                uIManager.addWordView();

                                //�{�̕\��
                                yield return uIManager.updateBook(word, formalList[wIndex]);

                                for (int i = index + head; i < index + head + word.Length; i++)
                                {
                                    /*
                                     * �Â��A�j���[�V����
                                    Block b = BlockArray[targetRow.Last(), i];
                                    b.lightUp();
                                    if (!destroyList.Contains(b)) destroyList.Add(b);
                                    yield return new WaitForSeconds(0.3f);
                                    */
                                    Block b = BlockArray[targetRow.Last(), i];
                                    b.emphasize();  //�A�j���[�V�����Đ�
                                    if (!destroyList.Contains(b)) destroyList.Add(b);

                                }
                                float pitch = 0.6f + comboNum * 0.4f;
                                audioManager.playSeOneShot(AudioKinds.SE_FindWord, pitch);
                                if (word.Length > 2) scorePerChain += 100 * (int)Mathf.Pow(2, word.Length - 3); //1�P�ꓖ����̃X�R�A 100*2^(������-3)
                                if (word.Length > 2) damagePerChain += word.Length - 2;    //������-2
                                sameEraseNum++; //�����������̉��Z
                                Debug.Log($"SCORE:len:{word.Length}:{word} -> {100 * Mathf.Pow(2, word.Length - 3)} : {sameEraseNum}");
                                Debug.Log($"DAMAGE:len:{word.Length}:{word} -> {damagePerChain} : {sameEraseNum}");
                                ComboNum++;  //�R���{���̒ǉ���UI�X�V
                                yield return new WaitForSeconds(1.5f);
                            }
                        }
                    }
                }
            }
        }

        yield return uIManager.closeWordBook();  //�{�����

        judgeTargetList.Clear();    //�m�F���X�g��������

        /*** �������ꂽ�P�ꂪ����ꍇ�A�u���b�N�폜��̗������� ***/
        if (destroyList.Count > 0)
        {

            /*
            /----------------------/if�̂��Ƃ�
            �������ꂽ�P��ɑ�������u���b�N����������
            destroyList.ForEach(block =>
            {
                BlockArray[block.CurrentRow, block.CurrentCol] = null;

                fallList.Remove(block);
                judgeTargetList.Remove(block);

                block.DestroyObject();
            });
            ------------------------------/


            Dictionary<int, int> upperGridDic = new Dictionary<int, int>();   //�񂲂Ƃɍŏ㕔�̃u���b�N�̍s�ԍ����i�[
            Dictionary<int, int> deleteNumDic = new Dictionary<int, int>();   //�񂲂Ƃɏ������u���b�N�̐����J�E���g
            for (int i = 0; i < Config.maxCol; i++)
            {
                deleteNumDic[i] = 0;
            }
            //�񂲂Ƃɍŏ㕔�̃u���b�N�Ə������u���b�N�̐����J�E���g
            destroyList.ForEach(block =>
            {
                deleteNumDic[block.CurrentCol]++;
                //block�̃J�����Ɠ������̂����� ���� ��オnull�łȂ�
                if (checkState(block.CurrentRow - 1, block.CurrentCol) != GridState.Null)
                {
                    if (upperGridDic.ContainsKey(block.CurrentCol))
                    {
                        //block�̕�����ɂ���
                        if (upperGridDic[block.CurrentCol] > block.CurrentRow)
                        {
                            upperGridDic[block.CurrentCol] = block.CurrentRow;  //��̍ŏ㕔�̃u���b�N�̍s�����X�V
                        }
                    }
                    else
                    {
                        upperGridDic[block.CurrentCol] = block.CurrentRow;
                    }
                }
            });

            //��������u���b�N�̗�����������
            List<Block> fallList = new List<Block>();   //��������u���b�N�Q
            for (int i = 0; i < BlockArray.GetLength(1); i++)
            {
                if (upperGridDic.ContainsKey(i))
                {

                    for (int j = upperGridDic[i] - 1; 0 <= j; j--)
                    {
                        if (checkState(j, i) == GridState.Null)
                        {
                            break;
                        }
                        else
                        {
                            Block b = BlockArray[j, i];
                            b.DestinationRow = b.CurrentRow + deleteNumDic[i];
                            b.BlockState = true;
                            fallList.Add(b);
                            judgeTargetList.Add(b);
                            BlockArray[j, i] = null;
                        }
                    }
                }
            }*/

            //��������u���b�N�̗�����������
            List<Block> fallList = new List<Block>();   //��������u���b�N�Q

            //�񂲂Ƃɍŏ㕔�̃u���b�N�Ə������u���b�N�̐����J�E���g
            destroyList.ForEach(block =>
            {
                
                for(int i = block.CurrentRow - 1; 0 <= i; i--)
                {
                    if (BlockArray[i, block.CurrentCol] != null)
                    {
                        Block fallBlock = BlockArray[i, block.CurrentCol];
                        Debug.Log($"TEST:{i},{block.CurrentCol}:{fallBlock.chara},{fallBlock.DestinationRow}");
                        fallBlock.DestinationRow += 1;
                        fallBlock.BlockState = true;

                        if (!fallList.Contains(fallBlock))fallList.Add(fallBlock);
                        if (!judgeTargetList.Contains(fallBlock))judgeTargetList.Add(fallBlock);
                    }
                    else
                    {
                        break;
                    }
                }
            });

            /*��������u���b�N�̌��̈ʒu���폜*/
            fallList.ForEach(block =>
            {
                BlockArray[block.CurrentRow, block.CurrentCol] = null;
            });

            /*** �������ꂽ�P��ɑ�������u���b�N���������� ***/
            destroyList.ForEach(block =>
            {
                BlockArray[block.CurrentRow, block.CurrentCol] = null;

                fallList.Remove(block);
                judgeTargetList.Remove(block);

                block.DestroyObject();
            });



            yield return new WaitForSeconds(1 / 3f);

            //�����鏈��(���ׂė��������遁���ׂĂ�BlockState=false�ɂȂ�܂�)
            if (fallList.Count > 0)
            {
                while (fallList.Count != fallList.FindAll(x => x.BlockState == false).Count)
                {
                    foreach (var b in fallList)
                    {
                        if (b.BlockState)
                        {
                            b.MoveDown();
                        }
                    }
                    //yield return new WaitForSeconds(0.0001f);
                }
                audioManager.playSeOneShot(AudioKinds.SE_BlockMove);
            }
            
        }

        //�A���Ȃ�
        if (destroyList.Count == 0)
        {
            isChained = false;
        }
        else //�A������
        {
            isChained = true;
            yield return new WaitForSeconds(0.5f);
        }

        destroyList.Clear();
        yield break;
    }

    //����̃u���b�N���܂ޏc�����i�ォ�牺�ǂ݁j�̕�������擾
    private string getStringFromRow(int row, int col, ref int head, ref int end)
    {
        //�擪�E���[���Œ[���������̂���
        head = 0;
        end = (int)BlockArray.GetLongLength(0) - 1;

        string str = BlockArray[row, col].chara.ToString();

        int r = row;
        while (0 < r)
        {
            r--;
            if (BlockArray[r, col] != null)
            {
                str = BlockArray[r, col].chara.ToString() + str;
            }
            else
            {
                head = r + 1;   //�擪�v�f�ԍ�
                break;
            }
        }

        r = row;
        while (r < BlockArray.GetLength(0) - 1)
        {
            r++;
            str = str + BlockArray[r, col].chara.ToString();
        }

        return str;
    }


    //����̃u���b�N���܂މ������i������E�ǂ݁j�̕�������擾
    private string getStringFromCol(int row, int col, ref int head, ref int end)
    {
        //�擪�E���[���Œ[���������̂���
        head = 0;
        end = (int)BlockArray.GetLongLength(1) - 1;

        string str = BlockArray[row, col].chara.ToString();
        int c = col;
        //���ɒT��
        while (0 < c)
        {
            c--;
            if (BlockArray[row, c] != null)
            {
                str = BlockArray[row, c].chara.ToString() + str;
            }
            else
            {
                head = c + 1;   //�擪�v�f�ԍ�
                break;
            }
        }

        c = col;
        //�E�ɒT��
        while (c < BlockArray.GetLength(1) - 1)
        {
            c++;
            if (BlockArray[row, c] != null)
            {
                str = str + BlockArray[row, c].chara.ToString();
            }
            else
            {
                end = c - 1;
                break;
            }
        }

        return str;
    }


    //�u���b�N�����E�Ɉړ�������
    public void moveColumn(int value)
    {
        

        foreach (var block in activeBlockList)
        {
            if (checkState(block.currentRowLine, block.CurrentCol + value) == GridState.OutStage || checkState(block.currentRowLine, block.CurrentCol + value) == GridState.Disactive)
            {
                audioManager.playSeOneShot(AudioKinds.SE_CanNotMove);
                return;
            }
        }
        audioManager.playSeOneShot(AudioKinds.SE_BlockMove);
        foreach (var block in activeBlockList)
        {
            block.CurrentCol += value;
            decideDestination();    //�ړ���̗�̖ڕW�s��������
        }
    }

    //activeBlockList[0]�𒆐S�Ɉ���dir�����̎��E��],���̎�����],0�̎��͖���]
    public void rotateBlock(float theta)
    {
        if (activeBlockList.Count != 2)
        {
            return;
        }

        //�ǂ��炩�̃u���b�N����A�N�e�B�u�Ȃ�
        if (!activeBlockList[0].BlockState || !activeBlockList[1].BlockState)
        {
            return;
        }

        
        activeBlockList.ForEach(e =>
        {
            e.isLocked = true;
        });

        //�e�u���b�N�̍s�ԍ��Ɨ�ԍ����擾
        //��]�𔽉f
        //�����Ȃ�SE�Đ�
        if (activeBlockList[1].rotate(activeBlockList[0], theta))
        {
            audioManager.playSeOneShot(AudioKinds.SE_BlockRotate);
        }  

        decideDestination();    //�ēx�ڕW�n�_��ݒ�

        //�����тȂ�@�i�o�O�΍�j
        if (activeBlockList[0].CurrentRow == activeBlockList[1].CurrentRow)
        {
            activeBlockList[1].DestinationRow = activeBlockList[0].DestinationRow;
        }
        
        activeBlockList.ForEach(e =>
        {
            e.isLocked = false;
        });

    }
    
    //���ז��u���b�N�𐶐� �����ɓn����������𗎂Ƃ�
    //n�Ƃ����Ɖ����o���Ȃ�
    public IEnumerator createObstacleBlock(string charaSet="������܂���[")
    {
        //maxCol�����ɂȂ�܂ŉE��n�Ŗ��߂�
        while (charaSet.Length < Config.maxCol)
        {
            charaSet += 'n';
        }

        List<GameObject> instanceList = new List<GameObject>();
        for(int i=0; i < Config.maxCol; i++)
        {
            //int col = Random.Range(0, Config.maxCol);

            //"n"�̂Ƃ��͉����o���Ȃ�
            if (charaSet[i].Equals('n'))
            {
                continue;
            }

            var instance = Instantiate(blockPrefab, this.gameObject.transform);
            //instance.SetActive(false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = new Vector3(1f / Config.maxCol, 1f / Config.maxRow, 1);

            Block block = instance.GetComponent<Block>();
            block.stage = this;
            block.init(charaSet[i].ToString(), 0, i);
            //block.init(decideCharacter(), 0, i);
            block.callActive();
            activeBlockList.Add(block);
            judgeTargetList.Add(block);
            
            instanceList.Add(instance);
        }

        instanceList.ForEach(e =>
        {
            //e.SetActive(true);
        });
        

        fallBoost = 28.0f;
        yield return fall(false);
        fallBoost = 1.0f;

        yield break;
    }

    public IEnumerator rowLineDelete(int row)
    {
        for (int col = 0; col < Config.maxCol; col++)
        {
            if (BlockArray[row, col] != null)
            {
                destroyList.Add(BlockArray[row, col]);
            }
        }

        yield return new WaitForSeconds(0.5f);
        fallBoost = 28.0f;
        yield return fall(false);
        fallBoost = 1.0f;

        yield break;
    }

    public IEnumerator colLineDelete(int col = 0)
    {
        for (int row = 0; row <= Config.maxRow; row++)
        {
            if (BlockArray[row, col] != null)
            {
                BlockArray[row, col].DestroyObject();
                BlockArray[row, col] = null;
            }
        }

        yield return new WaitForSeconds(0.5f);
        yield break;
    }
}
