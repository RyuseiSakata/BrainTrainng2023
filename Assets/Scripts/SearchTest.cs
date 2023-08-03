using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTest : MonoBehaviour
{
    public static void CalculteScore(string target)
    {
        int length = target.Length;

        // i 探索中の文字数  j 切り取りはじめの要素番号
        for(int i=length; 0<i; i--)
        {
            for(int j=0; j<=length-i; j++)
            {
                string targetWord = target.Substring(j, i);

                string[] words = DictionaryTest.retWords(targetWord);
                if(words == null)
                {
                    //Debug.Log("Not Found.");
                }
                else
                {
                    foreach (var word in words)
                    {
                        Debug.Log(word);
                    }
                    
                }
            }
        }
    }
}



