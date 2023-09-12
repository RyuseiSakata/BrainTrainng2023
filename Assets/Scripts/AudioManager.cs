using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioKinds
{
    FindWord = 0,   //単語をみつけた音
    BlockMove = 1,  //ブロックを動かす音、置く音
    CanNotMove = 2, //ブロックを動かせないと
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
