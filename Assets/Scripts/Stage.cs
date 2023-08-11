using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockKinds
{
    NULL,       //空
    OUTSTAGE,   //ステージ外
    SAMEGROUP,  //同じグループのブロック
    STATICBLOCK,//静止状態
}
public class Stage : MonoBehaviour
{

    int nn_test = 0;

    public float fallBoost = 1;

    [SerializeField] GameObject blockPrefab;

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow, Config.maxCol];  //ステージ全体のブロック配列
    public List<Block> activeBlockList = new List<Block>(); //今は2つまでを想定している

    private void Start()
    {
        spawnBlock(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            spawnBlock();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            spawnBlock(2);
        }

        if (Input.GetKeyDown(KeyCode.I)) { 

            string s = "\n";

            for(int i = 0; i < Config.maxRow; i++)
            {
                for(int j=0; j<Config.maxCol; j++)
                {
                    if (BlockArray[i,j] == null)
                    {
                        s += "  ,";
                    }
                    else
                    {
                        s += BlockArray[i,j].chara + ',';
                    }
                }
                s += '\n';
            }
            Debug.Log(s);
        }
    }

    public BlockKinds checkEmpty(int row, int col)
    {
        //引数の正確性のチェック
        if(!( 0 <= row && row < Config.maxRow && 0 <= col && col < Config.maxCol))
        {
            return BlockKinds.OUTSTAGE;
        }

        //要素が空なら
        if (BlockArray[row, col] == null)
        {
            return BlockKinds.NULL;
        }

        //セットのブロックは無視する
        if (activeBlockList.Contains(BlockArray[row, col]))
        {
            return BlockKinds.SAMEGROUP;
        }


        return BlockKinds.STATICBLOCK;
    }

    public bool checkFalling(int row, int col)
    {
        if (BlockArray[row,col] !=null)
        {
            return BlockArray[row, col].isFalling;
        }
        else
        {
            Debug.Log("BlockArray is null.");
            return false;
        }
        
    }

    //Blockの生成
    private void spawnBlock(int num = 1)
    {
        if(num != 2)
        {
            var instance = Instantiate(blockPrefab);
            instance.GetComponent<Block>().stage = this;
            instance.GetComponent<Block>().chara = (nn_test++).ToString("00");
            activeBlockList.Add(instance.GetComponent<Block>());
        }
        else
        {
            var instance = Instantiate(blockPrefab);
            instance.GetComponent<Block>().stage = this;
            instance.GetComponent<Block>().chara = (nn_test++).ToString("00");
            activeBlockList.Add(instance.GetComponent<Block>());

            var instance2 = Instantiate(blockPrefab);
            instance2.GetComponent<Block>().CurrentCol += 1;
            instance2.GetComponent<Block>().stage = this;
            instance2.GetComponent<Block>().chara = (nn_test++).ToString("00");
            instance2.transform.position += new Vector3(0.75f, 0f, 0f);
            activeBlockList.Add(instance2.GetComponent<Block>());
        }
        
    }

    //列の移動
    public void moveColumn(int value)
    {
        foreach(var block in activeBlockList)
        {
            if (checkEmpty(block.CurrentRow, block.CurrentCol + value) == BlockKinds.OUTSTAGE || checkEmpty(block.CurrentRow, block.CurrentCol + value) == BlockKinds.STATICBLOCK)
            {
                return;
            }
        }
        foreach(var block in activeBlockList){
            block.preCurrentRow = block.CurrentRow; //現在の位置（行）を保存
            block.preCurrentCol = block.CurrentCol; //現在の位置（列）を保存

            block.CurrentCol += value;  //列番号の変更

            BlockArray[block.CurrentRow, block.CurrentCol] = block;    //盤面に情報を追加
            BlockArray[block.preCurrentRow, block.preCurrentCol] = null;    //盤面に不要な情報を削除
        }
    }

    //引数dirが正の時右回転,負の時左回転,0の時は無回転
    public void rotateBlock(int dir)
    {
        
        if(activeBlockList.Count != 2)
        {
            return;
        }

        //落下処理をロック
        activeBlockList[0].isLocked = true;
        activeBlockList[1].isLocked = true;

        foreach(var block in activeBlockList)
        {
            block.preCurrentRow = block.CurrentRow; //現在の位置（行）を保存
            block.preCurrentCol = block.CurrentCol; //現在の位置（列）を保存
        }

        //各ブロックの行番号と列番号を取得
        Vector2 center = new Vector2(activeBlockList[0].CurrentRow, activeBlockList[0].CurrentCol);
        Vector2 target = new Vector2(activeBlockList[1].CurrentRow, activeBlockList[1].CurrentCol);

        //回転行列から回転後の座標を取得
        int r=0, c=0;
        if (dir > 0)
        {
            r = (int)(target.y - center.y); //x'=(y-y_0)sin(PI/2)
            c = (int)(-(target.x - center.x));//y'=-(x-x_0)sin(PI/2)
        }
        else if (dir < 0)
        {
            r = (int)(-(target.y - center.y)); //x'=(y-y_0)sin(PI/2)
            c = (int)(target.x - center.x);//y'=-(x-x_0)sin(PI/2)
        }

        //回転後の値を求める
        activeBlockList[1].CurrentRow = r + (int)center.x;
        activeBlockList[1].CurrentCol = c + (int)center.y;

        //変更をactiveBlockListに反映
        foreach(var block in activeBlockList)
        {
            block.MoveBasedArray(); //適切な座標に移動
            BlockArray[block.CurrentRow, block.CurrentCol] = block;    //盤面に情報を追加
            BlockArray[block.preCurrentRow, block.preCurrentCol] = null;    //盤面に不要な情報を削除
            Debug.Log(block.chara);
        }
    }

}
