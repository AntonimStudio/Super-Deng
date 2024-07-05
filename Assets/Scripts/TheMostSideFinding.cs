using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMostSideFinding : MonoBehaviour
{
    [SerializeField] private GameObject[] cubes;
    public GameObject rightMostCube;
    public GameObject leftMostCube;
    public GameObject nearMostCube;
    public GameObject farMostCube;

    void Update()
    {
        if (cubes.Length > 0)
        {
            rightMostCube = cubes[0];
            leftMostCube = cubes[0];
            nearMostCube = cubes[0];
            farMostCube = cubes[0];
            foreach (GameObject cube in cubes)
            {
                if (cube.transform.position.x > rightMostCube.transform.position.x)
                {
                    rightMostCube = cube;
                }
                else if (cube.transform.position.x < leftMostCube.transform.position.x)
                {
                    leftMostCube = cube;
                }

                if (cube.transform.position.z > nearMostCube.transform.position.z)
                {
                    nearMostCube = cube;
                }
                else if (cube.transform.position.z < farMostCube.transform.position.z)
                {
                    farMostCube = cube;
                }
            }
        }
        else
        {
            Debug.LogWarning("No cubes found in the scene.");
        }
    }
}

