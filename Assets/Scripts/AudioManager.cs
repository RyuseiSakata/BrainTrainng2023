using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKinds
{
    SE_FindWord = 0,   //単語をみつけた音
    SE_BlockMove = 1,  //ブロックを動かす音、置く音
    SE_CanNotMove = 2, //ブロックを動かせないと
    SE_Enter = 3,   //決定音
    SE_Countdown = 4,   //カウントダウン
    SE_PlayerAttack = 5,    //playerの攻撃SE
    SE_SlimeAttack = 6,    //スライムの攻撃SE
    SE_MinotaurosuAttack = 7,    //ミノタウロスの攻撃SE
    SE_DragonAttack = 8,    //ドラゴンの攻撃SE
    BGM_Main = 9,   //BGM
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource seAudioSource;
    [SerializeField] AudioSource bgmAudioSource;

    [SerializeField] AudioClip[] seList;

    private const float defaultPitch = 1.0f;

    private void Start()
    {
        bgmAudioSource.volume = Config.musicVolume;

        
    }

    public void playSeOneShot(AudioKinds audioKinds = AudioKinds.SE_FindWord, float pitch = defaultPitch)
    {
        seAudioSource.pitch = pitch;
        seAudioSource.PlayOneShot(seList[(int)audioKinds], Config.seVolume);
    }

    public IEnumerator playSeOneShotWait(AudioKinds audioKinds = AudioKinds.SE_FindWord, float pitch = defaultPitch)
    {
        seAudioSource.pitch = pitch;
        seAudioSource.PlayOneShot(seList[(int)audioKinds], Config.seVolume);
        yield return new WaitWhile(() => seAudioSource.isPlaying);
        yield break;
    }

    public void playBgm(AudioKinds audioKinds = AudioKinds.BGM_Main)
    {
        bgmAudioSource.clip = seList[(int)audioKinds];
        bgmAudioSource.Play();
    }

    public void pauseBgm()
    {
        bgmAudioSource.Pause();
    }

    public bool isSePlaying()
    {
        return seAudioSource.isPlaying;
    }

}
