using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NMecabTest : MonoBehaviour
{
    [SerializeField] Text debugStateText;
    [SerializeField] Text text;
    void Start()
    {
        debugStateText.text = "State: before Check()";
        var list = (new Jage()).Check("‚©‚è‚ñ‚²‚¤");
        debugStateText.text = "State: after Check()";

        debugStateText.text = "State: before forEach";
        list.ForEach(e =>
        {
            text.text = text.text + "\n" + e;
            Debug.Log("TEST:" + e);
        });
        debugStateText.text = "State: after forEach";
    }
}
