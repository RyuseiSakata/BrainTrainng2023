using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] GameObject optionPanel;
        [SerializeField] AudioManager audioManager;

        private bool canClick = true;   //Audio�̍Đ��I���X�̎��Ƀ{�^���������Ȃ�����

        //�ʏ탂�[�h�{�^�����������Ƃ�
        public void pushNormalButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionNormalButton());
            }
            
        }

        //�o�g�����[�h�{�^�����������Ƃ�
        public void pushBattleButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionBattleButton());
            }
        }

        //�I���{�^�����������Ƃ�
        public void pushQuitButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionQuitButton());
            }
        }

        //�I�v�V�����{�^�����������Ƃ�
        public void pushOptionButton()
        {
            if (canClick)
            {
                audioManager.playSeOneShot(AudioKinds.SE_Enter);
                optionPanel.SetActive(true);
            }
            
        }



        //�ʏ탂�[�h�{�^�����������Ƃ��ɌĂ΂��R���[�`��
        public IEnumerator actionNormalButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Normal);
            yield break;
        }

        //�o�g�����[�h�{�^�����������Ƃ��ɌĂ΂��R���[�`��
        public IEnumerator actionBattleButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Battle);
            yield break;
        }

        //�I���{�^�����������Ƃ��ɌĂ΂��R���[�`��
        public IEnumerator actionQuitButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
            #else
                Application.Quit();//�Q�[���v���C�I��
            #endif

            yield break;
        }
    }
}
