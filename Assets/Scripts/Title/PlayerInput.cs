using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] GameObject optionPanel;
        [SerializeField] AudioManager audioManager;

        private bool canClick = true;   //Audioの再生終了街の時にボタンを押せなくする

        [SerializeField] Button adventureButton;
        [SerializeField] Button closeOptionPanelButton;
        [SerializeField] Scrollbar scrollbar;

        private void Update()
        {
            if (optionPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    scrollbar.Select();
                }

                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    closeOptionPanelButton.Select();
                }
            }
            
        }

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
                closeOptionPanelButton.Select();
            }
            
        }

        //クレジットボタンを押したとき
        public void pushCreditButton()
        {
            if (canClick)
            {
                audioManager.playSeOneShot(AudioKinds.SE_Enter);
                
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
            SceneChanger.changeTo(SceneType.Adventure);
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
