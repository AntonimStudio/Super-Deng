using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform laserShowTransform;
    public bool isTurnOn = false;
    public bool isClockwise = true;
    public float rotationSpeed = 10f;

    private void Update()
    {
        if (isTurnOn)
        {
            float rotation = rotationSpeed * Time.deltaTime;

            if (isClockwise)
            {
                cameraTransform.Rotate(0, 0, rotation);
                laserShowTransform.Rotate(0, 0, rotation);
            }
            else
            {
                cameraTransform.Rotate(0, 0, -rotation);
                laserShowTransform.Rotate(0, 0, -rotation);
            }
        }
    }
}