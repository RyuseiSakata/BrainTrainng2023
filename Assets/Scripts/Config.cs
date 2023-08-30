using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const int maxRow = 10; //行数+1
    public const int maxCol = 7;    //列数

    public const float StageHeight = 9f;  //ステージ高さ
    public const float StageWidth = StageHeight / ((maxRow - 1) / maxCol);  //ステージ幅
    public const float BlockWidth = StageWidth / maxCol;  //ブロックの幅
    public const float deltaX = -0.28f;

    public static int sumProbability = 0;
    public const string character = "あいうえおかきくけこがぎぐげごさしすせそざじずぜぞたちつてとだぢづでどなにぬねのはひふへほばびぶべぼぱぴぷぺぽまみむめもやゆよらりるれろわをんー";
    //public const string character = "りんご";
    //public static int[] probability = { 1, 1, 1 };
    //各文字の出現確率の重み
    public static int[] probability = {
        348,1454,1463,299,237,  //あ行
        612,696,897,328,468,  //か行
        346,229,218,166,226,  //が行
        288,816,568,268,234,  //さ行
        163,423,154,95,67,  //ざ行
        469,452,1072,268,593,  //た行
        270,2,23,166,353,  //だ行
        301,257,83,206,265,  //な行
        168,139,235,109,148,  //は行
        318,273,252,161,209,  //ば行
        178,88,191,63,83,  //ぱ行
        374,407,262,268,258,  //ま行
        430,412,606,  //や行
        510,680,469,301,299,  //ら行
        220,0,1097,  //わ行
        1089,  //ー行
    };


}
