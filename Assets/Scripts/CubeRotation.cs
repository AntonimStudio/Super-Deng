using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    [Range(0, 20)]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isTimer;
    [SerializeField] private bool clockwiseRotation = true;

    private void Start()
    {
        if (isTimer)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (clockwiseRotation) transform.Rotate(Vector3.up, rotationSpeed * 10 * Time.deltaTime);
        else transform.Rotate(Vector3.up, - rotationSpeed * 10 * Time.deltaTime);
    }
}