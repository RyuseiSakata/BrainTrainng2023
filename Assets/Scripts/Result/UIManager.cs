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
        //ノーマル
        [SerializeField] private Text scoreText;
        [SerializeField] private Text rankText;

        //バトル
        [SerializeField] private Text timeText;
        [SerializeField] private Text clearText;

        //共通
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
            //消した単語リストを総当たり
            /*
            foreach(var word in wordList.CollectList)
            {
                var instance = Instantiate(wordWrapperPrefab, content.transform);

                var back = instance.transform.GetChild(0);      //backを取得

                var wordHiragana = back.GetChild(0);      //wordHiraganaを取得
                var wordFormal = back.GetChild(1);      //wordFormalを取得
                wordHiragana.GetComponent<Text>().text = word.Hiragana; //平仮名表記のテキストを格納
                wordFormal.GetComponent<Text>().text = word.Word; //正式表記のテキストを格納
            }*/

            //重複をのぞいたリストを作成
            List<WordData> distinctList = new List<WordData>();
            foreach (var data in wordList.CollectList)
            {
                if (!distinctList.Any(e=>e.Hiragana == data.Hiragana))
                {
                    distinctList.Add(data);
                }
            }

            /*五十音順に並び変える*/

            // 文字列の配列を五十音順にソート
            List<WordData> sortList = distinctList.OrderBy(e => e.Hiragana).ToList();
            /*
                //平仮名の配列を作成
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

            //消した単語リストを総当たり
            foreach (var word in wordList.CollectList)
            {
                var instance = Instantiate(wordWrapperPrefab, content.transform);

                var wordHiragana = instance.transform.GetChild(0);      //wordHiraganaを取得
                var wordFormal = instance.transform.GetChild(1);      //wordFormalを取得
                wordHiragana.GetComponent<Text>().text = word.Hiragana; //平仮名表記のテキストを格納
                wordFormal.GetComponent<Text>().text = word.Word; //正式表記のテキストを格納
            }
        }


    }
}
