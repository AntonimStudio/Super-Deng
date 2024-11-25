using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.XR;

public class BonusSpawnerScript : MonoBehaviour
{
    private GameObject[] faces;
    private FaceScript[] faceScripts;
    [SerializeField] private GameObject prefabBonusCombo;
    [SerializeField] private GameObject prefabBonusHealth;
    [SerializeField] private Material materialBasic;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private ComboManager CM;
    [SerializeField] private AnimationClip animClip;
    [SerializeField] private float delay;
    private List<int> numbersOfBonusFaces;

    private void Start()
    {
        faces = FAS.GetAllFaces();
        faceScripts = FAS.GetAllFaceScripts();
        numbersOfBonusFaces = new List<int>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
                    !faceScripts[i].isBonus)
                {
                    availableFaces.Add(i);
                }
            }

            if (availableFaces.Count == 0) return;

            int selectedFaceIndex = availableFaces[Random.Range(0, availableFaces.Count)];

            SetBonus(faceScripts[selectedFaceIndex].gameObject, Random.Range(0, 2));
        }
    }

    private void SetBonus(GameObject face, int type) //0 - Health, 1 - Combo
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FS.isBonus = true;
        FS.rend.material = materialPlayer;

        GameObject selectedPrefab = type == 0 ? prefabBonusCombo : prefabBonusHealth;
        GameObject instance = Instantiate(selectedPrefab, face.transform);
        instance.transform.localPosition = Vector3.zero;
        Animator animator = instance.GetComponent<Animator>();
        animator.enabled = false;
        StartCoroutine(DestroyBonus(face, instance, delay));
    }

    public void GetComboBonus()
    {
        CM.Double();
    }

    private IEnumerator DestroyBonus(GameObject face, GameObject bonus, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bonus != null)
        { 
            Animator animator = bonus.GetComponent<Animator>();
            if (animator != null && animClip != null)
            {
                animator.enabled = true;
                animator.Play(animClip.name);
                yield return new WaitForSeconds(animClip.length);
            }
            FaceScript FS = face.GetComponent<FaceScript>();
            FS.rend.material = materialBasic;
            FS.isBonus = false;
            animator.enabled = false;
        }
        Destroy(bonus);
    }
}
