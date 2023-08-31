using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextKinds
{
    Combo,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] Text comboText;

    //UI�e�L�X�g�̕ύX���s��
    public void textUpdate(TextKinds textKinds, int comboNum)
    {
        switch (textKinds)
        {
            case TextKinds.Combo:
                comboText.text = "�R���{���F" + comboNum.ToString("000");
                break;
        }
    }
}
