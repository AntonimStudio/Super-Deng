using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioClip soundClip;  // Перетащите сюда ваш аудиофайл в инспекторе
    private AudioSource audioSource;

    private void Start()
    {
        // Добавляем компонент AudioSource к объекту, если его еще нет
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    public void TurnOnSound()
    {
        // Воспроизводим звук по клику на объект
        if (audioSource != null && soundClip != null)
        {
            audioSource.Play();
        }
    }
}
