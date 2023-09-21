using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


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
        [SerializeField] Scrollbar scrollbar;
        [SerializeField] Button toTitleButton;

        [SerializeField] GameObject rankingPanel;
        [SerializeField] GameObject noDataPanel;
        [SerializeField] private Ranking ranking;
        private string[] rankingLabel = { "1st", "2nd", "3rd", "4th", "5th" };
        [SerializeField] private Text[] rankingLabelText;   //1st�Ȃ�
        [SerializeField] private Text[] rankingNameText;
        [SerializeField] private Text[] rankingScoreText;
        [SerializeField] private Text[] rankingTimeText;


        private void Start()
        {
            
            showCollectWordsScrollView();

            if (SceneChanger.getCurrentSceneName() == "NormalResult")
            {
                StartCoroutine(updateNormalRanking());    //�����L���O�̍X�V
                scoreText.text = GameController.score.ToString("0000000");
                rankText.text = GameController.rank;
                maxComboText.text = Stage.maxComboNum.ToString("#0");
                wordNumText.text = wordList.CollectList.Count.ToString("");
            }else if (SceneChanger.getCurrentSceneName() == "AdventureResult")
            {
                StartCoroutine(updateAdventureRanking());    //�����L���O�̍X�V
                int second = Mathf.FloorToInt(GameController.gameTime);
                int min = second > 99 * 60 ? 99 : (second / 60);
                second = second > 99 * 60 ? 59 : second % 60;
                timeText.text = min.ToString("00") + ":" + second.ToString("00");

                clearText.text = GameController.faseCount.ToString("0") + " / 3";
                maxComboText.text = Stage.maxComboNum.ToString("#0");
                wordNumText.text = wordList.CollectList.Count.ToString("");
            }
        }

        private void Update()
        {
            
            if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                scrollbar.Select();
            }
            
            if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                toTitleButton.Select();
            }

            if (Input.GetMouseButtonDown(0))
            {
                closeRankingPanel();
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

        private IEnumerator updateNormalRanking()
        {
            ranking.writeNormalRankingData(GameController.playerName,GameController.score);
            yield return new WaitUntil(()=>Ranking.isLoaded == true);
            Ranking.isLoaded = false;

            List<RankingData> rankingList =  Ranking.rankingList;
            int rank = 0;   //�\�����鏇��
            if(rankingList.Count != 0)
            {
                for (int i = 0; i < rankingList.Count; i++)
                {
                    //i != 0�@���@�ЂƂO�Ɠ����X�R�A�łȂ�
                    if ( i != 0 && rankingList[i-1].score != rankingList[i].score)
                    {
                        rank++;
                    }
                    rankingLabelText[i].text = rankingLabel[rank];
                    rankingNameText[i].text = rankingList[i].name;
                    rankingScoreText[i].text = rankingList[i].score.ToString("0000000");
                }
            }
            else
            {
                noDataPanel.SetActive(true);
            }

            yield break;
        }

        private IEnumerator updateAdventureRanking()
        {
            
            ranking.writeAdventureRanking(GameController.playerName, GameController.gameTime);
            yield return new WaitUntil(() => Ranking.isLoaded == true);
            Ranking.isLoaded = false;

            List<RankingData> rankingList = Ranking.rankingList;
            int rank = 0;  //�\�����鏇��
            if (rankingList.Count != 0)
            {
                for (int i = 0; i < rankingList.Count; i++)
                {
                    //�ЂƂO�Ɠ����X�R�A�łȂ��@���@i != 0
                    if (i != 0 && rankingList[i - 1].score != rankingList[i].score)
                    {
                        rank++;
                    }
                    rankingLabelText[i].text = rankingLabel[rank];
                    rankingNameText[i].text = rankingList[i].name;

                    int second = Mathf.FloorToInt(rankingList[i].time);
                    int min = second > 99 * 60 ? 99 : (second / 60);
                    second = second > 99 * 60 ? 59 : second % 60;
                    rankingTimeText[i].text = min.ToString("00") + ":" + second.ToString("00");
                }
            }
            else
            {
                noDataPanel.SetActive(true);
            }
            

            yield break;
        }

        public void showRankingPanel()
        {
            rankingPanel.SetActive(true);
        }

        public void closeRankingPanel()
        {
            rankingPanel.SetActive(false);
        }

    }
}
