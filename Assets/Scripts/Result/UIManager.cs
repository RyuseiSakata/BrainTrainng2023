using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private void Start()
        {
            if (SceneChanger.getCurrentSceneName() == "NormalResult")
            {
                scoreText.text = GameController.score.ToString("0000000");
                rankText.text = GameController.rank;
                maxComboText.text = Stage.maxComboNum.ToString("00");
                wordNumText.text = wordList.CollectList.Count.ToString("");
            }else if (SceneChanger.getCurrentSceneName() == "AdventureResult")
            {
                int second = Mathf.FloorToInt(GameController.gameTime);
                int min = second > 99 * 60 ? 99 : (second / 60);
                second = second % 60;
                timeText.text = min.ToString("00") + ":" + second.ToString("00");

                clearText.text = GameController.faseCount.ToString("0") + " / 3";
                maxComboText.text = Stage.maxComboNum.ToString("00");
                wordNumText.text = wordList.CollectList.Count.ToString("");
            }
        }
    }
}
