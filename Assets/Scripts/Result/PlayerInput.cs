using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] AudioManager audioManager;
        private bool canClick = true;   //Audio�̍Đ��I���X�̎��Ƀ{�^���������Ȃ�����

        //�^�C�g���{�^�����������Ƃ�
        public void pushTitleButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionTitleButton());
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

        //�^�C�g���{�^�����������Ƃ��ɌĂ΂��R���[�`��
        public IEnumerator actionTitleButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Title);
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
