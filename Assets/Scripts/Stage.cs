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

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow+1, Config.maxCol];  //�X�e�[�W�S�̂̃u���b�N�z��
    public List<Block> activeBlockList = new List<Block>(); //��������u���b�N�̃��X�g
    public List<Block> judgeTargetList = new List<Block>(); //������s���Ώ�


    private void Start()
    {
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
        if(!( 0 <= row && row < BlockArray.GetLength(0) && 0 <= col && col < Config.maxCol))
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

    private void spawnBlock(int num=1)
    {
        if(num != 2)
        {
            var instance = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance.GetComponent<Block>().Init(decideCharacter(), 0, 2);
            instance.GetComponent<Block>().stage = this;
            activeBlockList.Add(instance.GetComponent<Block>());
            judgeTargetList.Add(instance.GetComponent<Block>());
        }
        else
        {
            var instance = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance.GetComponent<Block>().Init(decideCharacter(), 0, 2);
            instance.GetComponent<Block>().stage = this;
            activeBlockList.Add(instance.GetComponent<Block>());
            judgeTargetList.Add(instance.GetComponent<Block>());

            var instance2 = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance2.GetComponent<Block>().stage = this;
            instance2.GetComponent<Block>().Init(decideCharacter(), 0, 3);
            activeBlockList.Add(instance2.GetComponent<Block>());
            judgeTargetList.Add(instance2.GetComponent<Block>());
        }   
    }

    private string decideCharacter()
    {
        //�e�����ɏo�����̏d�݂Â����Ă���������
        string character = "�������������������������������������������������������ĂƂ����ÂłǂȂɂʂ˂̂͂Ђӂւق΂тԂׂڂς҂Ղ؂ۂ܂݂ނ߂��������������";
        int num = character.Length;
        return character[Random.Range(0,num)].ToString();
    }

    private IEnumerator fall()
    {
        while (true)
        {
            spawnBlock(2);

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

            judgeAndDelete();
            fallBottom();   //��̏ꍇ�ɉ��܂ŉ�������

            // ���聕��������
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

        foreach(var block in activeBlockList)
        {
            if(checkState(block.CurrentRow+1,block.CurrentCol) == GridState.Null)   target = block;
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
        if(activeBlockList.Count == 2 && (activeBlockList[0].CurrentCol == activeBlockList[1].CurrentCol))
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
            for(int r=row+1; r <= BlockArray.GetLength(0); r++)
            {
                if(checkState(r, col) != GridState.Null)
                {
                    if (r == 0) Debug.Log("-1��������");

                    lower.DestinationRow = r-1;
                    break;
                };
            }
            upper.DestinationRow = lower.DestinationRow - 1;
        }
        else
        {
            int row = -100; //�ړI�̍s��(�����l-100)

            //�ڕW�̗�ԍ��̌���
            for (int r = activeBlockList[0].CurrentRow+1; r <= BlockArray.GetLength(0); r++)
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
                foreach(var block in activeBlockList)
                {
                    block.DestinationRow = row;
                }
            }
        }
    }

    private void judgeAndDelete()
    {
        List<string> horizontalString = new List<string>();
        List<string> verticalString = new List<string>();

        List<int> targetRow = new List<int>();
        List<int> targetCol = new List<int>();

        //�������̕�����̔���  ���ׂ�s�̕�������擾
        foreach (var block in judgeTargetList) 
        {
            //�����s���s��Ȃ��悤�ɂ���
            if (!targetRow.Contains(block.CurrentRow)) {
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);
                string str = getStringFromCol(targetRow[targetRow.Count-1], targetCol[targetCol.Count - 1]);
                //3�����ȏ�
                if(str.Length >= 3)
                {
                    horizontalString.Add(str);
                }
            }
        }
        
        targetRow = new List<int>();
        targetCol = new List<int>();

        //�c�����̕�����̔���  ���ׂ�s�̕�������擾
        foreach (var block in judgeTargetList)
        {
            if (!targetCol.Contains(block.CurrentCol))
            {
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);
                string str = getStringFromRow(targetRow[targetRow.Count - 1], targetCol[targetCol.Count - 1]);
                //3�����ȏ�
                if (str.Length >= 3)
                {
                    verticalString.Add(str);
                }
            }
        }

        judgeTargetList.Clear();
        
    }

    //����̃u���b�N���܂ނ��牡�����i������E�ǂ݁j�̕�������擾
    private string getStringFromCol(int row, int col)
    {
        string str = BlockArray[row, col].chara.ToString();

        int c = col;
        while (0 < c)
        {
            c--;
            if (BlockArray[row, c] != null)
            {
                str = BlockArray[row, c].chara.ToString() + str;
            }
            else
            {
                break;
            }
        }
        c = col;
        while(c < BlockArray.GetLength(1)-1)
        {
            c++;
            if (BlockArray[row, c] != null)
            {
                str = str + BlockArray[row, c].chara.ToString();
            }
            else
            {
                break;
            }
        }

        return str;
    }

    //����̃u���b�N���܂ނ���c�����i�ォ�牺�ǂ݁j�̕�������擾
    private string getStringFromRow(int row, int col)
    {
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
                break;
            }
        }
        r = row;
        while (r < BlockArray.GetLength(0) - 1)
        {
            r++;
            if (BlockArray[r, col] != null)
            {
                str = str + BlockArray[r, col].chara.ToString();
            }
            else
            {
                break;
            }
        }

        return str;
    }


    //�u���b�N�����E�Ɉړ�������
    public void moveColumn(int value)
    {
        foreach(var block in activeBlockList)
        {
            //Debug.Log(checkState(block.currentRowLine, block.CurrentCol + value));
            if (checkState(block.currentRowLine, block.CurrentCol + value) == GridState.OutStage || checkState(block.currentRowLine, block.CurrentCol + value) == GridState.Disactive)
            {
                return;
            }
        }
        foreach(var block in activeBlockList){
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
