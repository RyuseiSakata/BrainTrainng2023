using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public string chara = " ";    //�������Ă��镶��
    public bool isFalling = true; //�������ł��邩�̃t���O
    public bool isLocked = false;
    public int CurrentRow; //���݂̍s
    public int CurrentCol; //���݂̗�
    public int preCurrentRow; //�ЂƂO�̍s
    public int preCurrentCol; //�ЂƂO�̗�
    private double destinationHeight;

    public Stage stage;


    void Start()
    {
        StartCoroutine("Fall");
    }

    private IEnumerator Fall()
    {
        destinationHeight = 0.75 * (13 / 2f) - 0.75 * CurrentRow;

        while (isFalling)
        {
            if (!isLocked)
            {
                //������s�������ɒB������
                if (transform.position.y <= destinationHeight)
                {
                    //���̍s����Ȃ�
                    if (stage.checkEmpty(CurrentRow + 1, CurrentCol)==BlockKinds.NULL|| (stage.checkEmpty(CurrentRow + 1, CurrentCol) == BlockKinds.SAMEGROUP && stage.checkFalling(CurrentRow+1,CurrentCol)))
                    {
                        preCurrentRow = CurrentRow; //���݂̈ʒu�i�s�j��ۑ�
                        preCurrentCol = CurrentCol; //���݂̈ʒu�i��j��ۑ�

                        CurrentRow += 1;
                        destinationHeight = 0.75 * (13 / 2f) - 0.75 * CurrentRow;

                        stage.BlockArray[CurrentRow, CurrentCol] = this;    //�Ֆʂɏ���ǉ�
                        if (stage.BlockArray[preCurrentRow, preCurrentCol] == this)
                            stage.BlockArray[preCurrentRow, preCurrentCol] = null;    //�Ֆʂɕs�v�ȏ����폜
                        
                    }
                    //���̍s�ɂ��łɃu���b�N������Ȃ�
                    else
                    {
                        isFalling = false;
                        
                        //�ڕW�n�܂ł̈ړ�
                        while (transform.position.y != destinationHeight)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, new Vector3((float)(-1.875 + 0.75 * CurrentCol), (float)destinationHeight, 0.0f), 0.01f * stage.fallBoost);
                            yield return new WaitForSeconds(0.01f);
                        }
                        
                    }
                }
                else
                {
                    Move(new Vector3(0.0f, -0.01f * stage.fallBoost, 0.0f));    //��ɉ��ɓ�����
                }

                yield return new WaitForSeconds(0.01f);
            }
            
        }


        //����ƃZ�b�g
        stage.activeBlockList.Remove(this);


        yield break;    //�I��
    }

    //dir�̕����ɐi��
    private void Move(Vector3 dir)
    {
        Vector3 pos = transform.position + dir;

        float x = (float)(-1.875 + 0.75 * CurrentCol);
        transform.position = new Vector3(x, pos.y, pos.z);
    }

    //currentRow,currentColmn�Ɋ�Â��ړ�������
    public void MoveBasedArray()
    {
        destinationHeight = 0.75 * (13 / 2f) - 0.75 * CurrentRow;
        float x = (float)(-1.875 + 0.75 * CurrentCol);
        float y = (float)(0.75 * (13 / 2f) - 0.75 * CurrentRow);
        transform.position = new Vector3(x, y, 0);
        isLocked = false;
    }
}
