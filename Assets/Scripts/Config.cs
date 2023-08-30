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

    public static int sumProbability = 0;
    public const string character = "�������������������������������������������������������ĂƂ����ÂłǂȂɂʂ˂̂͂Ђӂւق΂тԂׂڂς҂Ղ؂ۂ܂݂ނ߂��������������[";
    //public const string character = "���";
    //public static int[] probability = { 1, 1, 1 };
    //�e�����̏o���m���̏d��
    public static int[] probability = {
        348,1454,1463,299,237,  //���s
        612,696,897,328,468,  //���s
        346,229,218,166,226,  //���s
        288,816,568,268,234,  //���s
        163,423,154,95,67,  //���s
        469,452,1072,268,593,  //���s
        270,2,23,166,353,  //���s
        301,257,83,206,265,  //�ȍs
        168,139,235,109,148,  //�͍s
        318,273,252,161,209,  //�΍s
        178,88,191,63,83,  //�ύs
        374,407,262,268,258,  //�܍s
        430,412,606,  //��s
        510,680,469,301,299,  //��s
        220,0,1097,  //��s
        1089,  //�[�s
    };


}
