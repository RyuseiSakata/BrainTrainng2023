using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonKinds
{
    Down = 0,
    Left = 1,
    Right = 2,
    LeftTurn = 3,
    RightTurn = 4,
    None = 5,
}

/* ���[�U�̓��͂������N���X */
public class PlayerInput : MonoBehaviour
{
    [SerializeField] Stage stage;

    private long frameFromTapped = 0;  //�^�b�v���Ă��痣���܂ł̃t���[����
    private float preTapPositionX = 0f;  //�O�Ƀ^�b�v����X���W
    private float preTapPositionY = 0f;  //�O�Ƀ^�b�v����X���W

    private bool isDownButtonHold = false;  //���ɂ��낷�{�^����������Ă��邩�̃t���O
    private bool isLeftButtonHold = false; //�E�ړ��{�^����������Ă��邩�̃t���O
    private bool isRightButtonHold = false; //���ړ��{�^����������Ă��邩�̃t���O
    private bool isNothingPanelHold = false;    //�{�^���̂Ȃ��Ƃ�����^�b�`���Ă��邩

    private float moveLeftInterval = 0f; //�E�ړ�����Ԋu ���ɉ�����悤�ɂȂ�܂ł̎���
    private float moveRightInterval = 0f; //���ړ�����Ԋu ���ɉ�����悤�ɂȂ�܂ł̎���

    private void Start()
    {
        //Input.multiTouchEnabled = false;
    }
    private void Update()
    {
        if (stage.CanUserOperate)
        {
            //�L�[�{�[�h UI�{�^��
            if (Input.GetKey(KeyCode.S) || isDownButtonHold)
            {
                stage.fallBoost = 10f;
            }
            else if (!Input.GetKey(KeyCode.S) || !isDownButtonHold)
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
                if ((Input.GetKey(KeyCode.D) || isRightButtonHold) && moveRightInterval <= 0)
                {
                    stage.moveColumn(+1);
                    moveRightInterval = 0.1f;
                }

                if ((Input.GetKey(KeyCode.A) || isLeftButtonHold) && moveLeftInterval <= 0)
                {
                    stage.moveColumn(-1);
                    moveLeftInterval = 0.1f;
                }
            }

            // �{�^���ȊO�̏����^�b�` ���� �^�b�`����Ă��邩�`�F�b�N
            if (isNothingPanelHold && Input.touchCount > 0)
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
                        //1�{�Ń^�b�`
                        if(Input.touchCount == 1)
                        {
                            stage.fallBoost = 8f;  //�������x��������
                            frameFromTapped = -1;   //��]�h�~
                        }
                        else  //2�{�ȏ�Ń^�b�`
                        {
                            stage.fallBoost = 100f;  //�������x��������
                            frameFromTapped = -1;   //��]�h�~
                        }
                        
                    }
                }
            }
            //�^�b�v����Ă��Ȃ�
            else if (Input.touchCount <= 0)
            {
                isNothingPanelHold = false;
            }
        }

        
        if (moveLeftInterval > 0)
        {
            moveLeftInterval -= Time.deltaTime;
        }

        if(moveRightInterval > 0)
        {
            moveRightInterval -= Time.deltaTime;
        }
    }

    public void OnClickDown(int num)
    {
        ButtonKinds button = (ButtonKinds)Enum.ToObject(typeof(ButtonKinds), num);
        switch (button)
        {
            case ButtonKinds.Down:
                isDownButtonHold = true;
                break;
            case ButtonKinds.Left:
                isLeftButtonHold = true;
                break;
            case ButtonKinds.Right:
                isRightButtonHold = true;
                break;
            case ButtonKinds.LeftTurn:
                stage.rotateBlock(-90f);
                break;
            case ButtonKinds.RightTurn:
                stage.rotateBlock(+90f);
                break;
            case ButtonKinds.None:
                isNothingPanelHold = true;
                Debug.Log("True");
                break;
        }
    }

    public void PointerUp(int num)
    {
        ButtonKinds button = (ButtonKinds)Enum.ToObject(typeof(ButtonKinds), num);
        switch (button)
        {
            case ButtonKinds.Down:
                isDownButtonHold = false;
                break;
            case ButtonKinds.Left:
                isLeftButtonHold = false;
                break;
            case ButtonKinds.Right:
                isRightButtonHold = false;
                break;
        }
    }

    public void updateTapPosition()
    {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);    // �^�b�`���̎擾
            preTapPositionX = touch.position.x;  //�O�Ƀ^�b�v����X���W
            preTapPositionY = touch.position.y;  //�O�Ƀ^�b�v����Y���W
            isDownButtonHold = false;  //���ɂ��낷�{�^����������Ă��邩�̃t���O
            isNothingPanelHold = false;    //�{�^���̂Ȃ��Ƃ�����^�b�`���Ă��邩
}   
    }
}
