using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Stage stage;

    private long frameFromTapped = 0;  //�^�b�v���Ă��痣���܂ł̃t���[����
    private float preTapPositionX = 0f;  //�O�Ƀ^�b�v����X���W
    private float preTapPositionY = 0f;  //�O�Ƀ^�b�v����X���W

    [SerializeField] Text debugText;
    [SerializeField] Text debugText2;

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            stage.fallBoost = 10f;
        }
        else
        {
            stage.fallBoost = 1f;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                stage.rotateBlock(+90f);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                stage.rotateBlock(-90f);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                stage.moveColumn(+1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                stage.moveColumn(-1);
            }
        }

        // �^�b�`����Ă��邩�`�F�b�N
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);    // �^�b�`���̎擾
            frameFromTapped++;  //�t���[�����̍X�V

            //�^�b�`�����u��
            if (touch.phase == TouchPhase.Began)
            {
                frameFromTapped = 0;
                preTapPositionX = touch.position.x;
                preTapPositionY = touch.position.y;

            }

            //�������u��
            if (touch.phase == TouchPhase.Ended)
            {
                //�^�b�v���Ă���0.03f�`0.75f�b�ȓ��Ȃ�
                if (0.03f < frameFromTapped * Time.deltaTime && frameFromTapped * Time.deltaTime < 0.75f)
                {
                    stage.rotateBlock(90f);
                }

                preTapPositionY = touch.position.y;    //��̍X�V
                stage.fallBoost = 1f;  //�������x�����ɖ߂�
            }

            //�^�b�`���ē����Ă����
            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.position.x - preTapPositionX;
                float deltaY = touch.position.y - preTapPositionY;
                frameFromTapped = 0;

                if (Mathf.Abs(deltaX) > 100)
                {
                    stage.moveColumn(0 <= deltaX ? +1 : -1);  //�u���b�N�̈ړ�
                    preTapPositionX = touch.position.x;    //��̍X�V
                    frameFromTapped = -1;   //��]�h�~
                }
                if (Mathf.Abs(deltaY) > 100)
                {
                    stage.fallBoost = 8f;  //�������x��������
                    frameFromTapped = -1;   //��]�h�~
                }
            }

            debugText.text = (frameFromTapped * Time.deltaTime).ToString();
        }
    }
}
