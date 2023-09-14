using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKinds
{
    SE_FindWord = 0,   //�P����݂�����
    SE_BlockMove = 1,  //�u���b�N�𓮂������A�u����
    SE_CanNotMove = 2, //�u���b�N�𓮂����Ȃ���
    SE_Enter = 3,   //���艹
    SE_Countdown = 4,   //�J�E���g�_�E��
    SE_PlayerAttack = 5,    //player�̍U��SE
    SE_SlimeAttack = 6,    //�X���C���̍U��SE
    SE_MinotaurosuAttack = 7,    //�~�m�^�E���X�̍U��SE
    SE_DragonAttack = 8,    //�h���S���̍U��SE
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
