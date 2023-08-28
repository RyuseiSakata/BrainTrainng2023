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

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow + 1, Config.maxCol];  //�X�e�[�W�S�̂̃u���b�N�z��
    public List<Block> activeBlockList = new List<Block>(); //��������u���b�N�̃��X�g
    public List<Block> judgeTargetList = new List<Block>(); //������s���Ώ�
    public List<Block>[] nextBlock = { new List<Block>(), new List<Block>() }; //���Ƃ��̎��̃u���b�N���i�[
    [SerializeField] private Transform[] SpawnPos;

    private bool isChained;  //�A������������\���t���O
    public bool CanUserOperate { get; set; } = true;   //���[�U������ł��邩�ۂ��̃t���O


    private void Start()
    {
        //�d�݂̍��v�l���i�[
        foreach (var value in Config.probability)
        {
            Config.sumProbability += value;
        }

        firstSetBlock();

        StartCoroutine("fall");
    }

    private void Update()
    {
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

    //Start�ŌĂяo��,nextBlock�Ƀu���b�N���Z�b�g����
    private void firstSetBlock()
    {
        for (int i = 0; i < 2; i++)
        {
            int randNum = Random.Range(0, 2);
            if (randNum == 0)
            {
                var instance = Instantiate(blockPrefab, SpawnPos[i].transform.position, Quaternion.identity, SpawnPos[i].transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localScale = new Vector3(0.98f / Config.maxCol, 1f / Config.maxRow, 1);
                Block block = instance.GetComponent<Block>();
                block.stage = this;
                block.init(decideCharacter(), 0, 2);
                nextBlock[i].Add(block);

            }
            else
            {
                var instance = Instantiate(blockPrefab, SpawnPos[i].transform.position, Quaternion.identity, SpawnPos[i].transform);
                instance.transform.localPosition = new Vector3(-(1f / Config.maxCol / 2f), 0f, 0f);
                instance.transform.localScale = new Vector3(0.98f / Config.maxCol, 1f / Config.maxRow, 1);
                Block block = instance.GetComponent<Block>();
                block.stage = this;
                block.init(decideCharacter(), 0, 2);
                nextBlock[i].Add(block);

                var instance2 = Instantiate(blockPrefab, SpawnPos[i].transform.position, Quaternion.identity, SpawnPos[i].transform);
                instance2.transform.localPosition = new Vector3(1f / Config.maxCol / 2f, 0f, 0f);
                instance2.transform.localScale = new Vector3(0.98f / Config.maxCol, 1f / Config.maxRow, 1);
                Block block2 = instance2.GetComponent<Block>();
                block2.stage = this;
                block2.init(decideCharacter(), 0, 3);
                nextBlock[i].Add(block2);
            }
        }
    }

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
        return Config.character[ret].ToString();
    }

    private IEnumerator fall()
    {
        while (true)
        {
            spawnBlock();

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

            fallBottom();   //�����т̃p�^�[���ɒu���Ă�������̏ꍇ�ɉ��܂ŉ�������

            Debug.Log("���n");

            isChained = true;
            while (isChained)
            {
                yield return judgeAndDelete();
            }

            fallBottom();   //��̏ꍇ�ɉ��܂ŉ�������

            CanUserOperate = true;  //���[�U�̑�����\��
        }

        yield break;
    }

    public void gameOver()
    {
        Debug.Log("game over");
        StopCoroutine("fall");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void fallBottom()
    {
        Block target = null;

        foreach (var block in activeBlockList)
        {
            if (checkState(block.CurrentRow + 1, block.CurrentCol) == GridState.Null) target = block;
        }

        activeBlockList.Clear();    //activeBlockList�̗v�f��S�폜

        if (target != null)
        {
            BlockArray[target.CurrentRow, target.CurrentCol] = null;    //���݈ʒu�̍폜
            target.BlockState = true;   //Active��Ԃ�
            activeBlockList.Add(target);

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
        }

        activeBlockList.Clear();    //activeBlockList�̗v�f��S�폜

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
        else
        {
            int row = -100; //�ړI�̍s��(�����l-100)

            //�ڕW�̗�ԍ��̌���
            for (int r = activeBlockList[0].CurrentRow + 1; r <= BlockArray.GetLength(0); r++)
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

    //����Ə����������s���@�ԋp�l��true�Ȃ�A���I��
    private IEnumerator judgeAndDelete()
    {
        List<string> horizontalString = new List<string>();
        List<string> verticalString = new List<string>();
        List<Block> destroyList = new List<Block>();

        List<int> targetRow = new List<int>();
        List<int> targetCol = new List<int>();

        //�c�����̕�����̔���  ���ׂ�s�̕�������擾
        foreach (var block in judgeTargetList)
        {
            if (!targetCol.Contains(block.CurrentCol))
            {
                int head = 0, end = 0;
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);

                string str = getStringFromRow(targetRow.Last(), targetCol.Last(), ref head, ref end);   //�Ώۂ̗�̕�������擾���擪���[�̔��f

                //3�����ȏ�
                if (str.Length >= 3)
                {
                    verticalString.Add(str);
                    //�i�ǋL�jstr�Ɋ܂܂��P��̃��X�g���擾���鏈�� List<string> wordList = (�擾���\�b�h)
                    List<string> wordList = new List<string>() { "���", "�����", "����", "����", "���育��" };
                    foreach (var word in wordList)
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
                                for (int i = index + head; i < index + head + word.Length; i++)
                                {
                                    Block b = BlockArray[i, targetCol.Last()];
                                    b.lightUp();
                                    if (!destroyList.Contains(b)) destroyList.Add(b);
                                    yield return new WaitForSeconds(0.3f);
                                }
                                yield return new WaitForSeconds(0.3f);
                                for (int i = index + head; i < index + head + word.Length; i++)
                                {
                                    Block b = BlockArray[i, targetCol.Last()];
                                    b.lightDown();
                                }


                            }
                        }
                    }
                }
            }
        }


        targetRow = new List<int>();
        targetCol = new List<int>();

        //�������̕�����̔���  ���ׂ�s�̕�������擾
        foreach (var block in judgeTargetList)
        {
            //�����s���s��Ȃ��悤�ɂ���
            if (!targetRow.Contains(block.CurrentRow))
            {
                int head = 0, end = 0;
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);
                string str = getStringFromCol(targetRow.Last(), targetCol.Last(), ref head, ref end);
                //3�����ȏ�
                if (str.Length >= 3)
                {
                    horizontalString.Add(str);
                    //�i�ǋL�jstr�Ɋ܂܂��P��̃��X�g���擾���鏈�� List<string> wordList = (�擾���\�b�h)
                    List<string> wordList = new List<string>() { "���", "�����", "����", "����", "���育��" };
                    foreach (var word in wordList)
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
                                for (int i = index + head; i < index + head + word.Length; i++)
                                {
                                    Block b = BlockArray[targetRow.Last(), i];
                                    b.lightUp();
                                    if (!destroyList.Contains(b)) destroyList.Add(b);
                                    yield return new WaitForSeconds(0.3f);
                                }
                                yield return new WaitForSeconds(0.3f);
                                for (int i = index + head; i < index + head + word.Length; i++)
                                {
                                    Block b = BlockArray[targetRow.Last(), i];
                                    b.lightDown();
                                }
                            }
                        }
                    }
                }
            }
        }

        judgeTargetList.Clear();

        //�u���b�N����������
        destroyList.ForEach(block =>
        {
            BlockArray[block.CurrentRow, block.CurrentCol] = null;
            block.DestroyObject();
        });

        //�u���b�N�폜��̗�������
        if (destroyList.Count > 0)
        {
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

                    for (int j = upperGridDic[i] - 1; 0 < j; j--)
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
            }

            //�����鏈��(���ׂė��������遁���ׂĂ�BlockState=false�ɂȂ�܂�)
            while (fallList.Count != fallList.FindAll(x => x.BlockState == false).Count)
            {
                foreach (var b in fallList)
                {
                    if (b.BlockState)
                    {
                        b.MoveDown();
                    }
                }
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
            yield return new WaitForSeconds(1f);
        }

        yield break;
    }

    //����̃u���b�N���܂ނ��牡�����i������E�ǂ݁j�̕�������擾
    private string getStringFromCol(int row, int col, ref int head, ref int end)
    {
        //�擪�E���[���Œ[���������̂���
        head = 0;
        end = (int)BlockArray.GetLongLength(1) - 1;

        string str = BlockArray[row, col].chara.ToString();
        Debug.Log(CanUserOperate);
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

    //����̃u���b�N���܂ނ���c�����i�ォ�牺�ǂ݁j�̕�������擾
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

    //�u���b�N�����E�Ɉړ�������
    public void moveColumn(int value)
    {
        foreach (var block in activeBlockList)
        {
            
            //Debug.Log(checkState(block.currentRowLine, block.CurrentCol + value));
            if (checkState(block.currentRowLine, block.CurrentCol + value) == GridState.OutStage || checkState(block.currentRowLine, block.CurrentCol + value) == GridState.Disactive)
            {
                return;
            }
        }
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

        activeBlockList.ForEach(e =>
        {
            e.isLocked = true;
        });

        //�e�u���b�N�̍s�ԍ��Ɨ�ԍ����擾
        //Vector2 center = new Vector2(activeBlockList[0].CurrentRow, activeBlockList[0].CurrentCol);
        //Vector2 target = new Vector2(activeBlockList[1].CurrentRow, activeBlockList[1].CurrentCol);

        activeBlockList[1].rotate(activeBlockList[0], theta);  //��]�𔽉f

        activeBlockList.ForEach(e =>
        {
            e.isLocked = false;
        });

        decideDestination();    //�ēx�ڕW�n�_��ݒ�
    }
}
