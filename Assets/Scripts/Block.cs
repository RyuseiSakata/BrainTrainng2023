using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] private Text text; //テキストを格納する

    public string chara = " ";    //所持している文字
    public bool BlockState = true;
    [SerializeField] private int currentRow = 1; //現在の行（ブロックの行の中央ライン）
    [SerializeField] private int currentCol = 2; //現在の列
    public bool isLocked = false;   // 下に降りる動きを固定するフラグ
    public int currentRowLine = 0;  //現在の行ライン（何行目に所属しうるか、ブロックの最下部）
    public int DestinationRow = 1;    //目標行数
    private float fallSpeed = 0.15f;   //落下速度

    public Stage stage; //ステージのインスタンス

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
        return (int)Mathf.Floor((Config.maxCol - 1) / 2f + Config.maxCol * posX)+2;
    }

    //y座標から行番号を求める
    public int getRowFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow - 1) / 2f - Config.maxRow * posY) + 1;
    }

    //y座標から番号遷移ラインの行番号を求める
    public int getRowLineFrom(float posY)
    {
        return (int)Mathf.Floor((Config.maxRow) / 2f + 1 - Config.maxRow * posY + 0.5f);
    }

    //ブロックの初期化を行う
    public void init(string ch = " ", int row = 1, int col = 2)
    {
        chara = ch;
        text.text = ch;
        BlockState = false;
        currentRow = row;
        currentCol = col;
        DestinationRow = row;
    }

    //ブロックステージ上に出現させる際の設定を行う
    public void callActive()
    {
        BlockState = true;
        moveProperTransformFrom(CurrentCol, CurrentRow);
        this.transform.position += new Vector3(0f, 0.5f, 0f);   //出現位置を少し上に
    }
 
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
                stage.GameOverFlag = true;
            }
            else
            {
                stage.BlockArray[CurrentRow, CurrentCol] = this;
                stage.CanUserOperate = false;
            }
        }
    }

    //Blockの回転を行う
    public void rotate(Block center, float theta)
    {
        int p_r = CurrentRow;   //テスト用
        int p_c = CurrentCol;   //テスト用

        //Debug.Log("T:pre[0](" + center.CurrentRow + "," + center.CurrentCol + "):[1](" + CurrentRow + "," + CurrentCol + ")");

        Vector3 prePos = transform.position;

        //回転行列から回転後の座標を取得
        this.transform.RotateAround(center.transform.position, Vector3.back, theta);
        this.transform.rotation = Quaternion.identity;

        //行列番号の変換
        //float euler = theta * Mathf.PI / 180f;
        //int col = (int)(Mathf.Cos(euler) * (currentCol - center.currentCol) - Mathf.Sin(euler) * (currentRow - center.currentRow)) + center.currentCol;
        //int row = (int)(Mathf.Sin(euler) * (currentCol - center.currentCol) + Mathf.Cos(euler) * (currentRow - center.currentRow)) + center.currentRow;
        int col = getColFrom(transform.localPosition.x);
        int row = getRowFrom(transform.localPosition.y);
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
            //Debug.Log("T:pre[0](" + center.CurrentRow + "," + center.CurrentCol + "):[1](" + CurrentRow + "," + CurrentCol + ")");
            if (p_r == currentRow && p_c == currentCol)
            {
                Debug.Log("不適切な変換" + "(" + p_r + "," + p_c + ")--->(" + currentRow + "," + currentCol + ")");
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
