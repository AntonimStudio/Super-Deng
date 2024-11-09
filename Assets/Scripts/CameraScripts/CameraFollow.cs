using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // The player object to follow
    [SerializeField] private Transform center; // The center object to look at
    [SerializeField] private float followSpeed = 5f; // Speed of the camera following the player

    private void Start()
    {
        transform.position = player.position;
        Vector3 direction = center.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        Vector3 direction = center.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}