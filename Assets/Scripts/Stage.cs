using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockKinds
{
    NULL,       //��
    OUTSTAGE,   //�X�e�[�W�O
    SAMEGROUP,  //�����O���[�v�̃u���b�N
    STATICBLOCK,//�Î~���
}
public class Stage : MonoBehaviour
{

    int nn_test = 0;

    public float fallBoost = 1;

    [SerializeField] GameObject blockPrefab;

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow, Config.maxCol];  //�X�e�[�W�S�̂̃u���b�N�z��
    public List<Block> activeBlockList = new List<Block>(); //����2�܂ł�z�肵�Ă���

    private void Start()
    {
        spawnBlock(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            spawnBlock();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            spawnBlock(2);
        }

        if (Input.GetKeyDown(KeyCode.I)) { 

            string s = "\n";

            for(int i = 0; i < Config.maxRow; i++)
            {
                for(int j=0; j<Config.maxCol; j++)
                {
                    if (BlockArray[i,j] == null)
                    {
                        s += "  ,";
                    }
                    else
                    {
                        s += BlockArray[i,j].chara + ',';
                    }
                }
                s += '\n';
            }
            Debug.Log(s);
        }
    }

    public BlockKinds checkEmpty(int row, int col)
    {
        //�����̐��m���̃`�F�b�N
        if(!( 0 <= row && row < Config.maxRow && 0 <= col && col < Config.maxCol))
        {
            return BlockKinds.OUTSTAGE;
        }

        //�v�f����Ȃ�
        if (BlockArray[row, col] == null)
        {
            return BlockKinds.NULL;
        }

        //�Z�b�g�̃u���b�N�͖�������
        if (activeBlockList.Contains(BlockArray[row, col]))
        {
            return BlockKinds.SAMEGROUP;
        }


        return BlockKinds.STATICBLOCK;
    }

    public bool checkFalling(int row, int col)
    {
        if (BlockArray[row,col] !=null)
        {
            return BlockArray[row, col].isFalling;
        }
        else
        {
            Debug.Log("BlockArray is null.");
            return false;
        }
        
    }

    //Block�̐���
    private void spawnBlock(int num = 1)
    {
        if(num != 2)
        {
            var instance = Instantiate(blockPrefab);
            instance.GetComponent<Block>().stage = this;
            instance.GetComponent<Block>().chara = (nn_test++).ToString("00");
            activeBlockList.Add(instance.GetComponent<Block>());
        }
        else
        {
            var instance = Instantiate(blockPrefab);
            instance.GetComponent<Block>().stage = this;
            instance.GetComponent<Block>().chara = (nn_test++).ToString("00");
            activeBlockList.Add(instance.GetComponent<Block>());

            var instance2 = Instantiate(blockPrefab);
            instance2.GetComponent<Block>().CurrentCol += 1;
            instance2.GetComponent<Block>().stage = this;
            instance2.GetComponent<Block>().chara = (nn_test++).ToString("00");
            instance2.transform.position += new Vector3(0.75f, 0f, 0f);
            activeBlockList.Add(instance2.GetComponent<Block>());
        }
        
    }

    //��̈ړ�
    public void moveColumn(int value)
    {
        foreach(var block in activeBlockList)
        {
            if (checkEmpty(block.CurrentRow, block.CurrentCol + value) == BlockKinds.OUTSTAGE || checkEmpty(block.CurrentRow, block.CurrentCol + value) == BlockKinds.STATICBLOCK)
            {
                return;
            }
        }
        foreach(var block in activeBlockList){
            block.preCurrentRow = block.CurrentRow; //���݂̈ʒu�i�s�j��ۑ�
            block.preCurrentCol = block.CurrentCol; //���݂̈ʒu�i��j��ۑ�

            block.CurrentCol += value;  //��ԍ��̕ύX

            BlockArray[block.CurrentRow, block.CurrentCol] = block;    //�Ֆʂɏ���ǉ�
            BlockArray[block.preCurrentRow, block.preCurrentCol] = null;    //�Ֆʂɕs�v�ȏ����폜
        }
    }

    //����dir�����̎��E��],���̎�����],0�̎��͖���]
    public void rotateBlock(int dir)
    {
        
        if(activeBlockList.Count != 2)
        {
            return;
        }

        //�������������b�N
        activeBlockList[0].isLocked = true;
        activeBlockList[1].isLocked = true;

        foreach(var block in activeBlockList)
        {
            block.preCurrentRow = block.CurrentRow; //���݂̈ʒu�i�s�j��ۑ�
            block.preCurrentCol = block.CurrentCol; //���݂̈ʒu�i��j��ۑ�
        }

        //�e�u���b�N�̍s�ԍ��Ɨ�ԍ����擾
        Vector2 center = new Vector2(activeBlockList[0].CurrentRow, activeBlockList[0].CurrentCol);
        Vector2 target = new Vector2(activeBlockList[1].CurrentRow, activeBlockList[1].CurrentCol);

        //��]�s�񂩂��]��̍��W���擾
        int r=0, c=0;
        if (dir > 0)
        {
            r = (int)(target.y - center.y); //x'=(y-y_0)sin(PI/2)
            c = (int)(-(target.x - center.x));//y'=-(x-x_0)sin(PI/2)
        }
        else if (dir < 0)
        {
            r = (int)(-(target.y - center.y)); //x'=(y-y_0)sin(PI/2)
            c = (int)(target.x - center.x);//y'=-(x-x_0)sin(PI/2)
        }

        //��]��̒l�����߂�
        activeBlockList[1].CurrentRow = r + (int)center.x;
        activeBlockList[1].CurrentCol = c + (int)center.y;

        //�ύX��activeBlockList�ɔ��f
        foreach(var block in activeBlockList)
        {
            block.MoveBasedArray(); //�K�؂ȍ��W�Ɉړ�
            BlockArray[block.CurrentRow, block.CurrentCol] = block;    //�Ֆʂɏ���ǉ�
            BlockArray[block.preCurrentRow, block.preCurrentCol] = null;    //�Ֆʂɕs�v�ȏ����폜
            Debug.Log(block.chara);
        }
    }

}
