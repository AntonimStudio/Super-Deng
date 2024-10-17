using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject glowingPart;
    public MeshRenderer rend;

    private void Awake()
    {
        rend = glowingPart.GetComponent<MeshRenderer>();
    }
}
