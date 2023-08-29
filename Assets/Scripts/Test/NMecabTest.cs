using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NMecabTest : MonoBehaviour
{
    [SerializeField] string checkString;
    [SerializeField] List<string> collectList = new List<string>();
    [SerializeField] Text debugStateText;
    [SerializeField] Text text;
    void Start()
    {
        debugStateText.text = "State: before Check()";
        var list = (new Jage()).Check(checkString);
        debugStateText.text = "State: after Check()";

        debugStateText.text = "State: before forEach";
        list.ForEach(e =>
        {
            text.text = text.text + "\n" + e;
            collectList.Add(e);
        });
        debugStateText.text = "State: after forEach";
    }
}
