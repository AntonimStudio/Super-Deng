using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceCenterScript : MonoBehaviour
{
    private GameObject[] faces;
    [SerializeField] private FaceArrayScript FAS; 
    public bool isTurnOn = false;
    public bool isChanging = false;
    public bool inProcess = false;
    public bool IsFaceDanceIncrease = false;
    public float durationChangingFaceDance;
    public float scaleFactor = 2f;
    public float duration = 0.2f;

    private void Start()
    {
        faces = FAS.GetAllFaces();
    }

    private void Update()
    {
        if (isChanging && !inProcess)
        {
            SetIncreaseAllFaces();
            inProcess = true;
        }
        if (!isChanging)
        {
            inProcess = false;
        }
    }

    public void SetAllParameters()
    {
        foreach (GameObject face in faces)
        {
            face.GetComponent<FaceDanceScript>().SetParameters(isTurnOn, isChanging, IsFaceDanceIncrease, scaleFactor, duration);
        }
    }

    public void SetIncreaseAllFaces()
    {
        foreach (GameObject face in faces)
        {
            face.GetComponent<FaceDanceScript>().AdjustEffectIntensity(IsFaceDanceIncrease ? scaleFactor : 1f, durationChangingFaceDance);
        }
    }
}
