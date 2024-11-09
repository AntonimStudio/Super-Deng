using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    [SerializeField] private GameObject[] faces;
    private List<int> numbersOfFalledFaces;

    [SerializeField] private Vector3 centerPoint; // Точка, из которой будет направлен импульс
    [SerializeField] private float impulseForce = 10f; // Сила импульса
    [SerializeField] private float torqueStrength = 10f; // Сила импульса
    [SerializeField] private float delay = 1.5f;

    private void Start()
    {
        numbersOfFalledFaces = new List<int>();
        foreach (GameObject face in faces)
        {
            Rigidbody rb = face.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int numb;
            do {numb = Random.Range(0, 80); }
            while (numbersOfFalledFaces.Contains(numb));
            numbersOfFalledFaces.Add(numb);

            ApplyImpulse(faces[numb], numb);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetFall();
        }
    }

    private void ApplyImpulse(GameObject face, int numb)
    {
        Rigidbody rb = face.GetComponent<Rigidbody>();
        var initialPosition = rb.transform.position;
        var initialRotation = rb.transform.rotation;
        var initialLocalPosition = rb.transform.localPosition;
        var initialLocalRotation = rb.transform.localRotation;

        Vector3 direction = (rb.transform.position - centerPoint).normalized;
        rb.AddForce(direction * impulseForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * torqueStrength;

        rb.AddTorque(randomTorque, ForceMode.Impulse);

        face.GetComponent<FaceScript>().isBlocked = true; ///!!!!!!!!!!!!!!

        StartCoroutine(ResetAfterDelay(face, initialPosition, initialRotation, initialLocalPosition, initialLocalRotation, delay, numb));
    }


    IEnumerator ResetAfterDelay(GameObject face, Vector3 initialPosition, Quaternion initialRotation, Vector3 initialLocalPosition, Quaternion initialLocalRotation, float delay, int numb)
    {
        
        Rigidbody rb = face.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(delay);

        Renderer[] childRenderers = face.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false; // Отключаем рендеринг у всех дочерних объектов
        }

        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = initialPosition;
        rb.transform.rotation = initialRotation;
        rb.transform.localPosition = initialLocalPosition;
        rb.transform.localRotation = initialLocalRotation;
        
        /*
        if (numbersOfFalledFaces.Contains(numb))
        {
            numbersOfFalledFaces.Remove(numb);
        }*/
    }

    public void ResetFall()
    {
        foreach (GameObject face in faces)
        {
            face.GetComponent<FaceScript>().isBlocked = false;
            Renderer[] childRenderers = face.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in childRenderers)
            {
                renderer.enabled = true;
            }
        }
        for (int numb = 0; numb <= 80; numb++)
        {
            if (numbersOfFalledFaces.Contains(numb))
            {
                numbersOfFalledFaces.Remove(numb);
            }
        }
    }
}