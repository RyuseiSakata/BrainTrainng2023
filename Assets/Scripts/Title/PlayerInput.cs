using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{ 
    public class PlayerInput : MonoBehaviour
    {
        //通常モードボタンを押したとき
        public void pushNormalButton()
        {
            SceneChanger.changeTo(SceneType.Normal);
        }

        //バトルモードボタンを押したとき
        public void pushBattleButton()
        {
            SceneChanger.changeTo(SceneType.Battle);
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
