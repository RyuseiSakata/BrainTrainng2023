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
        /*
        foreach(var word in wordList.CollectList)
        {
            var instance = Instantiate(wordWrapperPrefab, content.transform);

            var back = instance.transform.GetChild(0);      //back���擾

            var wordHiragana = back.GetChild(0);      //wordHiragana���擾
            var wordFormal = back.GetChild(1);      //wordFormal���擾
            wordHiragana.GetComponent<Text>().text = word.Hiragana; //�������\�L�̃e�L�X�g���i�[
            wordFormal.GetComponent<Text>().text = word.Word; //�����\�L�̃e�L�X�g���i�[
        }*/

        //�d�����̂��������X�g���쐬
        List<WordData> showList = new List<WordData>();
        foreach(var data in wordList.CollectList)
        {
            if (!showList.Contains(data))
            {
                showList.Add(data);
            }
        }

        //�������P�ꃊ�X�g�𑍓�����
        foreach(var word in showList)
        {
            var instance = Instantiate(wordWrapperPrefab, content.transform);

            var wordHiragana = instance.transform.GetChild(0);      //wordHiragana���擾
            var wordFormal = instance.transform.GetChild(1);      //wordFormal���擾
            wordHiragana.GetComponent<Text>().text = word.Hiragana; //�������\�L�̃e�L�X�g���i�[
            wordFormal.GetComponent<Text>().text = word.Word; //�����\�L�̃e�L�X�g���i�[
        }
    }
}
