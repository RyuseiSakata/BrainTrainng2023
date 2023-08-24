using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Stage stage;

    private long frameFromTapped = 0;  //タップしてから離すまでのフレーム数
    private float preTapPositionX = 0f;  //前にタップしたX座標
    private float preTapPositionY = 0f;  //前にタップしたX座標

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

            debugText.text = (frameFromTapped * Time.deltaTime).ToString();
        }
    }
}
