/*
    配列 vs List  固定サイズの場合は配列の方がわずかにアクセスは早い ->配列を採用（辞書配列に）
    Dictionary(HashTable) vs List  探索速度はO(1)vsO(n)でDictionaryの方が早い -> Dictionaryを採用（辞書の作成に）
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryTest
{

    private static Dictionary<string, string>[] vocabularyArray = new Dictionary<string, string>[]
    {
        null,
        //1文字
        new Dictionary<string, string>{
        
        },
        //2文字
        new Dictionary<string, string>{
            { "あい", "愛,藍" },
            { "あみ", "網" }
        },
        //3文字
        new Dictionary<string, string>{

        },
        //4文字
        new Dictionary<string, string>{

        },
        //5文字
        new Dictionary<string, string>{

        },
        //6文字
        new Dictionary<string, string>{

        },
    };


    public static string[] retWords(string str)
    {
        int len = str.Length;   //文字数

        //キーを含むか確認
        if (vocabularyArray[len].ContainsKey(str))
        {
            return vocabularyArray[len][str].Split(',');  //データを取得し,コンマを除去した文字列の配列を返す 
        }
        else
        {
            return null;
        }
    }
}
