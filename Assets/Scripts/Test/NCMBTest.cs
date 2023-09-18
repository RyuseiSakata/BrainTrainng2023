using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

[System.Serializable]
public class RankingData2
{
    public string name;
    public int score;

    public RankingData2(string n = "名無しさん", int s = 0)
    {
        name = n;
        score = s;
    }
}

public class NCMBTest : MonoBehaviour
{
    

    public List<RankingData2> rankingList = new List<RankingData2>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Scoreクラスを検索するクエリの作成
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Score");

            //Scoreを降順で取得する
            query.OrderByDescending("score");

            //検索件数を設定
            query.Limit = 5;

            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                if (e != null)
                {
                    //エラー処理
                    Debug.Log("検索に失敗しました。エラーコード：" + e.ErrorCode);
                }
                else
                {
                    //成功時の処理
                    Debug.Log("保存に成功しました。objectId");
                    foreach (var obj in objList)
                    {
                        string n = System.Convert.ToString(obj["name"]);
                        int s = System.Convert.ToInt32(obj["score"]);
                        RankingData2 data = new RankingData2(n, s);
                        rankingList.Add(data);
                    }
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            NCMBObject obj = new NCMBObject("Score");

            obj["name"] = "らむん";
            obj["score"] = 3200;

            obj.SaveAsync((NCMBException e) => {
                if (e != null)
                {
                    //エラー処理
                    Debug.Log("保存に失敗しました。エラーコード：" + e.ErrorCode);
                }
                else
                {
                    //成功時の処理
                    Debug.Log("保存に成功しました。objectId" + obj.ObjectId);
                }
            });
        }
    }
}
