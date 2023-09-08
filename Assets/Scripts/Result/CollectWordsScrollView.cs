using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectWordsScrollView : MonoBehaviour
{
    //[SerializeField] GameObject scrollView;
    [SerializeField] GameObject content;
    [SerializeField] GameObject wordWrapperPrefab;

    [SerializeField] WordList wordList;

    private void Start()
    {
        showCollectWordsScrollView();
    }

    private void showCollectWordsScrollView()
    {
        //消した単語リストを総当たり
        foreach(var word in wordList.CollectList)
        {
            var instance = Instantiate(wordWrapperPrefab, content.transform);

            var wordHiragana = instance.transform.GetChild(1);      //wordHiraganaを取得
            wordHiragana.GetComponent<Text>().text = word.Hiragana; //平仮名表記のテキストを格納
        }
    }
}
