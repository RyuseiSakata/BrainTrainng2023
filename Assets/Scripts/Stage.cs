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
    public List<Block> judgeTargetList = new List<Block>(); //判定を行う対象


    private void Start()
    {
        StartCoroutine("fall");
    }

    private void Update()
    {
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
            instance.GetComponent<Block>().Init(decideCharacter(), 0, 2);
            instance.GetComponent<Block>().stage = this;
            activeBlockList.Add(instance.GetComponent<Block>());
            judgeTargetList.Add(instance.GetComponent<Block>());
        }
        else
        {
            var instance = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance.GetComponent<Block>().Init(decideCharacter(), 0, 2);
            instance.GetComponent<Block>().stage = this;
            activeBlockList.Add(instance.GetComponent<Block>());
            judgeTargetList.Add(instance.GetComponent<Block>());

            var instance2 = Instantiate(blockPrefab, this.transform.localPosition, Quaternion.identity, this.transform);
            instance2.GetComponent<Block>().stage = this;
            instance2.GetComponent<Block>().Init(decideCharacter(), 0, 3);
            activeBlockList.Add(instance2.GetComponent<Block>());
            judgeTargetList.Add(instance2.GetComponent<Block>());
        }   
    }

    private string decideCharacter()
    {
        //各文字に出現率の重みづけしてもいいかも
        string character = "あいうえおかきくけこがぎぐげごさしすせそざじずぜぞたちつてとだぢづでどなにぬねのはひふへほばびぶべぼぱぴぷぺぽまみむめもやゆよらりるれろわをん";
        int num = character.Length;
        return character[Random.Range(0,num)].ToString();
    }

    private IEnumerator fall()
    {
        while (true)
        {
            spawnBlock(2);

            decideDestination();    //目標行数の決定

            //落ちる処理(すべて落下しきる＝すべてのBlockState=falseになるまで)
            while (activeBlockList.Count != activeBlockList.FindAll(x => x.BlockState == false).Count)
            {
                activeBlockList.ForEach(b =>
                {
                    if (b.BlockState)
                    {
                        b.MoveDown();
                    }
                });

                yield return new WaitForEndOfFrame();
            }

            fallBottom();   //横並びのパターンに置いてしたが空の場合に下まで下す処理

            Debug.Log("着地");

            judgeAndDelete();
            fallBottom();   //空の場合に下まで下す処理

            // 判定＆消す処理
        }

        yield break;
    }

    public void gameOver()
    {
        Debug.Log("game over");
        StopCoroutine("fall");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void fallBottom()
    {
        Block target = null;

        foreach(var block in activeBlockList)
        {
            if(checkState(block.CurrentRow+1,block.CurrentCol) == GridState.Null)   target = block;
        }

        activeBlockList.Clear();    //activeBlockListの要素を全削除

        if (target != null)
        {
            BlockArray[target.CurrentRow, target.CurrentCol] = null;    //現在位置の削除
            target.BlockState = true;   //Active状態に
            activeBlockList.Add(target);
            
            decideDestination();

            //落ちる処理(すべて落下しきる＝すべてのBlockState=falseになるまで)
            while (activeBlockList.Count != activeBlockList.FindAll(x => x.BlockState == false).Count)
            {
                activeBlockList.ForEach(b =>
                {
                    if (b.BlockState)
                    {
                        b.MoveDown();
                    }
                });
            }
        }

        activeBlockList.Clear();    //activeBlockListの要素を全削除

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
            for(int r=row+1; r <= BlockArray.GetLength(0); r++)
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
            for (int r = activeBlockList[0].CurrentRow+1; r <= BlockArray.GetLength(0); r++)
            {
                foreach (var block in activeBlockList)
                {
                    if (checkState(r, block.CurrentCol) != GridState.Null)
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

    private void judgeAndDelete()
    {
        List<string> horizontalString = new List<string>();
        List<string> verticalString = new List<string>();

        List<int> targetRow = new List<int>();
        List<int> targetCol = new List<int>();

        //横向きの文字列の判定  調べる行の文字列を取得
        foreach (var block in judgeTargetList) 
        {
            //同じ行を行わないようにする
            if (!targetRow.Contains(block.CurrentRow)) {
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);
                string str = getStringFromCol(targetRow[targetRow.Count-1], targetCol[targetCol.Count - 1]);
                //3文字以上
                if(str.Length >= 3)
                {
                    horizontalString.Add(str);
                }
            }
        }
        
        targetRow = new List<int>();
        targetCol = new List<int>();

        //縦向きの文字列の判定  調べる行の文字列を取得
        foreach (var block in judgeTargetList)
        {
            if (!targetCol.Contains(block.CurrentCol))
            {
                targetRow.Add(block.CurrentRow);
                targetCol.Add(block.CurrentCol);
                string str = getStringFromRow(targetRow[targetRow.Count - 1], targetCol[targetCol.Count - 1]);
                //3文字以上
                if (str.Length >= 3)
                {
                    verticalString.Add(str);
                }
            }
        }

        judgeTargetList.Clear();
        
    }

    //特定のブロックを含むから横向き（左から右読み）の文字列を取得
    private string getStringFromCol(int row, int col)
    {
        string str = BlockArray[row, col].chara.ToString();

        int c = col;
        while (0 < c)
        {
            c--;
            if (BlockArray[row, c] != null)
            {
                str = BlockArray[row, c].chara.ToString() + str;
            }
            else
            {
                break;
            }
        }
        c = col;
        while(c < BlockArray.GetLength(1)-1)
        {
            c++;
            if (BlockArray[row, c] != null)
            {
                str = str + BlockArray[row, c].chara.ToString();
            }
            else
            {
                break;
            }
        }

        return str;
    }

    //特定のブロックを含むから縦向き（上から下読み）の文字列を取得
    private string getStringFromRow(int row, int col)
    {
        string str = BlockArray[row, col].chara.ToString();

        int r = row;
        while (0 < r)
        {
            r--;
            if (BlockArray[r, col] != null)
            {
                str = BlockArray[r, col].chara.ToString() + str;
            }
            else
            {
                break;
            }
        }
        r = row;
        while (r < BlockArray.GetLength(0) - 1)
        {
            r++;
            if (BlockArray[r, col] != null)
            {
                str = str + BlockArray[r, col].chara.ToString();
            }
            else
            {
                break;
            }
        }

        return str;
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
