using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    //화면 흔들림 구현 위한 CinemachineSource
    public CinemachineImpulseSource impulseSource;


    public AudioClip NormalAttackSound; 
    public AudioClip PowerAttackSound;
    public AudioClip FireBreathSound;
    public AudioClip TauntSound;
    public AudioClip SpinAttackSound; 
    public AudioClip GruntSound; 
    public AudioClip GruntSound2; 

    public void Atk1Audio()
    {
        audioSource.clip = NormalAttackSound;
        audioSource.Play();
    }

    public void Atk2Audio()
    {
        audioSource.clip = PowerAttackSound;
        audioSource.Play();
    }

    public void Atk3Audio()
    {
        audioSource.clip = SpinAttackSound;
        audioSource.Play();
    }

    public void GruntAudio()
    {
        audioSource.clip = GruntSound;
        audioSource.Play();
    }
   
    public void TauntAudio()
    {
        audioSource.clip = TauntSound;
        audioSource.Play();
        impulseSource.GenerateImpulse();
    }

    public void FireBreathAudio()
    {
        audioSource.clip = FireBreathSound;
        audioSource.Play();
    }

    public void PowerUpSound()
    {
        audioSource.clip = PowerAttackSound;
        audioSource.Play();
        impulseSource.GenerateImpulse();
    }
}
