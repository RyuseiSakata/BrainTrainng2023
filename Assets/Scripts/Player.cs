using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Stage stage;

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            stage.fallBoost = 5f;
        }
        else
        {
            stage.fallBoost = 1f;
        }

        //shift‚ª‰Ÿ‚³‚ê‚Ä‚¢‚é‚Æ‚«
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        { 
            if (Input.GetKeyDown(KeyCode.D))
            {
                stage.rotateBlock(+1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                stage.rotateBlock(-1);
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("moveR");
                stage.moveColumn(+1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("moveL");
                stage.moveColumn(-1);
            }
        }

        
    }
}
