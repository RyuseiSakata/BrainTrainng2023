using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{ 
    public float fallBoost = 1;

    [SerializeField] GameObject blockPrefab;

    public Block[,] BlockArray { get; set; } = new Block[Config.maxRow, Config.maxCol];  //ステージ全体のブロック配列
    public List<Block> activeBlockList = new List<Block>();

    private void Start()
    {
        spawnBlock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            spawnBlock();
        }
    }

    public bool checkEmpty(int row, int col)
    {
        Debug.Log(row + "," + col);
        //引数の正確性のチェック
        if(!( 0 <= row && row < Config.maxRow && 0 <= col && col < Config.maxCol))
        {
            Debug.Log("ERROR");
            return false;
        }
        Debug.Log(BlockArray[row, col] == null);
        //最大行数よりも小さく,次の要素が空なら
        if (row < Config.maxRow && BlockArray[row, col] == null)
        {
            return true;
        }

        return false;
    }

    private void spawnBlock()
    {
        var instance = Instantiate(blockPrefab);
        instance.GetComponent<Block>().stage = this;
        activeBlockList.Add(instance.GetComponent<Block>());
    }

    public void MoveColumn(int value)
    {
        foreach(var block in activeBlockList)
        {
            if (!checkEmpty(block.CurrentRow, block.CurrentCol + value))
            {
                return;
            }
        }
        Debug.Log("a");
        foreach(var block in activeBlockList){
            block.CurrentCol += value;
        }
    }

}
