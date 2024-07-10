using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public float bpm = 120f;
    public float beatInterval; // Длительность одного такта в секундах
    public float timer = 0.0f;
    private float nextBeatTime; // Время следующего удара
    [SerializeField] TMP_Text textTimer;
    public delegate void OnBeat();
    public static event OnBeat BeatEvent;

    private void Awake()
    {
        beatInterval = 60f / bpm;

        nextBeatTime = Time.time + beatInterval;
    }
    private void Start()
    {
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= beatInterval)
        {
            timer -= beatInterval;
            BeatEvent?.Invoke();
        }
        textTimer.text = "Timer: " + timer.ToString("F2");
    }
}