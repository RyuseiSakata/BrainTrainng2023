using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WordData
{
    [SerializeField] string hiragana;   //�������\�L
    [SerializeField] string word;       //���t
    [SerializeField] string meaning;    //���t�̈Ӗ�

    public string Hiragana { get => hiragana; set { hiragana = value; } }
    public string Word { get => word; set { word = value; } }
    public string Meaning { get => meaning; set { meaning = value; } }

    //�R���X�g���N�^
    public WordData(string h = "", string w = "", string m = "")
    {
        hiragana = h;
        word = w;
        meaning = m;
    }
}

[CreateAssetMenu(fileName = "WordDataList", menuName = "ScriptableObjects/CreateWordDataList")]
public class WordList : ScriptableObject
{

    [SerializeField] List<WordData> collectList = new List<WordData>();

    public List<WordData> CollectList { get => collectList; set { collectList = value; } }
}
