using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yusha : MonoBehaviour {

    // 再生アニメーションのResourcesフォルダ内のサブパス
    [SerializeField]
    public Object[] AnimationList;
    int num;

    // 再生アニメーション指定用 
    private enum AnimationPattern : int
    {
        Wait = 33,      // 待機
        Attack = 1,     // 攻撃 
        Run = 24,       // 走り 
        Count
    }

    // キャラクター管理用 
    private GameObject m_goCharacter = null;
    private GameObject m_goCharPos = null;
    private Vector3 m_vecCharacterPos;      // キャラクター位置 
    private Vector3 m_vecCharacterScale;    // キャラクタースケール 
    public GameObject gameObject;

    // 処理ステップ用 
    private enum Step : int
    {
        Init = 0,   // 初期化 
        Title,      // タイトル 
        Wait,       // 待機 
        Move,       // 移動 
        Attack,     // 攻撃
        End
    }

    // 処理ステップ管理用 
    private Step m_Step = Step.Init;

    // 汎用
    // いろいろ使いまわす用変数
    private int m_Count = 0;
    private bool m_SW = true;

    // Use this for initialization
    void Start () {

        // キャラクターパラメータ関連を設定 

        // 座標設定 
        m_vecCharacterPos.x = 0.0f;
        m_vecCharacterPos.y = -240.0f;
        m_vecCharacterPos.z = 0.0f;

        // スケール設定 
        m_vecCharacterScale.x = 0.5f;
        m_vecCharacterScale.y = 0.5f;
        m_vecCharacterScale.z = 1.0f;
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Z) == true){
            StartCoroutine("AStart");
            gameObject.SetActive(false);
        }
       
    }

    IEnumerator AStart(){
        Destroy( GameObject.Find("Comipo"));
        AnimationStart();
        AnimationChange(AnimationPattern.Attack);
        num++;
        m_goCharacter = null;
        
        yield return new WaitForSeconds(1f);
        Destroy( GameObject.Find("Comipo"));
        AnimationStart();
        AnimationChange(AnimationPattern.Attack);
        
        num++;
        m_goCharacter = null;
        
       

    }
    // アニメーション開始 
    private void AnimationStart()
    {
        Object resourceObject;
        Script_SpriteStudio6_Root scriptRoot = null;    // SpriteStudio Anime を操作するためのクラス
        int listLength = AnimationList.Length;

        // すでにアニメーション生成済 or リソース設定無い場合はreturn
        if (m_goCharacter != null || listLength<1)
            return;

        // 再生するリソース名をリストから取得して再生する
        if(num%2==0){
            resourceObject = AnimationList[0];
            Debug.Log("1");
        }
        else{
           resourceObject = AnimationList[1];
            Debug.Log("2");
        }
        if (resourceObject != null)
        {
            // アニメーションを実体化
            m_goCharacter = Instantiate(resourceObject, Vector3.zero, Quaternion.identity) as GameObject;
            if (m_goCharacter != null)
            {
                scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(m_goCharacter);
                if (scriptRoot != null)
                {
                    // 座標設定するためのGameObject作成
                    m_goCharPos = new GameObject();
                    if (m_goCharPos == null)
                    {
                        // 作成できないケース対応 
                        Destroy(m_goCharacter);
                        m_goCharacter = null;
                    }
                    else
                    {
                        // Object名変更 
                        m_goCharPos.name = "Comipo";

                        // 座標設定 
                        m_goCharacter.transform.parent = m_goCharPos.transform;

                        // 自分の子に移動して座標を設定
                        m_goCharPos.transform.parent = this.transform;
                        m_goCharPos.transform.localPosition = m_vecCharacterPos;
                        m_goCharPos.transform.localRotation = Quaternion.identity;
                        m_goCharPos.transform.localScale = m_vecCharacterScale;

                        //アニメーション再生
                        AnimationChange(AnimationPattern.Wait);
                    }
                }
            }
        }
    }

    // アニメーション 再生/変更 
    private void AnimationChange(AnimationPattern pattern)
    {
        Script_SpriteStudio6_Root scriptRoot = null;    // SpriteStudio Anime を操作するためのクラス
        int iTimesPlaey = 0;

        if (m_goCharacter == null)
            return;

        scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(m_goCharacter);
        if (scriptRoot != null)
        {
            switch (pattern)
            {
                case AnimationPattern.Wait:
                    iTimesPlaey = 0;    // ループ再生 
                    break;
                case AnimationPattern.Attack:
                    iTimesPlaey = 1;    // 1回だけ再生 
                    break;
                case AnimationPattern.Run:
                    iTimesPlaey = 0;    // ループ再生 
                    break;
                default:
                    break;
            }
            scriptRoot.AnimationPlay(-1, (int)pattern, iTimesPlaey);
        }
    }

    // アニメーションが再生中か停止中(エラー含)か取得します
    private bool IsAnimationPlay()
    {
        bool ret = false;

        Script_SpriteStudio6_Root scriptRoot = null;    // SpriteStudio Anime を操作するためのクラス

        if (m_goCharacter != null)
        {
            scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(m_goCharacter);
            if (scriptRoot != null)
            {
                // 再生回数を取得して、プレイ終了かを判断します
                int Remain = scriptRoot.PlayTimesGetRemain(0);
                if (Remain >= 0)
                    ret = true;
            }
        }

        return ret;
    }

}