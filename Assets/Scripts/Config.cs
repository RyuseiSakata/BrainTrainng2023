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
    public const float deltaX = -0.28f;

    public static int sumProbability = 0;
    public const string character = "あいうえおかきくけこがぎぐげごさしすせそざじずぜぞたちつてとだぢづでどなにぬねのはひふへほばびぶべぼぱぴぷぺぽまみむめもやゆよらりるれろわをん";
    //public const string character = "りんご";
    //public static int[] probability = { 1, 1, 1 };
    //各文字の出現確率の重み
    public static int[] probability = { 
        1, 1, 1, 1, 1,  //あ行
        1, 1, 1, 1, 1,  //か行
        1, 1, 1, 1, 1,  //が行
        1, 1, 1, 1, 1,  //さ行
        1, 1, 1, 1, 1,  //ざ行
        1, 1, 1, 1, 1,  //た行
        1, 1, 1, 1, 1,  //だ行
        1, 1, 1, 1, 1,  //な行
        1, 1, 1, 1, 1,  //は行
        1, 1, 1, 1, 1,  //ば行
        1, 1, 1, 1, 1,  //ぱ行
        1, 1, 1, 1, 1,  //ま行
        1, 1, 1,        //や行
        1, 1, 1, 1, 1,  //ら行
        1, 1, 1         //わ行 
    };
    

}
