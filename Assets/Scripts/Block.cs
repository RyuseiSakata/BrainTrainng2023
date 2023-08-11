using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public string chara = " ";    //所持している文字
    public bool isFalling = true; //落下中であるかのフラグ
    public bool isLocked = false;
    public int CurrentRow; //現在の行
    public int CurrentCol; //現在の列
    public int preCurrentRow; //ひとつ前の行
    public int preCurrentCol; //ひとつ前の列
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
                //判定を行う高さに達したら
                if (transform.position.y <= destinationHeight)
                {
                    //次の行が空なら
                    if (stage.checkEmpty(CurrentRow + 1, CurrentCol)==BlockKinds.NULL|| (stage.checkEmpty(CurrentRow + 1, CurrentCol) == BlockKinds.SAMEGROUP && stage.checkFalling(CurrentRow+1,CurrentCol)))
                    {
                        preCurrentRow = CurrentRow; //現在の位置（行）を保存
                        preCurrentCol = CurrentCol; //現在の位置（列）を保存

                        CurrentRow += 1;
                        destinationHeight = 0.75 * (13 / 2f) - 0.75 * CurrentRow;

                        stage.BlockArray[CurrentRow, CurrentCol] = this;    //盤面に情報を追加
                        if (stage.BlockArray[preCurrentRow, preCurrentCol] == this)
                            stage.BlockArray[preCurrentRow, preCurrentCol] = null;    //盤面に不要な情報を削除
                        
                    }
                    //次の行にすでにブロックがあるなら
                    else
                    {
                        isFalling = false;
                        
                        //目標地までの移動
                        while (transform.position.y != destinationHeight)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, new Vector3((float)(-1.875 + 0.75 * CurrentCol), (float)destinationHeight, 0.0f), 0.01f * stage.fallBoost);
                            yield return new WaitForSeconds(0.01f);
                        }
                        
                    }
                }
                else
                {
                    Move(new Vector3(0.0f, -0.01f * stage.fallBoost, 0.0f));    //常に下に動かす
                }

                yield return new WaitForSeconds(0.01f);
            }
            
        }


        //判定とセット
        stage.activeBlockList.Remove(this);


        yield break;    //終了
    }

    //dirの方向に進む
    private void Move(Vector3 dir)
    {
        Vector3 pos = transform.position + dir;

        float x = (float)(-1.875 + 0.75 * CurrentCol);
        transform.position = new Vector3(x, pos.y, pos.z);
    }

    //currentRow,currentColmnに基づき移動させる
    public void MoveBasedArray()
    {
        destinationHeight = 0.75 * (13 / 2f) - 0.75 * CurrentRow;
        float x = (float)(-1.875 + 0.75 * CurrentCol);
        float y = (float)(0.75 * (13 / 2f) - 0.75 * CurrentRow);
        transform.position = new Vector3(x, y, 0);
        isLocked = false;
    }
}
