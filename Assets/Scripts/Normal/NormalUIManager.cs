using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Normal
{
    public enum UIKinds
    {
        Score,
    }
    public class NormalUIManager : MonoBehaviour
    {
        [SerializeField] Text scoreText;

        //UIテキストの変更を行う
        public void uiUpdate(UIKinds uiKinds, float value)
        {
            switch (uiKinds)
            {
                case UIKinds.Score:
                    scoreText.text = value.ToString("0000");
                    break;

            }
        }
    }
}

