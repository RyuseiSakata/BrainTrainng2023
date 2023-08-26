using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] private Text text; //テキストを格納する

    public string chara = " ";    //所持している文字
    public bool BlockState = true;
    [SerializeField] private int currentRow = 1; //現在の行
    [SerializeField] private int currentCol = 2; //現在の列
    public bool isLocked = false;   // 下に降りる動きを固定するフラグ
    public int currentRowLine = 0;
    public int DestinationRow = 1;    //目標行数
    private float fallSpeed = 0.15f;   //落下速度

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

    //ブロックの行列番号から適切なTransformに変化させる(0:col(x)のみ反映, 1:row(y)のみ反映, 非0,非1:両方を反映)
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

    //行列番号から位置を求める
    private Vector3 getVector3From(int col, int row)
    {
        Vector3 pos = Vector3.zero;
        pos.x = (col - (Config.maxCol - 1) / 2f) / Config.maxCol + Config.deltaX;
        pos.y = -(row - (Config.maxRow - 1) / 2f - 1) / Config.maxRow;
        return pos;
    }

    //x座標から列番号を求める
    public int getColFrom(float posX)
    {
        return (int)Mathf.Floor((Config.maxCol - 1) / 2f + Config.maxCol * posX);
    }

    //y座標から行番号を求める
    public int getRowFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow - 1) / 2f + 1 - Config.maxRow * posY);
    }

    //y座標から番号遷移ラインの行番号を求める
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
        this.transform.position += new Vector3(0f, 0.5f, 0f);   //出現位置を少し上に
    }
    /*
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
    */

    //ブロックを下に移動させる処理
    public void MoveDown()
    {

        Vector3 destinationPos = getVector3From(CurrentCol, DestinationRow);
        if (!isLocked)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destinationPos, fallSpeed * stage.fallBoost * Time.deltaTime);
            currentRow = getRowFrom(transform.localPosition.y);   //現在の行数を高さにより計算
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

    //Blockの回転を行う
    public void rotate(Block center, float theta)
    {
        Vector3 prePos = transform.position;

        //回転行列から回転後の座標を取得
        this.transform.RotateAround(center.transform.position, Vector3.back, theta);
        this.transform.rotation = Quaternion.identity;

        //行列番号の変換
        float euler = theta * Mathf.PI / 180f;
        int col = (int)(Mathf.Cos(euler) * (currentCol - center.currentCol) - Mathf.Sin(euler) * (currentRow - center.currentRow)) + center.currentCol;
        int row = (int)(Mathf.Sin(euler) * (currentCol - center.currentCol) + Mathf.Cos(euler) * (currentRow - center.currentRow)) + center.currentRow;
        int rowLine = getRowLineFrom(transform.localPosition.y);

        //ブロックの回転位置が不適切でないか
        if (stage.checkState(rowLine, col) == GridState.OutStage || stage.checkState(rowLine, col) == GridState.Disactive)
        {
            Debug.Log("回転に失敗");
            //補正処理を入れるならここ
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
