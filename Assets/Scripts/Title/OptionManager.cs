using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [SerializeField] GameObject soundButton;
    [SerializeField] GameObject operateButton;

    [SerializeField] Button adventureButton;    //アドベンチャーボタン

    [SerializeField] AudioManager audioManager;

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
        audioManager.bgmAudioSource.volume = Config.musicVolume;
        Config.seVolume = seSlider.value;
        Config.buttonSize = Mathf.FloorToInt(buttonSizeSlider.value);
        musicVolumeText.text = (musicSlider.value * 100).ToString("##0") + "％";
        seVolumeText.text = (seSlider.value * 100).ToString("##0") + "％";
        buttonSizeText.text = (Mathf.FloorToInt(buttonSizeSlider.value)).ToString("#0");

        if (Input.anyKeyDown){
            if(EventSystem.current.currentSelectedGameObject == soundButton)
            {
                soundButton.GetComponent<Image>().color = new Color32(0, 0, 0, 200);
                operateButton.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                soundButton.transform.GetChild(1).gameObject.SetActive(true);
                operateButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else if(EventSystem.current.currentSelectedGameObject == operateButton)
            {
                soundButton.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                operateButton.GetComponent<Image>().color = new Color32(0, 0, 0, 200);
                soundButton.transform.GetChild(1).gameObject.SetActive(false);
                operateButton.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void pushCloseButton()
    {
        gameObject.SetActive(false);
        soundButton.transform.GetChild(1).gameObject.SetActive(true);
        operateButton.transform.GetChild(1).gameObject.SetActive(false);
        adventureButton.Select();  //アドベンチャーボタンを選択状態に
    }

    public void pushSoundButton()
    {
        soundButton.GetComponent<Button>().Select();
        soundButton.transform.GetChild(1).gameObject.SetActive(true);
        operateButton.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void pushOperateButton()
    {
        operateButton.GetComponent<Button>().Select();
        soundButton.transform.GetChild(1).gameObject.SetActive(false);
        operateButton.transform.GetChild(1).gameObject.SetActive(true);
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
