using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] private Text text; //�e�L�X�g���i�[����

    public string chara = " ";    //�������Ă��镶��
    public bool BlockState = true;
    [SerializeField] private int currentRow = 1; //���݂̍s
    [SerializeField] private int currentCol = 2; //���݂̗�
    public bool isLocked = false;   // ���ɍ~��铮�����Œ肷��t���O
    public int currentRowLine = 0;
    public int DestinationRow = 1;    //�ڕW�s��
    private float fallSpeed = 0.15f;   //�������x

    public Stage stage;

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
        return (int)Mathf.Floor((Config.maxCol - 1) / 2f + Config.maxCol * posX);
    }

    //y���W����s�ԍ������߂�
    public int getRowFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow - 1) / 2f + 1 - Config.maxRow * posY);
    }

    //y���W����ԍ��J�ڃ��C���̍s�ԍ������߂�
    public int getRowLineFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow) / 2f + 1 - Config.maxRow * posY + 0.5f);
    }

    public void init(string ch = " ", int row = 1, int col = 2)
    {
        chara = ch;
        text.text = ch;
        BlockState = false;
        currentRow = row;
        currentCol = col;
        DestinationRow = row;
    }

    public void callActive()
    {
        BlockState = true;
        moveProperTransformFrom(CurrentCol, CurrentRow);
        this.transform.position += new Vector3(0f, 0.5f, 0f);   //�o���ʒu���������
    }
    /*
        private IEnumerator Fall()
        {
            double destinationHeight = 0.75 * (11 / 2f) - 0.75 * CurrentRow;

            while (isFalling)
            {
                //������s�������ɒB������
                if (transform.position.y <= destinationHeight)
                {
                    //���̍s����Ȃ�
                    if (stage.checkEmpty(CurrentRow+1, CurrentCol))
                    {
                        CurrentRow += 1;
                        destinationHeight = 0.75 * (11 / 2f) - 0.75 * CurrentRow;
                    }
                    //���̍s�ɂ��łɃu���b�N������Ȃ�
                    else
                    {
                        isFalling = false; 

                        //�ڕW�n�܂ł̈ړ�
                        while (transform.position.y != destinationHeight)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, new Vector3((float)(-1.875 + 0.75*CurrentCol), (float)destinationHeight, 0.0f), 0.01f*stage.fallBoost);
                            yield return new WaitForSeconds(0.01f);
                        }
                        stage.BlockArray[CurrentRow, CurrentCol] = this;
                    }
                }
                else
                {
                    Move(new Vector3(0.0f, -0.01f * stage.fallBoost, 0.0f));
                }

                yield return new WaitForSeconds(0.01f);
            }


            //����ƃZ�b�g
            stage.activeBlockList.Remove(this);


            yield break;    //�I��
        }
    */

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
                stage.gameOver();
            }
            else
            {
                stage.BlockArray[CurrentRow, CurrentCol] = this;
            }

        }
    }

    //Block�̉�]���s��
    public void rotate(Block center, float theta)
    {
        Vector3 prePos = transform.position;

        //��]�s�񂩂��]��̍��W���擾
        this.transform.RotateAround(center.transform.position, Vector3.back, theta);
        this.transform.rotation = Quaternion.identity;

        //�s��ԍ��̕ϊ�
        float euler = theta * Mathf.PI / 180f;
        int col = (int)(Mathf.Cos(euler) * (currentCol - center.currentCol) - Mathf.Sin(euler) * (currentRow - center.currentRow)) + center.currentCol;
        int row = (int)(Mathf.Sin(euler) * (currentCol - center.currentCol) + Mathf.Cos(euler) * (currentRow - center.currentRow)) + center.currentRow;
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
