/*
    �z�� vs List  �Œ�T�C�Y�̏ꍇ�͔z��̕����킸���ɃA�N�Z�X�͑��� ->�z����̗p�i�����z��Ɂj
    Dictionary(HashTable) vs List  �T�����x��O(1)vsO(n)��Dictionary�̕������� -> Dictionary���̗p�i�����̍쐬�Ɂj
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryTest
{

    private static Dictionary<string, string>[] vocabularyArray = new Dictionary<string, string>[]
    {
        null,
        //1����
        new Dictionary<string, string>{
        
        },
        //2����
        new Dictionary<string, string>{
            { "����", "��,��" },
            { "����", "��" }
        },
        //3����
        new Dictionary<string, string>{

        },
        //4����
        new Dictionary<string, string>{

        },
        //5����
        new Dictionary<string, string>{

        },
        //6����
        new Dictionary<string, string>{

        },
    };


    public static string[] retWords(string str)
    {
        int len = str.Length;   //������

        //�L�[���܂ނ��m�F
        if (vocabularyArray[len].ContainsKey(str))
        {
            return vocabularyArray[len][str].Split(',');  //�f�[�^���擾��,�R���}����������������̔z���Ԃ� 
        }
        else
        {
            return null;
        }
    }
}
