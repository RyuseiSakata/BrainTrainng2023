using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result
{
    public class PlayerInput : MonoBehaviour
    {
        //タイトルボタンを押したとき
        public void pushTitleButton()
        {
            SceneChanger.changeTo(SceneType.Title);
        }

        //終了ボタンを押したとき
        public void pushQuitButton()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
            #else
                Application.Quit();//ゲームプレイ終了
            #endif
        }
    }
}
