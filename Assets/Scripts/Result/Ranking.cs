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
        public float time;

        public RankingData(string n = "", int s = 0, float t = 0)
        {
            name = n;
            score = s;
            time = t;
        }
    }

    public class Ranking : MonoBehaviour
    {
        public static List<RankingData> rankingList = new List<RankingData>();
        public static bool isLoaded = false;

        //ランキングを取得
        public void updateRankingList()
        {
            rankingList.Clear();

            if (SceneChanger.getCurrentSceneName() == "NormalResult")
            {
                //Scoreクラスを検索するクエリの作成
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Normal");

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
            else if(SceneChanger.getCurrentSceneName() == "AdventureResult")
            {
                //Scoreクラスを検索するクエリの作成
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Adventure");

                //Scoreを降順で取得する
                query.OrderByAscending("time");

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
                            float s = (float)System.Convert.ToDouble(obj["time"]);
                            RankingData data = new RankingData(n, 0 ,s);
                            rankingList.Add(data);

                        }

                        isLoaded = true;
                    }
                });
            }
                

            
        }

        //ノーマルランキングにデータを書き込む
        public void writeNormalRankingData(string name = "", int score = 0)
        {
            if (name != string.Empty)
            {
                NCMBObject obj = new NCMBObject("Normal");

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
            else
            {
                updateRankingList();
            }
           
        }

        //アドベンチャーランキングにデータを書き込む
        public void writeAdventureRanking(string name = "", float time = 0)
        {
            if (name != string.Empty && GameController.faseCount == 3)
            {
                NCMBObject obj = new NCMBObject("Adventure");

                obj["name"] = name;
                obj["time"] = time;

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
            else
            {
                updateRankingList();
            }
            
        }
    }
}