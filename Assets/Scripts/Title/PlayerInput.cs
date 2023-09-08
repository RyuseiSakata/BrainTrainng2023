using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{ 
    public class PlayerInput : MonoBehaviour
    {
        //�ʏ탂�[�h�{�^�����������Ƃ�
        public void pushNormalButton()
        {
            SceneChanger.changeTo(SceneType.Normal);
        }

        //�o�g�����[�h�{�^�����������Ƃ�
        public void pushBattleButton()
        {
            SceneChanger.changeTo(SceneType.Battle);
        }

        //�I���{�^�����������Ƃ�
        public void pushQuitButton()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
            #else
                Application.Quit();//�Q�[���v���C�I��
            #endif
        }
    }
}
