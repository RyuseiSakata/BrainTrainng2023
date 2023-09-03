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
        //�������P�ꃊ�X�g�𑍓�����
        foreach(var word in wordList.CollectList)
        {
            var instance = Instantiate(wordWrapperPrefab, content.transform);

            var wordHiragana = instance.transform.GetChild(1);      //wordHiragana���擾
            wordHiragana.GetComponent<Text>().text = word.Hiragana; //�������\�L�̃e�L�X�g���i�[
        }
    }
}