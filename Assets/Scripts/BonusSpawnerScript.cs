using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public int proximityLimit = 0;
    public int colvo = 0;
    public bool isTurnOn = false;
    public bool isRandomSpawnTime = false;
    public bool isBonusHealth = false;
    public bool isBonusCombo = false;
    public List<int> faceIndices = new();

    private void Start()
    {
        faces = FAS.GetAllFaces();
        faceScripts = FAS.GetAllFaceScripts();
        numbersOfBonusFaces = new List<int>();
    }

    public void StartSettingBonus()
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
                    SetBonus(faceScripts[selectedFaceIndex].gameObject, SetBonusType());
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    SetBonus(faceScripts[index].gameObject, SetBonusType());
                    //Debug.Log(faces[index].name);
                }
            }
        }
    }

    private int SetBonusType()
    {
        int type;
        if (isBonusHealth && isBonusCombo)
        {
            type = Random.Range(0, 2);
        }
        else if (isBonusHealth && !isBonusCombo)
        {
            type = 0;
        }
        else if (!isBonusHealth && isBonusCombo)
        {
            type = 1;
        }
        else
        {
            type = Random.Range(0, 2);
            Debug.Log("ОШИБКА, ВАРЯ, ОШИБКА. Сколько я говорил обращать внимание на красные надписи?!");
        }
        return type;
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
