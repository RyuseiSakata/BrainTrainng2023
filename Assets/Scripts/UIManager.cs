using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum TextKinds
{
    Combo,
    CountDown,
    Score,
    MaxCombo,
    Rank,
}

public class UIManager : MonoBehaviour
{
    //ボタンの配置
    [SerializeField] GameObject buttonLayout_0;
    [SerializeField] GameObject buttonLayout_1;

    //ボタンサイズ
    [SerializeField] GameObject[] buttons;

    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;
    [SerializeField] Text maxComboText;
    [SerializeField] Text rankText;
    [SerializeField] GameObject countDownText;
    [SerializeField] GameObject popUpText;
    [SerializeField] GameObject darkPanel;
    [SerializeField] GameObject book;
    [SerializeField] Text hiraganaText;
    [SerializeField] Text formalText;
    [SerializeField] GameObject faseText;

    //バトルモードのみ
    [SerializeField] Text gameTimeText; 
    [SerializeField] GameObject winPanel;   
    [SerializeField] GameObject losePanel;   
    [SerializeField] GameObject battleModeUI;    
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject enemyObject;


    [SerializeField] AudioManager audioManager;

    /*デバッグ用*/
    [SerializeField] InputField fallInputField; //自由落下速度調整用の入力欄

    [SerializeField] private WordList wordList;
    [SerializeField] private Transform wordViewContent;
    [SerializeField] private GameObject wordViewElementPrefab;


    private void Update()
    {
        if(fallInputField != null)
        {
            /*デバッグ用*/
            var str = fallInputField.text;
            int num = 50;
            int.TryParse(str, out num);
            if (num == 0) num = 50;
            Block.fallSpeed = num / 1000f;
        }

    }

    //UIテキストの変更を行う
    public void textUpdate(TextKinds textKinds, float value)
    {
        switch (textKinds)
        {
            case TextKinds.Combo:
                if (value > 99) value = 99;
                comboText.text = value.ToString("00");
                break;
            case TextKinds.CountDown:
                countDownText.GetComponent<Text>().text = value.ToString("0");
                break;
            case TextKinds.Score:
                if (scoreText != null)
                    scoreText.text = value.ToString("0000000");
                break;
            case TextKinds.MaxCombo:
                if (maxComboText != null)
                {
                    if (value > 99) value = 99;
                    maxComboText.text = value.ToString("#0");
                }
                break;
        }
    }

    public void textUpdate(TextKinds textKinds, string value)
    {
        switch (textKinds) { 
            case TextKinds.Rank:
                rankText.text = value;
                break;
        }
    }

    //ゲームスタート時のカウントダウンの表示を行うコルーチン
    public IEnumerator showCountDown()
    {
        Text text = countDownText.GetComponent<Text>();
        Animator anim = countDownText.GetComponent<Animator>();

        darkPanel.SetActive(true);

        if(gameTimeText != null)
        {
            int second = Mathf.FloorToInt(GameController.gameTime);
            int min = second > 99 * 60 ? 99 : (second / 60);
            second = second > 99 * 60 ? 59 : second % 60;

            gameTimeText.text = "たいむ " + min.ToString("00")+ ":" + second.ToString("00");
        }
        audioManager.playSeOneShot(AudioKinds.SE_Countdown);
        text.text = "3";
        anim.SetTrigger("upstart");
        yield return new WaitForSeconds(1f);
        text.text = "2";
        anim.SetTrigger("upstart");
        yield return new WaitForSeconds(1f);
        text.text = "1";
        anim.SetTrigger("upstart");
        yield return new WaitForSeconds(1f);
        text.text = "Start";
        yield return new WaitForSeconds(1.2f);
        text.text = "";
        if (gameTimeText != null) gameTimeText.text = "";
        darkPanel.SetActive(false);


        yield break;
    }

    public IEnumerator showPopUp(string text="Finish",float time = 1f)
    {
        
        darkPanel.SetActive(true);
        popUpText.SetActive(true);
        popUpText.GetComponent<Text>().text = text;
        yield return new WaitForSeconds(time);
        darkPanel.SetActive(false);
        popUpText.SetActive(false);
        yield break;
    }

    //本を開く
    private IEnumerator openWordBook()
    {
        book.SetActive(true);
        yield break;
    }

    //本を閉じる
    public IEnumerator closeWordBook()
    {
        book.SetActive(false);
        yield break;
    }


    //本に文字を表示する
    public IEnumerator updateBook(string hiragana="", string formal="")
    {
        if (!book.activeSelf) yield return openWordBook();  //本を開く

        hiraganaText.text = hiragana;
        formalText.text = formal;

        yield break;
    }

    //設定をUIに反映
    public void configInit()
    {
        Debug.Log("ずん"+ Config.operateMode);
        //操作方法（タッチのみでないの場合）
        if (Config.operateMode != 1)
        {
            //ボタン配置
            if (Config.buttonLayout == 0)
            {
                buttonLayout_0.SetActive(true);
                buttonLayout_1.SetActive(false);
            }
            else
            {
                buttonLayout_0.SetActive(false);
                buttonLayout_1.SetActive(true);
            }
        }
        else
        {
            buttonLayout_0.SetActive(false);
            buttonLayout_1.SetActive(false);
        }

        //ボタンサイズ
        float size = 0.06f * (Config.buttonSize-1) + 0.76f;
        for (int i=0; i<buttons.Length; i++)
        {
            buttons[i].GetComponent<RectTransform>().localScale = new Vector3(size, size, 1);
        }
    }

    //Normalのワードビューに最新単語を追加する
    public void addWordView()
    {
        if(wordViewContent != null)
        {
            //軽量化のため消した文字は10個まで表示
            if(wordViewContent.childCount >= 10)
            {
                Destroy(wordViewContent.GetChild(wordViewContent.childCount - 1).gameObject);
            }
            var instance = Instantiate(wordViewElementPrefab, wordViewContent);
            instance.transform.GetChild(0).GetComponent<Text>().text = wordList.CollectList.Last().Hiragana;
            instance.transform.GetChild(1).GetComponent<Text>().text = wordList.CollectList.Last().Word;
            instance.transform.SetSiblingIndex(0);
        }
    }

    public IEnumerator showResultPanel(EndState endState, float time)
    {
        faseText.SetActive(false);
        battleModeUI.SetActive(false);
        playerObject.SetActive(false);
        
        if(endState == EndState.WIN)
        {
            enemyObject.SetActive(false);   //敵を消す
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
        
        yield return new WaitForSeconds(time);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        yield break;
    }
}
