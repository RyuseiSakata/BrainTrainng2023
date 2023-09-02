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

    /*�f�o�b�O�p*/
    [SerializeField] InputField fallInputField; //���R�������x�����p�̓��͗�


    private void Update()
    {
        if(fallInputField != null)
        {
            /*�f�o�b�O�p*/
            var str = fallInputField.text;
            int num = 150;
            int.TryParse(str, out num);
            if (num == 0) num = 150;
            Block.fallSpeed = num / 1000f;
        }

    }

    //UI�e�L�X�g�̕ύX���s��
    public void textUpdate(TextKinds textKinds, float value)
    {
        switch (textKinds)
        {
            case TextKinds.Combo:
                comboText.text = "�R���{���F" + value.ToString("000");
                break;
            case TextKinds.CountDown:
                countDownText.text = value.ToString("0");
                break;
        }
    }

    //�Q�[���X�^�[�g���̃J�E���g�_�E���̕\�����s���R���[�`��
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
