using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Normal,
    Adventure,
    NormalResult,
    AdventureResult,
    Tutorial,
}

public class SceneChanger : MonoBehaviour
{
    //�V�[���̕ύX���s�����\�b�h
    public static void changeTo(SceneType type)
    {
        switch (type)
        {
            case SceneType.Title:
                SceneManager.LoadScene("TitleScene");
                break;
            case SceneType.Normal:
                SceneManager.LoadScene("MainScene");
                break;
            case SceneType.Adventure:
                SceneManager.LoadScene("BattleScene");
                break;
            case SceneType.NormalResult:
                SceneManager.LoadScene("NormalResult");
                break;
            case SceneType.AdventureResult:
                SceneManager.LoadScene("AdventureResult");
                break;
            case SceneType.Tutorial:
                SceneManager.LoadScene("TutorialScene");
                break;
            default:
                Debug.Log("SceneChanger:����������܂���");
                break;
        }
    }

    public static string getCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

}
