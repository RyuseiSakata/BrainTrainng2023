using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKinds
{
    SE_FindWord = 0,   //�P����݂�����
    SE_BlockMove = 1,  //�u���b�N�𓮂������A�u����
    SE_CanNotMove = 2, //�u���b�N�𓮂����Ȃ���
    BGM_Main = 3,   //BGM
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource seAudioSource;
    [SerializeField] AudioSource bgmAudioSource;

    [SerializeField] AudioClip[] seList;
    [SerializeField] AudioClip[] bgmList;

    private const float defaultPitch = 1.0f;

    private void Start()
    {
        bgmAudioSource.volume = Config.musicVolume;

        playBgm(AudioKinds.BGM_Main);
    }

    public void playSeOneShot(AudioKinds audioKinds = AudioKinds.SE_FindWord, float pitch = defaultPitch)
    {
        seAudioSource.pitch = pitch;
        seAudioSource.PlayOneShot(seList[(int)audioKinds], Config.seVolume);
    }

    public void playBgm(AudioKinds audioKinds = AudioKinds.BGM_Main)
    {
        bgmAudioSource.clip = seList[(int)audioKinds];
        bgmAudioSource.Play();
    }

}
