using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextKinds
{
    Combo,
    CountDown,
    Score,
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
    [SerializeField] Text countDownText;
    [SerializeField] GameObject popUpText;
    [SerializeField] GameObject darkPanel;
    [SerializeField] GameObject book;
    [SerializeField] Text hiraganaText;
    [SerializeField] Text formalText;
    [SerializeField] Text gameTimeText; //バトルモードのみ

    [SerializeField] AudioManager audioManager;

    /*デバッグ用*/
    [SerializeField] InputField fallInputField; //自由落下速度調整用の入力欄


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
                countDownText.text = value.ToString("0");
                break;
            case TextKinds.Score:
                if(scoreText!=null)
                    scoreText.text = value.ToString("000000");
                break;
        }
    }

    //ゲームスタート時のカウントダウンの表示を行うコルーチン
    public IEnumerator showCountDown()
    {
        darkPanel.SetActive(true);

        if(gameTimeText != null)
        {
            int second = Mathf.FloorToInt(GameController.gameTime);
            int min = second > 99 * 60 ? 99 : (second / 60);
            second = second % 60;

            gameTimeText.text = "たいむ " + min.ToString("00")+ ":" + second.ToString("00");
        }
        audioManager.playSeOneShot(AudioKinds.SE_Countdown);
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.text = "Start";
        yield return new WaitForSeconds(1.2f);
        countDownText.text = "";
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
}
