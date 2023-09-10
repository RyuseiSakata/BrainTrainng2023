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
    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;
    [SerializeField] Text countDownText;
    [SerializeField] GameObject popUpText;
    [SerializeField] GameObject darkPanel;
    [SerializeField] GameObject book;
    [SerializeField] Text hiraganaText;
    [SerializeField] Text formalText;

    /*�f�o�b�O�p*/
    [SerializeField] InputField fallInputField; //���R�������x�����p�̓��͗�


    private void Update()
    {
        if(fallInputField != null)
        {
            /*�f�o�b�O�p*/
            var str = fallInputField.text;
            int num = 50;
            int.TryParse(str, out num);
            if (num == 0) num = 50;
            Block.fallSpeed = num / 1000f;
        }

    }

    //UI�e�L�X�g�̕ύX���s��
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
                    scoreText.text = "�X�R�A�F"+value.ToString("0000");
                break;
        }
    }

    //�Q�[���X�^�[�g���̃J�E���g�_�E���̕\�����s���R���[�`��
    public IEnumerator showCountDown()
    {
        darkPanel.SetActive(true);
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

    //�{���J��
    private IEnumerator openWordBook()
    {
        book.SetActive(true);
        yield break;
    }

    //�{�����
    public IEnumerator closeWordBook()
    {
        book.SetActive(false);
        yield break;
    }


    //�{�ɕ�����\������
    public IEnumerator updateBook(string hiragana="", string formal="")
    {
        if (!book.activeSelf) yield return openWordBook();  //�{���J��

        hiraganaText.text = hiragana;
        formalText.text = formal;

        yield break;
    }


}
