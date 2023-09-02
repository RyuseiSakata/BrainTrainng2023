using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Normal,
    Battle,
    Result,
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
            case SceneType.Battle:
                SceneManager.LoadScene("BattleScene");
                break;
            case SceneType.Result:
                SceneManager.LoadScene("ResultScene");
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
