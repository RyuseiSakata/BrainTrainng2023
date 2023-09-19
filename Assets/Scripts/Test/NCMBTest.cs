using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

[System.Serializable]
public class RankingData2
{
    public string name;
    public int score;

    public RankingData2(string n = "����������", int s = 0)
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
                        RankingData2 data = new RankingData2(n, s);
                        rankingList.Add(data);
                    }
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            NCMBObject obj = new NCMBObject("Score");

            obj["name"] = "��ނ�";
            obj["score"] = 3200;

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
                }
            });
        }
    }
}
