using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10.0f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float shakeY = Mathf.Sin(Time.time * shakeSpeed * 1.2f) * shakeAmount;

        transform.localPosition = originalPosition + new Vector3(shakeX, shakeY, 0);
    }
}
