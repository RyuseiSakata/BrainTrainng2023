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

    public static bool isCaluculatedSum = false;    //重み合計を計算したかのフラグ
    public static int sumProbability = 0;           //重み合計
    public const string character = "あいうえおかきくけこがぎぐげごさしすせそざじずぜぞたちつてとだぢづでどなにぬねのはひふへほばびぶべぼぱぴぷぺぽまみむめもやゆよらりるれろわをんー";
    //public const string character = "りんご";
    //public static int[] probability = { 1, 1, 1 };
    //各文字の出現確率の重み
    public static int[] probability = {
        8437,15173,13497,4794,5275,  //あ行
        6967,6144,6991,2847,3838,  //か行
        3596,1642,1415,1501,1606,  //が行
        4368,8347,4819,3025,1721,  //さ行
        1061,3584,1522,532,475,  //ざ行
        4249,6344,8873,2264,4284,  //た行
        2235,61,484,1094,2372,  //だ行
        4399,2791,758,1671,3173,  //な行
        1743,1180,1637,416,967,  //は行
        2250,1578,1891,749,1110,  //ば行
        629,392,663,222,384,  //ぱ行
        5722,4209,2156,3034,2920,  //ま行
        4484,3483,5691,  //や行
        5035,6470,7452,3443,2497,  //ら行
        3463,604,12994,  //わ行
        4545,  //ー行
    };


}
