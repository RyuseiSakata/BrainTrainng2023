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

    //UIテキストの変更を行う
    public void textUpdate(TextKinds textKinds, int comboNum)
    {
        switch (textKinds)
        {
            case TextKinds.Combo:
                comboText.text = "コンボ数：" + comboNum.ToString("000");
                break;
        }
    }
}
