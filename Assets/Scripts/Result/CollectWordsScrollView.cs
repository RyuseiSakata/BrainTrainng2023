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

            var back = instance.transform.GetChild(0);      //backを取得

            var wordHiragana = back.GetChild(0);      //wordHiraganaを取得
            var wordFormal = back.GetChild(1);      //wordFormalを取得
            wordHiragana.GetComponent<Text>().text = word.Hiragana; //平仮名表記のテキストを格納
            wordFormal.GetComponent<Text>().text = word.Word; //正式表記のテキストを格納
        }
    }
}
