using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceArrayScript : MonoBehaviour
{
    [SerializeField] private GameObject[] faces;
    [SerializeField] private Transform Strip_A;
    [SerializeField] private Transform Strip_B;
    [SerializeField] private Transform Strip_C;
    [SerializeField] private Transform Strip_D;
    [SerializeField] private Transform Strip_E;
    [SerializeField] private Transform Strip_F;
    [Space]
    [Header("Y-axis (original)")]
    [SerializeField] private GameObject[] facesStripY_A;
    [SerializeField] private GameObject[] facesStripY_B;
    [SerializeField] private GameObject[] facesStripY_C;
    [SerializeField] private GameObject[] facesStripY_D;
    [SerializeField] private GameObject[] facesStripY_E;
    [SerializeField] private GameObject[] facesStripY_F;
    [Space]
    [Header("X-axis")]
    [SerializeField] private GameObject[] facesStripX_A;
    [SerializeField] private GameObject[] facesStripX_B;
    [SerializeField] private GameObject[] facesStripX_C;
    [SerializeField] private GameObject[] facesStripX_D;
    [SerializeField] private GameObject[] facesStripX_E;
    [SerializeField] private GameObject[] facesStripX_F;
    /*[Space]
    [Header("Z-axis")]
    [SerializeField] private GameObject[] facesStripZ_A;
    [SerializeField] private GameObject[] facesStripZ_B;
    [SerializeField] private GameObject[] facesStripZ_C;
    [SerializeField] private GameObject[] facesStripZ_D;
    [SerializeField] private GameObject[] facesStripZ_E;
    [SerializeField] private GameObject[] facesStripZ_F;*/

    public GameObject[] GetAllFaces()
    {
        return faces;
    }

    public FaceScript[] GetAllFaceScripts()
    {
        FaceScript[] result = new FaceScript[faces.Length];
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i] != null)
            {
                result[i] = faces[i].GetComponent<FaceScript>();
            }
        }
        return result;
    }

    public void ReassembleStripsYX(int numbSet) //Y == 0, X == 1, Z == 2
    {
        if (numbSet == 0)
        {
            ReassembleStrip(Strip_A, facesStripY_A);
            ReassembleStrip(Strip_B, facesStripY_B);
            ReassembleStrip(Strip_C, facesStripY_C);
            ReassembleStrip(Strip_D, facesStripY_D);
            ReassembleStrip(Strip_E, facesStripY_E);
            ReassembleStrip(Strip_F, facesStripY_F);
        }
        else if (numbSet == 1)
        {
            ReassembleStrip(Strip_A, facesStripY_A);
            ReassembleStrip(Strip_B, facesStripY_B);
            ReassembleStrip(Strip_C, facesStripY_C);
            ReassembleStrip(Strip_D, facesStripY_D);
            ReassembleStrip(Strip_E, facesStripY_E);
            ReassembleStrip(Strip_F, facesStripY_F);
        }/*
        else if (numbSet == 2)
        {
            ReassembleStrip(Strip_A, facesStripZ_A);
            ReassembleStrip(Strip_B, facesStripZ_B);
            ReassembleStrip(Strip_C, facesStripZ_C);
            ReassembleStrip(Strip_D, facesStripZ_D);
            ReassembleStrip(Strip_E, facesStripZ_E);
            ReassembleStrip(Strip_F, facesStripZ_F);
        }*/
        
    }

    private void ReassembleStrip(Transform strip, GameObject[] newFaces)
    {

        foreach (GameObject newFace in newFaces)
        {
            if (newFace != null)
            {
                newFace.transform.SetParent(strip);
            }
        }
    }
}
