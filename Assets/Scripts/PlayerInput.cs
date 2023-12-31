using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonKinds
{
    Down = 0,
    Left = 1,
    Right = 2,
    LeftTurn = 3,
    RightTurn = 4,
    None = 5,
}

/* ユーザの入力を扱うクラス */
public class PlayerInput : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Stage stage;

    private long frameFromTapped = 0;  //タップしてから離すまでのフレーム数
    private float preTapPositionX = 0f;  //前にタップしたX座標
    private float preTapPositionY = 0f;  //前にタップしたX座標

    private bool isDownButtonHold = false;  //下におろすボタンが押されているかのフラグ
    private bool isLeftButtonHold = false; //右移動ボタンが押されているかのフラグ
    private bool isRightButtonHold = false; //左移動ボタンが押されているかのフラグ
    private bool isNothingPanelHold = false;    //ボタンのないところをタッチしているか

    private float moveLeftInterval = 0f; //右移動する間隔 次に押せるようになるまでの時間
    private float moveRightInterval = 0f; //左移動する間隔 次に押せるようになるまでの時間

    private void Start()
    {
        //Input.multiTouchEnabled = false;
    }
    private void Update()
    {
        if (stage.CanUserOperate)
        {
            //キーボード UIボタン

            //下加速
            var onDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            if (onDown || isDownButtonHold)
            {
                stage.fallBoost = 14f;
            }
            else if (!onDown || !isDownButtonHold)
            {
                stage.fallBoost = 1f;
            }

            //回転
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stage.rotateBlock(+90f);
            }

            //右移動
            var onlyRight = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow));
            if ((onlyRight || isRightButtonHold) && moveRightInterval <= 0)
            {
                stage.moveColumn(+1);
                moveRightInterval = 0.2f;
            }

            //左移動
            var onlyLeft = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow));
            if (( onlyLeft || isLeftButtonHold) && moveLeftInterval <= 0)
            {
                stage.moveColumn(-1);
                moveLeftInterval = 0.2f;
            }
            

            // ボタンのみモードではない かつ ボタン以外の所をタッチ かつ タッチされているかチェック
            if (Config.operateMode != 0 && isNothingPanelHold && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);    // タッチ情報の取得
                frameFromTapped++;  //フレーム数の更新

                //タッチした瞬間
                if (touch.phase == TouchPhase.Began)
                {
                    frameFromTapped = 0;
                    preTapPositionX = touch.position.x;
                    preTapPositionY = touch.position.y;

                }

                //離した瞬間
                if (touch.phase == TouchPhase.Ended)
                {
                    //タップしてから0.03f〜0.75f秒以内なら
                    if (0.03f < frameFromTapped * Time.deltaTime && frameFromTapped * Time.deltaTime < 0.75f)
                    {
                        stage.rotateBlock(90f);
                    }

                    preTapPositionY = touch.position.y;    //基準の更新
                    stage.fallBoost = 1f;  //落下速度を元に戻す
                }

                //タッチして動いている間
                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.position.x - preTapPositionX;
                    float deltaY = touch.position.y - preTapPositionY;
                    frameFromTapped = 0;

                    if (Mathf.Abs(deltaX) > 100)
                    {
                        stage.moveColumn(0 <= deltaX ? +1 : -1);  //ブロックの移動
                        preTapPositionX = touch.position.x;    //基準の更新
                        frameFromTapped = -1;   //回転防止
                    }
                    if (Mathf.Abs(deltaY) > 100)
                    {
                        //1本でタッチ
                        if(Input.touchCount == 1)
                        {
                            stage.fallBoost = 12f;  //落下速度をあげる
                            frameFromTapped = -1;   //回転防止
                        }
                        else  //2本以上でタッチ
                        {
                            stage.fallBoost = 100f;  //落下速度をあげる
                            frameFromTapped = -1;   //回転防止
                        }
                        
                    }
                }
            }
            //タップされていない
            else if (Input.touchCount <= 0)
            {
                isNothingPanelHold = false;
            }
        }

        //左右移動の間隔
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
                isRightButtonHold = false;
                break;
            case ButtonKinds.Right:
                isRightButtonHold = true;
                isLeftButtonHold = false;
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
            Touch touch = Input.GetTouch(0);    // タッチ情報の取得
            preTapPositionX = touch.position.x;  //前にタップしたX座標
            preTapPositionY = touch.position.y;  //前にタップしたY座標
            isDownButtonHold = false;  //下におろすボタンが押されているかのフラグ
            isNothingPanelHold = false;    //ボタンのないところをタッチしているか
}   
    }
}
