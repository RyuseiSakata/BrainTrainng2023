using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] AudioManager audioManager;

        //タイトルボタンを押したとき
        public void pushTitleButton()
        {
            StartCoroutine(actionTitleButton());
        }

        //終了ボタンを押したとき
        public void pushQuitButton()
        {
            StartCoroutine(actionQuitButton());
        }

        //タイトルボタンを押したときに呼ばれるコルーチン
        public IEnumerator actionTitleButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Title);
            yield break;
        }

        //終了ボタンを押したときに呼ばれるコルーチン
        public IEnumerator actionQuitButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
            #else
                Application.Quit();//ゲームプレイ終了
            #endif

            yield break;
        }
    }
}
