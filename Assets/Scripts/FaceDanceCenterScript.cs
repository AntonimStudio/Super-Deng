using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceCenterScript : MonoBehaviour
{
    private GameObject[] faces;
    [SerializeField] private FaceArrayScript FAS; 
    public bool isTurnOn = false;
    public bool IsFaceDanceIncrease = false;
    public bool IsFaceDanceDecrease = false;
    public float scaleFactor = 2f;
    public float duration = 0.2f;

    private void Start()
    {
        faces = FAS.GetAllFaces();
    }

    public void SetAllParameters()
    {
        foreach (GameObject face in faces)
        {
            face.GetComponent<FaceDanceScript>().SetParameters(isTurnOn, IsFaceDanceIncrease, IsFaceDanceDecrease, scaleFactor, duration);
        }
    }
}
