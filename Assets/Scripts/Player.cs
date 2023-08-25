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
}

public class Player : MonoBehaviour
{
    [SerializeField] Stage stage;

    private long frameFromTapped = 0;  //タップしてから離すまでのフレーム数
    private float preTapPositionX = 0f;  //前にタップしたX座標
    private float preTapPositionY = 0f;  //前にタップしたX座標

    public bool isDownButtonHold = false;  //下におろすボタンが押されているかのフラグ

    [SerializeField] Text debugText;
    [SerializeField] Text debugText2;

    private void Update()
    {
        //キーボード UIボタン
        if (Input.GetKey(KeyCode.S) || isDownButtonHold)
        {
            stage.fallBoost = 10f;
        }
        else if(!Input.GetKey(KeyCode.S) || !isDownButtonHold)
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

        // タッチされているかチェック
        if (Input.touchCount > 0)
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
                //タップしてから0.03f～0.75f秒以内なら
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
                    stage.fallBoost = 8f;  //落下速度をあげる
                    frameFromTapped = -1;   //回転防止
                }
            }

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
                stage.moveColumn(-1);
                break;
            case ButtonKinds.Right:
                stage.moveColumn(+1);
                break;
            case ButtonKinds.LeftTurn:
                stage.rotateBlock(-90f);
                break;
            case ButtonKinds.RightTurn:
                stage.rotateBlock(+90f);
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
                Debug.Log("A");
                break;
        }
    }
}
