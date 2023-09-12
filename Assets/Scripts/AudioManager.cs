using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKinds
{
    SE_FindWord = 0,   //単語をみつけた音
    SE_BlockMove = 1,  //ブロックを動かす音、置く音
    SE_CanNotMove = 2, //ブロックを動かせないと
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
