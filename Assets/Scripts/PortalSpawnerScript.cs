using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public int proximityLimit = 0;
    public int colvo = 0;
    public bool isTurnOn = false;
    public bool isRandomSpawnTime = false;
    public List<int> faceIndices = new();

    private void Start()
    {
        faceScripts = FAS.GetAllFaceScripts();
    }

    public void StartSettingPortal()
    {
        if (isTurnOn)
        {
            List<int> availableFaces = new List<int>();

            for (int i = 0; i < faceScripts.Length; i++)
            {
                if (!faceScripts[i].havePlayer &&
                    !faceScripts[i].isRight &&
                    !faceScripts[i].isLeft &&
                    !faceScripts[i].isTop &&
                    !faceScripts[i].isBlinking &&
                    !faceScripts[i].isKilling &&
                    !faceScripts[i].isBlocked &&
                    !faceScripts[i].isColored &&
                    !faceScripts[i].isPortal &&
                    !faceScripts[i].isBonus &&
                    faceScripts[i].pathObjectCount >= proximityLimit)
                {
                    availableFaces.Add(i);
                }
            }

            if (isRandomSpawnTime)
            {
                //Debug.Log(colvo);
                for (int i = 0; i < colvo; i++)
                {
                    if (availableFaces.Count == 0) return;

                    int randomIndex = Random.Range(0, availableFaces.Count);
                    int selectedFaceIndex = availableFaces[randomIndex];
                    SetPortal(faceScripts[selectedFaceIndex]);
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    SetPortal(faceScripts[index]);
                }
            }
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
        //panel.GetComponent<Animator>().Play(animClip.name);
        yield return new WaitForSeconds(animClip.length);
        SceneManager.LoadScene(indexScene);
    }
}
