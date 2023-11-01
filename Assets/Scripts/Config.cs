public static class Config
{

    public const int maxRow = 10; //行数 StageのScaleをmaxRow*0.9みたいにすること    インデックスは1から
    public const int maxCol = 7;    //列数    StageのScaleをmaxRow*0.9みたいにすること

    public const float StageHeight = 9f;  //ステージ高さ
    public const float StageWidth = StageHeight / ((maxRow - 1) / maxCol);  //ステージ幅
    public const float BlockWidth = StageWidth / maxCol;  //ブロックの幅
    public const float deltaX = -0.2f;

    public static bool isCaluculatedSum = false;    //重み合計を計算したかのフラグ
    public static int sumProbability = 0;           //重み合計
    public const string character = "あいうえおかきくけこがぎぐげごさしすせそざじずぜぞたちつてとだぢづでどなにぬねのはひふへほばびぶべぼぱぴぷぺぽまみむめもやゆよらりるれろわをんー";
    //各文字の出現確率の重み
    public static int[] probability = {
        10841,49126,62574,8497,10874,  //あ行
        25299,24331,26816,9979,16298,  //か行
        9933,5657,3128,4743,5105,  //が行
        12387,36850,9164,12954,7175,  //さ行
        3670,15811,3050,2317,1919,  //ざ行
        12619,13940,23569,7484,11031,  //た行
        6755,151,1108,2430,6295,  //だ行
        8613,5525,1278,3726,6075,  //な行
        7444,5521,5428,1870,4869,  //は行
        5692,3711,5661,1810,3518,  //ば行
        1011,542,751,346,747,  //ぱ行
        11128,8810,4120,6124,6851,  //ま行
        11678,15921,29102,  //や行
        9310,15405,11338,5639,5978,  //ら行
        6325,595,56728,  //わ行
        3286,  //ー行
    };

    public static float musicVolume = 0.5f;    //Music音量
    public static float seVolume = 0.5f;       //SE音量
    public static int operateMode = 2;  //操作方法 0:ボタンのみ 1:タッチのみ 2:両方
    public static int buttonSize = 5;       //ボタンサイズ
    public static int buttonLayout = 0; //ボタンの配置 0:（移動->左 回転->右）1:（移動->右 回転->左）

    public static int maxWordNum = 99999999; //単語の最大保存数(とりまほぼ無限に設定)

    //public static string[] playerNameRandom = { "とある勇者","とある村人","とあるドラゴン","名無しさん"};
}
