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

        //�����L���O���擾
        public void updateRankingList()
        {
            rankingList.Clear();

            if (SceneChanger.getCurrentSceneName() == "NormalResult")
            {
                //Score�N���X����������N�G���̍쐬
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Normal");

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
            else if(SceneChanger.getCurrentSceneName() == "AdventureResult")
            {
                //Score�N���X����������N�G���̍쐬
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Adventure");

                //Score���~���Ŏ擾����
                query.OrderByAscending("time");

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
                            float s = (float)System.Convert.ToDouble(obj["time"]);
                            RankingData data = new RankingData(n, 0 ,s);
                            rankingList.Add(data);

                        }

                        isLoaded = true;
                    }
                });
            }
                

            
        }

        //�m�[�}�������L���O�Ƀf�[�^����������
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
            else
            {
                updateRankingList();
            }
           
        }

        //�A�h�x���`���[�����L���O�Ƀf�[�^����������
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
            else
            {
                updateRankingList();
            }
            
        }
    }
}