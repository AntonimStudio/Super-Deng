using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HumanoidSceneScript : MonoBehaviour
{
    [SerializeField] private GameObject humanoid1; // Первый объект
    [SerializeField] private GameObject humanoid2; // Второй объект
    [SerializeField] private GameObject humanoidHead1;
    [SerializeField] private GameObject humanoidHead2;
    [SerializeField] private GameObject humanoidIcoHead1;
    [SerializeField] private GameObject humanoidIcoHead2;
    [SerializeField] private GameObject textHitogata;
    [SerializeField] private GameObject textIcobeat; 
    [SerializeField] private PostProcessVolume postProcessVolume; // Постобработка
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioClip soundScream;
    //[SerializeField] private AudioClip music;
    private float timer = 0f;
    //private bool isSwitching = true;
    private Grain grainEffect; // Зернистость
    private Coroutine grainCoroutine;

    private void Start()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out grainEffect);
        }
        humanoid1.SetActive(true);
        humanoid2.SetActive(true);
        humanoidHead1.SetActive(true);
        humanoidHead2.SetActive(true);
        humanoidIcoHead1.SetActive(false);
        humanoidIcoHead2.SetActive(false);
        StartCoroutine(SwitchObjects());
    }

    private IEnumerator SwitchObjects()
    {
        if (grainEffect != null)
        {
            grainCoroutine = StartCoroutine(IncreaseGrainEffect());
        }
        humanoid1.SetActive(true);
        humanoid2.SetActive(true);
        yield return new WaitForSeconds(2f);
        humanoid1.SetActive(true);
        humanoid2.SetActive(false);
        while (timer < 10f)
        {
            timer += 2f;
            humanoid1.SetActive(!humanoid1.activeSelf);
            humanoid2.SetActive(!humanoid2.activeSelf);
            PlaySound();
            yield return new WaitForSeconds(2f);
        }

        // Остановить переключение объектов
        //isSwitching = false;


        humanoid1.SetActive(true);
        humanoid2.SetActive(true);
        textHitogata.SetActive(true);

        yield return new WaitForSeconds(3f);
        DisableHead();
        textHitogata.SetActive(false);
        textIcobeat.SetActive(true);
        //PlayMusic();
    }

    private void DisableHead()
    {
        humanoid1.SetActive(true);
        humanoid2.SetActive(true);
        humanoidHead1.SetActive(false);
        humanoidHead2.SetActive(false);
        humanoidIcoHead1.SetActive(true);
        humanoidIcoHead2.SetActive(true);
    }

    private IEnumerator IncreaseGrainEffect()
    {
        float grainIntensity = 0f;

        while (grainIntensity < 1f) // Увеличиваем до максимального значения
        {
            grainIntensity += Time.deltaTime * 0.1f; // Скорость увеличения
            grainEffect.intensity.value = grainIntensity;
            yield return null;
        }
    }
    private void PlaySound()
    {
        // Проверяем, что звук и источник заданы
        if (audioSourceSFX != null && soundScream != null)
        {
            audioSourceSFX.PlayOneShot(soundScream); // Проигрываем звук переключения
        }
    }
    /*
    private void PlayMusic()
    {
        // Проверяем, что звук и источник заданы
        if (audioSourceMusic != null && music != null)
        {
            audioSourceMusic.PlayOneShot(music); // Проигрываем звук переключения
        }
    }*/
}
