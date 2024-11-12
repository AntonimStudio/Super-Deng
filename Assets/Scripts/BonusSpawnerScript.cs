using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class BonusSpawnerScript : MonoBehaviour
{
    private GameObject[] faces;
    [SerializeField] private GameObject prefabBonusCombo;
    [SerializeField] private GameObject prefabBonusHealth;
    [SerializeField] private Material material;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private PlayerScript PS;
    private List<int> numbersOfBonusFaces;

    private void Start()
    {
        faces = FAS.GetAllFaces();
        numbersOfBonusFaces = new List<int>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (numbersOfBonusFaces.Count >= faces.Length - 1)
            {
                return;
            }
            int numb;
            FaceScript FS;
            do 
            { 
                numb = Random.Range(0, 80);
                FS = faces[numb].GetComponent<FaceScript>();
            }
            while (numbersOfBonusFaces.Contains(numb) || FS.havePlayer || FS.isBlinking || FS.isKilling || FS.isBlocked || FS.isBonus);
            numbersOfBonusFaces.Add(numb);
            SetBonus(faces[numb], numb, Random.Range(0, 2));
        }


    }

    private void SetBonus(GameObject face, int numb, int type) //0 - Health, 1 - Combo
    {
        FaceScript FS = faces[numb].GetComponent<FaceScript>();
        FS.isBonus = true;
        FS.rend.material = material;
        if (type == 0)
        {
            GameObject instance = Instantiate(prefabBonusCombo, face.transform);
            instance.transform.localPosition = Vector3.zero;
            //instance.transform.localRotation = Quaternion.identity;
            StartCoroutine(DestroyBonus(face, instance, 10f));
        }
        else
        {
            GameObject instance = Instantiate(prefabBonusHealth, face.transform);
            instance.transform.localPosition = Vector3.zero;
            //instance.transform.localRotation = Quaternion.identity;
            StartCoroutine(DestroyBonus(face, instance, 10f));
        }
    }

    IEnumerator DestroyBonus(GameObject face, GameObject bonus, float delay)
    {
        Debug.Log("HUIaaaaaaaaaaaaaaaa");
        FaceScript FS = face.GetComponent<FaceScript>();
        if (FS.havePlayer)
        {
            Debug.Log("HUI");
            Destroy(bonus);
        }
        yield return new WaitForSeconds(delay);
        Destroy(bonus);
    }
}
