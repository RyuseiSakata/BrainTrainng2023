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
    }
}
