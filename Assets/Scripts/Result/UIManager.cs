using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Globalization;

namespace Result
{
    public class UIManager : MonoBehaviour
    {
        //�m�[�}��
        [SerializeField] private Text scoreText;
        [SerializeField] private Text rankText;

        //�o�g��
        [SerializeField] private Text timeText;
        [SerializeField] private Text clearText;

        //����
        [SerializeField] private Text maxComboText;
        [SerializeField] private Text wordNumText;
        [SerializeField] private WordList wordList;
        [SerializeField] GameObject content;
        [SerializeField] GameObject wordWrapperPrefab;

        private void Start()
        {
            showCollectWordsScrollView();

            if (SceneChanger.getCurrentSceneName() == "NormalResult")
            {
                scoreText.text = GameController.score.ToString("0000000");
                rankText.text = GameController.rank;
                maxComboText.text = Stage.maxComboNum.ToString("#0");
                wordNumText.text = wordList.CollectList.Count.ToString("");
            }else if (SceneChanger.getCurrentSceneName() == "AdventureResult")
            {
                int second = Mathf.FloorToInt(GameController.gameTime);
                int min = second > 99 * 60 ? 99 : (second / 60);
                second = second > 99 * 60 ? 59 : second % 60;
                timeText.text = min.ToString("00") + ":" + second.ToString("00");

                clearText.text = GameController.faseCount.ToString("0") + " / 3";
                maxComboText.text = Stage.maxComboNum.ToString("#0");
                wordNumText.text = wordList.CollectList.Count.ToString("");
            }
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
            List<WordData> distinctList = new List<WordData>();
            foreach (var data in wordList.CollectList)
            {
                if (!distinctList.Any(e=>e.Hiragana == data.Hiragana))
                {
                    distinctList.Add(data);
                }
            }

            /*�܏\�����ɕ��ѕς���*/

            // ������̔z����܏\�����Ƀ\�[�g
            List<WordData> sortList = distinctList.OrderBy(e => e.Hiragana).ToList();
            /*
                //�������̔z����쐬
                List<string> hiraganaList = new List<string>();
                foreach (var data in distinctList)
                {
                    hiraganaList.Add(data.Hiragana);
                }
                hiraganaList = hiraganaList.OrderBy(e => e).ToList();

                List<WordData> sortList = new List<WordData>();
                foreach (var hiragana in hiraganaList)
                {
                    sortList.Add(distinctList.Find(e => e.Hiragana == hiragana));
                }
            */

            wordList.CollectList = sortList;

            //�������P�ꃊ�X�g�𑍓�����
            foreach (var word in wordList.CollectList)
            {
                var instance = Instantiate(wordWrapperPrefab, content.transform);

                var wordHiragana = instance.transform.GetChild(0);      //wordHiragana���擾
                var wordFormal = instance.transform.GetChild(1);      //wordFormal���擾
                wordHiragana.GetComponent<Text>().text = word.Hiragana; //�������\�L�̃e�L�X�g���i�[
                wordFormal.GetComponent<Text>().text = word.Word; //�����\�L�̃e�L�X�g���i�[
            }
        }


    }
}
