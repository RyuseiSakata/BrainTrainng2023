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

        if (Input.GetKeyDown(KeyCode.D))
        {
            stage.MoveColumn(+1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            stage.MoveColumn(-1);
        }
    }
}
