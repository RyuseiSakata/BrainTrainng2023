using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const int maxRow = 10; //�s��+1
    public const int maxCol = 7;    //��

    public const float StageHeight = 9f;  //�X�e�[�W����
    public const float StageWidth = StageHeight / ((maxRow-1)/maxCol);  //�X�e�[�W��
    public const float BlockWidth = StageWidth / maxCol;  //�u���b�N�̕�
    public const float deltaX = -0.28f;

    public static int sumProbability = 0;
    public const string character = "�������������������������������������������������������ĂƂ����ÂłǂȂɂʂ˂̂͂Ђӂւق΂тԂׂڂς҂Ղ؂ۂ܂݂ނ߂��������������";
    //public const string character = "���";
    //public static int[] probability = { 1, 1, 1 };
    //�e�����̏o���m���̏d��
    public static int[] probability = { 
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //���s
        1, 1, 1, 1, 1,  //�ȍs
        1, 1, 1, 1, 1,  //�͍s
        1, 1, 1, 1, 1,  //�΍s
        1, 1, 1, 1, 1,  //�ύs
        1, 1, 1, 1, 1,  //�܍s
        1, 1, 1,        //��s
        1, 1, 1, 1, 1,  //��s
        1, 1, 1         //��s 
    };
    

}
