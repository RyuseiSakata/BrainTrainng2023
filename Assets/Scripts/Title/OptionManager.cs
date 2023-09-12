using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    
    [SerializeField] Slider musicSlider;        //Music音量の調整スライダー
    [SerializeField] Slider seSlider;           //SE音量の調整スライダー
    [SerializeField] Slider buttonSizeSlider;   //ボタンサイズの調整スライダー

    [SerializeField] Text musicVolumeText;  //Music音量のテキスト
    [SerializeField] Text seVolumeText;     //SE音量のテキスト
    [SerializeField] Text buttonSizeText;   //ボタンサイズのテキスト

    [SerializeField] Dropdown operateMethodDropdown;    //操作方法のドロップダウン
    [SerializeField] Toggle[] buttonLayoutToggles;  //ボタン配置のトグル

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
        musicVolumeText.text = (musicSlider.value * 100).ToString("##0") + "％";
        seVolumeText.text = (seSlider.value * 100).ToString("##0") + "％";
        buttonSizeText.text = (Mathf.FloorToInt(buttonSizeSlider.value)).ToString("#0");
    }

    public void pushCloseButton()
    {
        gameObject.SetActive(false);
    }

    //操作方法のドロップダウンが変更されたときに呼ぶ
    public void changedOperationDropdown()
    {
        int selectedValue = operateMethodDropdown.value;
        Config.operateMode = selectedValue;
    }

    //ボタン配置のトグルが押されたとき
    public void selectButtonLayoutToggle(int num)
    {
        if (num == 0 || num == 1)
        {
            Config.buttonLayout = num;
        }
        else
        {
            Debug.Log("トグルの番号を0か1で選択してください");
        }
    }
}
