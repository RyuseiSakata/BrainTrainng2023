using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTest : MonoBehaviour
{
    public static void CalculteScore(string target)
    {
        int length = target.Length;

        // i ’Tõ’†‚Ì•¶š”  j Ø‚èæ‚è‚Í‚¶‚ß‚Ì—v‘f”Ô†
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



