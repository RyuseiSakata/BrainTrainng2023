using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{

    public const int maxRow = 10; //�s��+1
    public const int maxCol = 7;    //��

    public const float StageHeight = 9f;  //�X�e�[�W����
    public const float StageWidth = StageHeight / ((maxRow - 1) / maxCol);  //�X�e�[�W��
    public const float BlockWidth = StageWidth / maxCol;  //�u���b�N�̕�
    public const float deltaX = -0.28f;

    public static bool isCaluculatedSum = false;    //�d�ݍ��v���v�Z�������̃t���O
    public static int sumProbability = 0;           //�d�ݍ��v
    public const string character = "�������������������������������������������������������ĂƂ����ÂłǂȂɂʂ˂̂͂Ђӂւق΂тԂׂڂς҂Ղ؂ۂ܂݂ނ߂��������������[";
    //public const string character = "���";
    //public static int[] probability = { 1, 1, 1 };
    //�e�����̏o���m���̏d��
    public static int[] probability = {
        8437,15173,13497,4794,5275,  //���s
        6967,6144,6991,2847,3838,  //���s
        3596,1642,1415,1501,1606,  //���s
        4368,8347,4819,3025,1721,  //���s
        1061,3584,1522,532,475,  //���s
        4249,6344,8873,2264,4284,  //���s
        2235,61,484,1094,2372,  //���s
        4399,2791,758,1671,3173,  //�ȍs
        1743,1180,1637,416,967,  //�͍s
        2250,1578,1891,749,1110,  //�΍s
        629,392,663,222,384,  //�ύs
        5722,4209,2156,3034,2920,  //�܍s
        4484,3483,5691,  //��s
        5035,6470,7452,3443,2497,  //��s
        3463,604,12994,  //��s
        4545,  //�[�s
    };


}
