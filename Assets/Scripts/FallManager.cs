using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    public Vector3 centerPoint; // Точка, из которой будет направлен импульс
    public float impulseForce = 10f; // Сила импульса
    public float torqueStrength = 10f; // Сила импульса

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    public Rigidbody rb;

    void Start()
    {
        // Сохраняем начальное положение и вращение объекта
        initialPosition = rb.transform.position;
        initialRotation = rb.transform.rotation;
        initialLocalPosition = rb.transform.localPosition;
        initialLocalRotation = rb.transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyImpulse();
            StartCoroutine(ResetAfterDelay(1.5f));
        }
    }

    void ApplyImpulse()
    {
        Vector3 direction = (rb.transform.position - centerPoint).normalized;
        rb.AddForce(direction * impulseForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * torqueStrength;

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = initialPosition;
        rb.transform.rotation = initialRotation;
        rb.transform.localPosition = initialLocalPosition;
        rb.transform.localRotation = initialLocalRotation;
    }
}