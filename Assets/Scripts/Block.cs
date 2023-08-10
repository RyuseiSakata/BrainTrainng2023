using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public char chara = ' ';    //所持している文字
    public bool isFalling = true; //落下中であるかのフラグ
    public int CurrentRow = 0; //現在の行
    public int CurrentCol = 0; //現在の列
    

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
            //判定を行う高さに達したら
            if (transform.position.y <= destinationHeight)
            {
                //次の行が空なら
                if (stage.checkEmpty(CurrentRow+1, CurrentCol))
                {
                    CurrentRow += 1;
                    destinationHeight = 0.75 * (11 / 2f) - 0.75 * CurrentRow;
                }
                //次の行にすでにブロックがあるなら
                else
                {
                    isFalling = false; 

                    //目標地までの移動
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


        //判定とセット
        stage.activeBlockList.Remove(this);


        yield break;    //終了
    }

    private void Move(Vector3 dir)
    {
        Vector3 pos = transform.position + dir;
        transform.position = new Vector3((float)(-1.875 + 0.75 * CurrentCol), pos.y, pos.z);
    }
}
