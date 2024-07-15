using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RhythmManager : MonoBehaviour
{
    [SerializeField] private float bpm = 90f;
    public float beatInterval; // Длительность одного такта в секундах
    private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;
   
    private void Awake()
    {
        beatInterval = 60f / bpm;
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    private void Update()
    {
        foreach (Intervals interval in intervals)
        {
            float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * steps);
    }

    public void CheckForNewInterval (float interval)
    {
        if (Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            trigger.Invoke();
        }
    }
}