using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] private Text text; //�e�L�X�g���i�[����

    public string chara = " ";    //�������Ă��镶��
    public bool BlockState = true;
    [SerializeField] private int currentRow = 1; //���݂̍s�i�u���b�N�̍s�̒������C���j
    [SerializeField] private int currentCol = 2; //���݂̗�
    public bool isLocked = false;   // ���ɍ~��铮�����Œ肷��t���O
    public int currentRowLine = 0;  //���݂̍s���C���i���s�ڂɏ��������邩�A�u���b�N�̍ŉ����j
    public int DestinationRow = 1;    //�ڕW�s��
    private float fallSpeed = 0.15f;   //�������x

    public Stage stage; //�X�e�[�W�̃C���X�^���X

    //getter,setter
    public int CurrentRow
    {
        get { return currentRow; }
        set
        {
            currentRow = value;
            moveProperTransformFrom(1, CurrentCol, CurrentRow);
        }
    }
    public int CurrentCol
    {
        get { return currentCol; }
        set
        {
            currentCol = value;
            moveProperTransformFrom(0, CurrentCol, CurrentRow);
        }
    }

    //�u���b�N�̍s��ԍ�����K�؂�Transform�ɕω�������(0:col(x)�̂ݔ��f, 1:row(y)�̂ݔ��f, ��0,��1:�����𔽉f)
    private void moveProperTransformFrom(int mode = -1, int col = 0, int row = 0)
    {
        if (mode == 0)
        {
            Vector3 pos = getVector3From(CurrentCol, CurrentRow);
            transform.localPosition = new Vector3(pos.x, transform.localPosition.y, 0);
            transform.localScale = new Vector3(0.98f / Config.maxCol, 1f / Config.maxRow, 1);
        }
        else if (mode == 1)
        {
            Vector3 pos = getVector3From(CurrentCol, CurrentRow);
            transform.localPosition = new Vector3(transform.localPosition.x, pos.y, 0);
            transform.localScale = new Vector3(0.98f / Config.maxCol, 1f / Config.maxRow, 1);
        }
        else
        {
            Vector3 pos = getVector3From(CurrentCol, CurrentRow);
            transform.localPosition = new Vector3(pos.x, pos.y, 0);
            transform.localScale = new Vector3(0.98f / Config.maxCol, 1f / Config.maxRow, 1);
        }

    }

    //�s��ԍ�����ʒu�����߂�
    private Vector3 getVector3From(int col, int row)
    {
        Vector3 pos = Vector3.zero;
        pos.x = (col - (Config.maxCol - 1) / 2f) / Config.maxCol + Config.deltaX;
        pos.y = -(row - (Config.maxRow - 1) / 2f - 1) / Config.maxRow;
        return pos;
    }

    //x���W�����ԍ������߂�
    public int getColFrom(float posX)
    {
        return (int)Mathf.Floor((Config.maxCol - 1) / 2f + Config.maxCol * posX)+2;
    }

    //y���W����s�ԍ������߂�
    public int getRowFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow - 1) / 2f - Config.maxRow * posY) + 1;
    }

    //y���W����ԍ��J�ڃ��C���̍s�ԍ������߂�
    public int getRowLineFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow) / 2f + 1 - Config.maxRow * posY + 0.5f);
    }

    //�u���b�N�̏��������s��
    public void init(string ch = " ", int row = 1, int col = 2)
    {
        chara = ch;
        text.text = ch;
        BlockState = false;
        currentRow = row;
        currentCol = col;
        DestinationRow = row;
    }

    //�u���b�N�X�e�[�W��ɏo��������ۂ̐ݒ���s��
    public void callActive()
    {
        BlockState = true;
        moveProperTransformFrom(CurrentCol, CurrentRow);
        this.transform.position += new Vector3(0f, 0.5f, 0f);   //�o���ʒu���������
    }
 
    //�u���b�N�����Ɉړ������鏈��
    public void MoveDown()
    {
        Vector3 destinationPos = getVector3From(CurrentCol, DestinationRow);
        if (!isLocked)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destinationPos, fallSpeed * stage.fallBoost * Time.deltaTime);
            currentRow = getRowFrom(transform.localPosition.y);   //���݂̍s���������ɂ��v�Z
            currentRowLine = getRowLineFrom(transform.localPosition.y);
        }

        if (Mathf.Abs(this.transform.localPosition.y - getVector3From(0, DestinationRow).y) == 0)
        {
            BlockState = false;

            if (CurrentRow == -1)
            {
                stage.GameOverFlag = true;
            }
            else
            {
                stage.BlockArray[CurrentRow, CurrentCol] = this;
                stage.CanUserOperate = false;
            }
        }
    }

    //Block�̉�]���s��
    public void rotate(Block center, float theta)
    {
        int p_r = CurrentRow;   //�e�X�g�p
        int p_c = CurrentCol;   //�e�X�g�p

        //Debug.Log("T:pre[0](" + center.CurrentRow + "," + center.CurrentCol + "):[1](" + CurrentRow + "," + CurrentCol + ")");

        Vector3 prePos = transform.position;

        //��]�s�񂩂��]��̍��W���擾
        this.transform.RotateAround(center.transform.position, Vector3.back, theta);
        this.transform.rotation = Quaternion.identity;

        //�s��ԍ��̕ϊ�
        //float euler = theta * Mathf.PI / 180f;
        //int col = (int)(Mathf.Cos(euler) * (currentCol - center.currentCol) - Mathf.Sin(euler) * (currentRow - center.currentRow)) + center.currentCol;
        //int row = (int)(Mathf.Sin(euler) * (currentCol - center.currentCol) + Mathf.Cos(euler) * (currentRow - center.currentRow)) + center.currentRow;
        int col = getColFrom(transform.localPosition.x);
        int row = getRowFrom(transform.localPosition.y);
        int rowLine = getRowLineFrom(transform.localPosition.y);

        //�u���b�N�̉�]�ʒu���s�K�؂łȂ���
        if (stage.checkState(rowLine, col) == GridState.OutStage || stage.checkState(rowLine, col) == GridState.Disactive)
        {
            Debug.Log("��]�Ɏ��s");
            //�␳����������Ȃ炱��
            transform.position = prePos;
        }
        else
        {
            currentCol = col;
            currentRow = row;
            currentRowLine = rowLine;
            //Debug.Log("T:pre[0](" + center.CurrentRow + "," + center.CurrentCol + "):[1](" + CurrentRow + "," + CurrentCol + ")");
            if (p_r == currentRow && p_c == currentCol)
            {
                Debug.Log("�s�K�؂ȕϊ�" + "(" + p_r + "," + p_c + ")--->(" + currentRow + "," + currentCol + ")");
            }
        }

        
        
    }

    public void lightUp()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
    }
    public void lightDown()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(142, 142, 142, 255);
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
