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

}
