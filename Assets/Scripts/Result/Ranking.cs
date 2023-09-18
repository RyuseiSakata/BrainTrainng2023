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

        public RankingData(string n = "����������", int s = 0)
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

            //Score�N���X����������N�G���̍쐬
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Score");

            //Score���~���Ŏ擾����
            query.OrderByDescending("score");

            //����������ݒ�
            query.Limit = 5;

            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                if (e != null)
                {
                    //�G���[����
                    Debug.Log("�����Ɏ��s���܂����B�G���[�R�[�h�F" + e.ErrorCode);
                }
                else
                {
                    //�������̏���
                    Debug.Log("�ۑ��ɐ������܂����BobjectId");
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

        public void writeRankingData(string name = "����������", int score = 0)
        {
            NCMBObject obj = new NCMBObject("Score");

            obj["name"] = name;
            obj["score"] = score;

            obj.SaveAsync((NCMBException e) => {
                if (e != null)
                {
                    //�G���[����
                    Debug.Log("�ۑ��Ɏ��s���܂����B�G���[�R�[�h�F" + e.ErrorCode);
                }
                else
                {
                    //�������̏���
                    Debug.Log("�ۑ��ɐ������܂����BobjectId" + obj.ObjectId);
                    updateRankingList();
                }
            });

            
        }
    }

}