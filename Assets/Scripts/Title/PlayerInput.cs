using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class PlayerInput : MonoBehaviour
    {

        [SerializeField] GameObject optionPanel;
        [SerializeField] AudioManager audioManager;

        private bool canClick = true;   //Audio�̍Đ��I���X�̎��Ƀ{�^���������Ȃ�����

        [SerializeField] Button adventureButton;
        [SerializeField] Button closeOptionPanelButton;
        [SerializeField] Scrollbar scrollbar;

        [SerializeField] InputField nameInputField;

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

        private void Start()
        {
            GameController.playerName = string.Empty;
            nameInputField.text = GameController.playerName;
        }

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

        //�`���[�g���A���{�^�����������Ƃ�
        public void pushTutorialButton()
        {
            if (canClick)
            {
                canClick = false;
                StartCoroutine(actionTutorialButton());
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
                closeOptionPanelButton.Select();
            }
            
        }

        //�N���W�b�g�{�^�����������Ƃ�
        public void pushCreditButton()
        {
            if (canClick)
            {
                audioManager.playSeOneShot(AudioKinds.SE_Enter);
                
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
            SceneChanger.changeTo(SceneType.Adventure);
            yield break;
        }

        //�`���[�g���A���{�^�����������Ƃ��ɌĂ΂��R���[�`��
        public IEnumerator actionTutorialButton()
        {
            yield return audioManager.playSeOneShotWait(AudioKinds.SE_Enter);
            SceneChanger.changeTo(SceneType.Tutorial);
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

        public void endEditName()
        {
            if(nameInputField.text.Length >= 10)   nameInputField.text = nameInputField.text.Substring(0, 10);
            GameController.playerName = nameInputField.text;
        }
    }
}
