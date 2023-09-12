using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKinds
{
    FindWord = 0,   //�P����݂�����
    BlockMove = 1,  //�u���b�N�𓮂������A�u����
    CanNotMove = 2, //�u���b�N�𓮂����Ȃ���
}

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] AudioClip[] seList;

    private const float defaultPitch = 1.0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playSeOneShot(AudioKinds audioKinds = AudioKinds.FindWord, float pitch = defaultPitch)
    {
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(seList[(int)audioKinds], Config.seVolume);
    }

}
