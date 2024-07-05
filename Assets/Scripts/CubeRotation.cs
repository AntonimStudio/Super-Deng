using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    [Range(0, 20)]
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * 10 * Time.deltaTime);
        //transform.Rotate(Vector3.right, rotationSpeed * 10 * Time.deltaTime);
        //transform.Rotate(Vector3.forward, rotationSpeed * 10 * Time.deltaTime);
    }
}