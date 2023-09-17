using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WordData
{
    [SerializeField] string hiragana;   //平仮名表記
    [SerializeField] string word;       //言葉

    public string Hiragana { get => hiragana; set { hiragana = value; } }
    public string Word { get => word; set { word = value; } }

    //コンストラクタ
    public WordData(string h = "", string w = "")
    {
        hiragana = h;
        word = w;
    }
}

[CreateAssetMenu(fileName = "WordDataList", menuName = "ScriptableObjects/CreateWordDataList")]
public class WordList : ScriptableObject
{

    [SerializeField] List<WordData> collectList = new List<WordData>();

    public List<WordData> CollectList { get => collectList; set { collectList = value; } }
}
