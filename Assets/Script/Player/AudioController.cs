using System.Diagnostics.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用静态类生成实例
public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioSource audioSource;
    [SerializeField] private AudioClip exchangeAudio, pistolAudio, shotgunAudio, rocketAudio, keyAudio, drinkAudio, successAudio;
    private void Awake()
    {
        instance = this;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    public void ExchangeAudio()
    {
        audioSource.clip = exchangeAudio;
        audioSource.Play();
    }

    public void PistolAudio()
    {
        audioSource.clip = pistolAudio;
        audioSource.Play();
    }

    public void ShotgunAudio()
    {
        audioSource.clip = shotgunAudio;
        audioSource.Play();
    }

    public void RocketAudio()
    {
        audioSource.clip = rocketAudio;
        audioSource.Play();
    }

    public void KeyAudio()
    {
        audioSource.clip = keyAudio;
        audioSource.Play();
    }

    public void DrinkAudio()
    {
        audioSource.clip = drinkAudio;
        audioSource.Play();
    }
    public void SuccessAudio()
    {
        audioSource.clip = successAudio;
        audioSource.Play();
    }
}
