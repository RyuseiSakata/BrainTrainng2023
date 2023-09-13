using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    
    [SerializeField] Slider musicSlider;        //Music���ʂ̒����X���C�_�[
    [SerializeField] Slider seSlider;           //SE���ʂ̒����X���C�_�[
    [SerializeField] Slider buttonSizeSlider;   //�{�^���T�C�Y�̒����X���C�_�[

    [SerializeField] Text musicVolumeText;  //Music���ʂ̃e�L�X�g
    [SerializeField] Text seVolumeText;     //SE���ʂ̃e�L�X�g
    [SerializeField] Text buttonSizeText;   //�{�^���T�C�Y�̃e�L�X�g

    [SerializeField] Dropdown operateMethodDropdown;    //������@�̃h���b�v�_�E��
    [SerializeField] Toggle[] buttonLayoutToggles;  //�{�^���z�u�̃g�O��

    private void Start()
    {
        musicSlider.value = Config.musicVolume;
        seSlider.value = Config.seVolume;
        buttonSizeSlider.value = Config.buttonSize;
        operateMethodDropdown.value = Config.operateMode;
        buttonLayoutToggles[Config.buttonLayout].isOn = true;
    }

    private void Update()
    {
        Config.musicVolume = musicSlider.value;
        Config.seVolume = seSlider.value;
        Config.buttonSize = Mathf.FloorToInt(buttonSizeSlider.value);
        musicVolumeText.text = (musicSlider.value * 100).ToString("##0") + "��";
        seVolumeText.text = (seSlider.value * 100).ToString("##0") + "��";
        buttonSizeText.text = (Mathf.FloorToInt(buttonSizeSlider.value)).ToString("#0");
    }

    public void pushCloseButton()
    {
        gameObject.SetActive(false);
    }

    //������@�̃h���b�v�_�E�����ύX���ꂽ�Ƃ��ɌĂ�
    public void changedOperationDropdown()
    {
        int selectedValue = operateMethodDropdown.value;
        Config.operateMode = selectedValue;
    }

    //�{�^���z�u�̃g�O���������ꂽ�Ƃ�
    public void selectButtonLayoutToggle(int num)
    {
        if (num == 0 || num == 1)
        {
            Config.buttonLayout = num;
        }
        else
        {
            Debug.Log("�g�O���̔ԍ���0��1�őI�����Ă�������");
        }
    }
}
