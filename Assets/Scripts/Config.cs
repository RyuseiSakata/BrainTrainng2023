public static class Config
{

    public const int maxRow = 10; //�s�� Stage��Scale��maxRow*0.9�݂����ɂ��邱��    �C���f�b�N�X��1����
    public const int maxCol = 7;    //��    Stage��Scale��maxRow*0.9�݂����ɂ��邱��

    public const float StageHeight = 9f;  //�X�e�[�W����
    public const float StageWidth = StageHeight / ((maxRow - 1) / maxCol);  //�X�e�[�W��
    public const float BlockWidth = StageWidth / maxCol;  //�u���b�N�̕�
    public const float deltaX = -0.2f;

    public static bool isCaluculatedSum = false;    //�d�ݍ��v���v�Z�������̃t���O
    public static int sumProbability = 0;           //�d�ݍ��v
    public const string character = "�������������������������������������������������������ĂƂ����ÂłǂȂɂʂ˂̂͂Ђӂւق΂тԂׂڂς҂Ղ؂ۂ܂݂ނ߂��������������[";
    //�e�����̏o���m���̏d��
    public static int[] probability = {
        10841,49126,62574,8497,10874,  //���s
        25299,24331,26816,9979,16298,  //���s
        9933,5657,3128,4743,5105,  //���s
        12387,36850,9164,12954,7175,  //���s
        3670,15811,3050,2317,1919,  //���s
        12619,13940,23569,7484,11031,  //���s
        6755,151,1108,2430,6295,  //���s
        8613,5525,1278,3726,6075,  //�ȍs
        7444,5521,5428,1870,4869,  //�͍s
        5692,3711,5661,1810,3518,  //�΍s
        1011,542,751,346,747,  //�ύs
        11128,8810,4120,6124,6851,  //�܍s
        11678,15921,29102,  //��s
        9310,15405,11338,5639,5978,  //��s
        6325,595,56728,  //��s
        3286,  //�[�s
    };

    public static float musicVolume = 0.5f;    //Music����
    public static float seVolume = 0.5f;       //SE����
    public static int operateMode = 2;  //������@ 0:�{�^���̂� 1:�^�b�`�̂� 2:����
    public static int buttonSize = 5;       //�{�^���T�C�Y
    public static int buttonLayout = 0; //�{�^���̔z�u 0:�i�ړ�->�� ��]->�E�j1:�i�ړ�->�E ��]->���j

    public static int maxWordNum = 99999999; //�P��̍ő�ۑ���(�Ƃ�܂قږ����ɐݒ�)

    //public static string[] playerNameRandom = { "�Ƃ���E��","�Ƃ��鑺�l","�Ƃ���h���S��","����������"};
}
