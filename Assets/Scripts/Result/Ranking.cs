using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

namespace Result
{

    [System.Serializable]
    public class RankingData
    {
        public string name;
        public int score;

        public RankingData(string n = "名無しさん", int s = 0)
        {
            name = n;
            score = s;
        }
    }

    public class Ranking : MonoBehaviour
    {
        public static List<RankingData> rankingList = new List<RankingData>();
        public static bool isLoaded = false;

        public void updateRankingList()
        {
            rankingList.Clear();

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
                        RankingData data = new RankingData(n, s);
                        rankingList.Add(data);
                        
                    }
                    
                    isLoaded = true;
                }
            });

            
        }

        public void writeRankingData(string name = "名無しさん", int score = 0)
        {
            NCMBObject obj = new NCMBObject("Score");

            obj["name"] = name;
            obj["score"] = score;

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
                    updateRankingList();
                }
            });

            
        }
    }

}