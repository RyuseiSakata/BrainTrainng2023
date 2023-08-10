using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushButtonTest : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button okButton;


    public void PushedOkButton()
    {
        SearchTest.CalculteScore(inputField.text);
    }
}
