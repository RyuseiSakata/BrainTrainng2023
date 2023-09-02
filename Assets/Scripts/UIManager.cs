using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextKinds
{
    Combo,
    CountDown,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] Text comboText;
    [SerializeField] Text countDownText;
    [SerializeField] GameObject finishText;
    [SerializeField] GameObject darkPanel;

    /*デバッグ用*/
    [SerializeField] InputField fallInputField; //自由落下速度調整用の入力欄


    private void Update()
    {
        if(fallInputField != null)
        {
            /*デバッグ用*/
            var str = fallInputField.text;
            int num = 150;
            int.TryParse(str, out num);
            if (num == 0) num = 150;
            Block.fallSpeed = num / 1000f;
        }

    }

    //UIテキストの変更を行う
    public void textUpdate(TextKinds textKinds, float value)
    {
        switch (textKinds)
        {
            case TextKinds.Combo:
                comboText.text = "コンボ数：" + value.ToString("000");
                break;
            case TextKinds.CountDown:
                countDownText.text = value.ToString("0");
                break;
        }
    }

    //ゲームスタート時のカウントダウンの表示を行うコルーチン
    public IEnumerator showCountDown()
    {
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.text = "Start";
        yield return new WaitForSeconds(0.8f);
        countDownText.text = "";
        darkPanel.SetActive(false);

        yield break;
    }

    public IEnumerator showFinish()
    {
        darkPanel.SetActive(true);
        finishText.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield break;
    }
}
