using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const int maxRow = 10; //行数+1
    public const int maxCol = 7;    //列数

    public const float StageHeight = 9f;  //ステージ高さ
    public const float StageWidth = StageHeight / ((maxRow-1)/maxCol);  //ステージ幅
    public const float BlockWidth = StageWidth / maxCol;  //ブロックの幅

}
