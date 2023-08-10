using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public char chara = ' ';    //�������Ă��镶��
    public bool isFalling = true; //�������ł��邩�̃t���O
    public int CurrentRow = 0; //���݂̍s
    public int CurrentCol = 0; //���݂̗�
    

    public Stage stage;


    void Start()
    {
        StartCoroutine("Fall");
    }

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

    private void Move(Vector3 dir)
    {
        Vector3 pos = transform.position + dir;
        transform.position = new Vector3((float)(-1.875 + 0.75 * CurrentCol), pos.y, pos.z);
    }
}
