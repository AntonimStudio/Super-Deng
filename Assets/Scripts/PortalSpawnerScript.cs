using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalSpawnerScript : MonoBehaviour
{
    private FaceScript[] faceScripts;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Material materialBasic;
    [SerializeField] private Material materialPortal;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private Image panel;
    [SerializeField] private AnimationClip animClip;
    [SerializeField] private float delay;
    [SerializeField] private int indexScene;

    private void Start()
    {
        faceScripts = FAS.GetAllFaceScripts();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            List<int> availableFaces = new List<int>();

            for (int i = 0; i < faceScripts.Length; i++)
            {

                if (!faceScripts[i].havePlayer &&
                    !faceScripts[i].isBlinking &&
                    !faceScripts[i].isKilling &&
                    !faceScripts[i].isBlocked &&
                    !faceScripts[i].isColored &&
                    !faceScripts[i].isPortal &&
                    !faceScripts[i].isBonus)
                {
                    availableFaces.Add(i);
                }
            }

            if (availableFaces.Count == 0) return;

            int selectedFaceIndex = availableFaces[Random.Range(0, availableFaces.Count)];

            SetPortal(faceScripts[selectedFaceIndex]);
        }
    }

    private void SetPortal(FaceScript face) //0 - Health, 1 - Combo
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FS.isPortal = true;
        FS.rend.material = materialPortal;

        GameObject instance = Instantiate(prefab, face.transform);
        instance.transform.localPosition = Vector3.zero;
        StartCoroutine(DestroyBonus(face, instance, delay));
    }

    private IEnumerator DestroyBonus(FaceScript face, GameObject bonus, float delay)
    {
        yield return new WaitForSeconds(delay);
        face.isPortal = false;
        panel.enabled = true;
        Destroy(bonus);
    }

    public void LoadSecretScene()
    {
        StartCoroutine(LoadingScene());
    }

    private IEnumerator LoadingScene()
    {
        panel.enabled = true;
        panel.GetComponent<Animator>().enabled = true;
        panel.GetComponent<Animator>().Play(animClip.name);
        yield return new WaitForSeconds(animClip.length);
        SceneManager.LoadScene(indexScene);
    }
}
