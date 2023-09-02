using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Playing,
    GameOver,
}


public class GameController : MonoBehaviour
{
    [SerializeField] Stage stage;
    private int numberOfTurns = 0;  //�^�[����
    private GameState gameState = GameState.Playing;    //�Q�[���̏��

    //�^�[�����̃v���p�e�B
    public int NumberOfTurns
    {
        get => numberOfTurns;
        set { numberOfTurns = value; }
    }

    private void Start()
    {
        StartCoroutine("mainLoop");
    }

    private IEnumerator mainLoop()
    {
        while (gameState == GameState.Playing) {
            Debug.Log("Start");
            yield return stage.fall();
            NumberOfTurns++;    //�^�[�����𑝉�
            Debug.Log("turn:"+numberOfTurns);
        }

        yield return new WaitForSeconds(1f);

        SceneChanger.changeTo(SceneType.Result);
        yield break;
    }

    public void gameOver()
    {
        Debug.Log("game over");
        gameState = GameState.GameOver;
    }
}

