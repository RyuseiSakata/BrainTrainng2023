using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result
{
    public class PlayerInput : MonoBehaviour
    {
        //�^�C�g���{�^�����������Ƃ�
        public void pushTitleButton()
        {
            SceneChanger.changeTo(SceneType.Title);
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
