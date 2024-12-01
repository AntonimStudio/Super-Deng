using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanSceneScript : MonoBehaviour
{
    public float scaleBasic = 1f;
    public float scaleFactorMajor = 10f;
    public float scaleFactorMinor = 7.5f;
    public float delay = 1f;
    public GameObject[] lines;
    public GameObject[] diodArray;

    private void Start()
    {

        List<GameObject> diodObjects = new List<GameObject>();

        foreach (GameObject line in lines)
        {
            DIOD[] diodComponents = line.GetComponentsInChildren<DIOD>();

            foreach (DIOD diod in diodComponents)
            {
                diodObjects.Add(diod.gameObject);
                diod.transform.localScale = new Vector3(scaleBasic, scaleBasic/2, scaleBasic); 
            }
        }
        diodArray = diodObjects.ToArray();

        StartCoroutine(Scan());
    }

    private IEnumerator Scan()
    {
        for (int i = 0; i < diodArray.Length; i++)
        {
            diodArray[i].transform.localScale *= scaleFactorMajor;
            for (int j = 0; j < diodArray.Length; j++)
            {
                if (i != j)
                {
                    diodArray[j].transform.localScale *= scaleFactorMinor;
                }
            }
            yield return new WaitForSeconds(delay); // Wait before moving to the next object
            diodArray[i].transform.localScale /= scaleFactorMajor;
            for (int j = 0; j < diodArray.Length; j++)
            {
                if (i != j)
                {
                    diodArray[j].transform.localScale /= scaleFactorMinor;
                }
            }
            yield return new WaitForSeconds(delay/2);
        }
    }
}