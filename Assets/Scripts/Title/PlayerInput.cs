using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] GameObject optionPanel;
        [SerializeField] AudioManager audioManager;

        private bool canClick = true;   //Audioの再生終了街の時にボタンを押せなくする

        //通常モードボタンを押したとき
        public void pushNormalButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionNormalButton());
            }
            
        }

        //バトルモードボタンを押したとき
        public void pushBattleButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionBattleButton());
            }
        }

        //終了ボタンを押したとき
        public void pushQuitButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionQuitButton());
            }
        }

        //オプションボタンを押したとき
        public void pushOptionButton()
        {
            if (canClick)
            {
                audioManager.playSeOneShot(AudioKinds.SE_Enter);
                optionPanel.SetActive(true);
            }
            
        }



        //通常モードボタンを押したときに呼ばれるコルーチン
        public IEnumerator actionNormalButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Normal);
            yield break;
        }

        //バトルモードボタンを押したときに呼ばれるコルーチン
        public IEnumerator actionBattleButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Battle);
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
