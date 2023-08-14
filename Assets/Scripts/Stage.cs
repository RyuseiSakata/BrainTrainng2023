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

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow+1, Config.maxCol];  //ステージ全体のブロック配列
    public List<Block> activeBlockList = new List<Block>(); //落下するブロックのリスト

    private void Start()
    {
        spawnBlock();
        StartCoroutine("fall");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (activeBlockList.Count == 0)
            {
                spawnBlock();
                StartCoroutine("fall");
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (activeBlockList.Count == 0)
            {
                spawnBlock(2);
                StartCoroutine("fall");
            }
        }

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
    
    //指定した(row,col)の状態を獲得する
    public GridState checkState(int row, int col)
    {
        //引数の正確性のチェック
        if(!( 0 <= row && row < BlockArray.GetLength(0) && 0 <= col && col < Config.maxCol))
        {
            return GridState.OutStage;
        }

        //最大行数よりも小さく,次の要素が空なら
        if (BlockArray[row, col] == null)
        {
            return GridState.Null;
        }

        //ブロックの状態がアクティブか非アクティブか
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
            instance.GetComponent<Block>().Init("0",0,0);
            instance.GetComponent<Block>().stage = this;
            activeBlockList.Add(instance.GetComponent<Block>());
        }
        else
        {
            var instance = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance.GetComponent<Block>().Init("1", 0, 2);
            instance.GetComponent<Block>().stage = this;
            activeBlockList.Add(instance.GetComponent<Block>());

            var instance2 = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance2.GetComponent<Block>().stage = this;
            instance2.GetComponent<Block>().Init("2", 1, 2);
            activeBlockList.Add(instance2.GetComponent<Block>());
        }
        
    }

    private IEnumerator fall()
    {
        decideDestination();    //目標行数の決定

        //落ちる処理(すべて落下しきる＝すべてのBlockState=falseになるまで)
        while (activeBlockList.Count != activeBlockList.FindAll(x=>x.BlockState==false).Count)
        {
            activeBlockList.ForEach(b =>
            {
                if(b.BlockState)
                {
                    b.MoveDown();
                }
            });

            yield return new WaitForEndOfFrame();
        }

        activeBlockList.Clear();    //activeBlockListの要素を全削除

        Debug.Log("着地");

        // 落ちる処理
        // 判定＆消す処理


        yield break;
    }

    //activeBlockListのブロック群が何行目まで行くかを決める
    private void decideDestination()
    {
        //落下ブロックの個数が2で,同じ列の時（縦並びブロック(2個)）
        if(activeBlockList.Count == 2 && (activeBlockList[0].CurrentCol == activeBlockList[1].CurrentCol))
        {
            Block upper, lower;
            int col = activeBlockList[0].CurrentCol;    //共通の列番号
            int row;    //下側の行番号

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

            row = lower.CurrentRow; //下側の行番号を格納

            //目標の列番号の決定
            for(int r=0; r <= BlockArray.GetLength(0); r++)
            {
                if(checkState(r, col) != GridState.Null)
                {
                    if (r == 0) Debug.Log("-1が入った");

                    lower.DestinationRow = r-1;
                    break;
                };
            }
            upper.DestinationRow = lower.DestinationRow - 1;
        }
        else
        {
            int row = -100; //目的の行数(初期値-100)

            //目標の列番号の決定
            for (int r = 0; r <= BlockArray.GetLength(0); r++)
            {
                foreach (var block in activeBlockList)
                {
                    if (checkState(r,block.CurrentCol) != GridState.Null)
                    {
                        if (r == 0) Debug.Log("-1が入った");

                        row = r - 1;
                        
                        break;
                    }
                }

                
                if (row != -100) break; //目標の行数が定まった
            }

            if (row == -100) Debug.Log("目標が定まらなった");
            else
            {
                foreach(var block in activeBlockList)
                {
                    block.DestinationRow = row;
                }
            }
        }
    }

    //ブロックを左右に移動させる
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
            decideDestination();    //移動後の列の目標行数を決定
        }
    }

    //activeBlockList[0]を中心に引数dirが正の時右回転,負の時左回転,0の時は無回転
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

        //各ブロックの行番号と列番号を取得
        //Vector2 center = new Vector2(activeBlockList[0].CurrentRow, activeBlockList[0].CurrentCol);
        //Vector2 target = new Vector2(activeBlockList[1].CurrentRow, activeBlockList[1].CurrentCol);


        activeBlockList[1].rotate(activeBlockList[0], theta);  //回転を反映
        

        activeBlockList.ForEach(e =>
        {
            e.isLocked = false;
        });

        decideDestination();    //再度目標地点を設定
    }
}
