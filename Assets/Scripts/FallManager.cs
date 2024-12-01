using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    private GameObject[] faces;
    private FaceScript[] faceScripts;
    [SerializeField] private FaceArrayScript FAS;
    private List<int> numbersOfFalledFaces;
    public int proximityLimit = 0;
    public bool isResetDelay = false;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private Vector3 centerPoint;
    [SerializeField] private float impulseForce = 10f; 
    [SerializeField] private float torqueStrength = 10f; 
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private AnimationClip animClipFall;
    [SerializeField] private AnimationClip animClipReset;
    public float resetDelayTime = 0f;
    private bool waitForDeath = false;
    private bool isReset = false;
    public bool isTurnOn = false;
    public bool isRandomSpawnTime = false;
    public int colvo = 0;
    public List<int> faceIndices = new();

    private void Start()
    {
        faces = FAS.GetAllFaces();
        faceScripts = FAS.GetAllFaceScripts();
        numbersOfFalledFaces = new List<int>();
        foreach (GameObject face in faces)
        {
            Rigidbody rb = face.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
            }
            Animator animator = face.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
        }
    }

    public void StartSettingFallFace()
    {
        
        if (numbersOfFalledFaces.Count >= 79) return;
        if (isTurnOn)
        {
            List<int> availableFaces = new List<int>();

            for (int i = 0; i < faces.Length; i++)
            {
                FaceScript FS = faces[i].GetComponent<FaceScript>();
                if (//!FS.havePlayer &&
                    !FS.isBlinking &&
                    !FS.isKilling &&
                    !FS.isBlocked &&
                    !FS.isColored &&
                    !FS.isPortal &&
                    !FS.isBonus &&
                    FS.pathObjectCount >= proximityLimit)
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


                    numbersOfFalledFaces.Add(selectedFaceIndex);
                    StartCoroutine(PlayAnimationFall(faces[selectedFaceIndex], selectedFaceIndex));
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    numbersOfFalledFaces.Add(index);
                    StartCoroutine(PlayAnimationFall(faces[index], index));

                }
            }
        }
    }

    private IEnumerator PlayAnimationFall(GameObject face, int numb)
    {
        FaceScript FS = face.GetComponent<FaceScript>();    
        Animator animator = face.GetComponent<Animator>();
        if (animator != null && animClipFall != null)
        {
            animator.enabled = true;
            FS.isBlinking = true;
            animator.Play(animClipFall.name);
            yield return new WaitForSeconds(animClipFall.length);
        }
        FS.isBlocked = true;
        FS.isBlinking = false;
        animator.enabled = false;
        ApplyImpulse(face, numb);
    }

    private void ApplyImpulse(GameObject face, int numb)
    {
        Rigidbody rb = face.GetComponent<Rigidbody>();
        var initialPosition = rb.transform.position;
        var initialRotation = rb.transform.rotation;
        var initialLocalPosition = rb.transform.localPosition;
        var initialLocalRotation = rb.transform.localRotation;
        if (face.GetComponent<FaceScript>().havePlayer) 
        {
            face.GetComponent<FaceScript>().havePlayer = false;
            waitForDeath = true;
        }

        Vector3 direction = (rb.transform.position - centerPoint).normalized;
        rb.AddForce(direction * impulseForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * torqueStrength;

        rb.AddTorque(randomTorque, ForceMode.Impulse);

        StartCoroutine(ResetAfterDelay(face, initialPosition, initialRotation, initialLocalPosition, initialLocalRotation, delay, numb));
    }

    IEnumerator ResetAfterDelay(GameObject face, Vector3 initialPosition, Quaternion initialRotation, Vector3 initialLocalPosition, Quaternion initialLocalRotation, float delay, int numb)
    {
        Rigidbody rb = face.GetComponent<Rigidbody>();

        if (isReset)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.transform.position = initialPosition;
            rb.transform.rotation = initialRotation;
            rb.transform.localPosition = initialLocalPosition;
            rb.transform.localRotation = initialLocalRotation;
        }

        yield return new WaitForSeconds(delay);
        if (waitForDeath)
        {
            PS.Lose();
        }
        
        Renderer[] childRenderers = face.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false; // Отключаем рендеринг у всех дочерних объектов
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = initialPosition;
        rb.transform.rotation = initialRotation;
        rb.transform.localPosition = initialLocalPosition;
        rb.transform.localRotation = initialLocalRotation;
    }

    public void ResetFall()
    {
        isReset = true;
        foreach (GameObject face in faces)
        {
            Renderer[] childRenderers = face.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in childRenderers)
            {
               if (!renderer.enabled && !face.GetComponent<FaceScript>().havePlayer)
               {
                    StartCoroutine(PlayAnimationReset(face));
                    renderer.enabled = true;
               }
            }
            
            face.GetComponent<FaceScript>().isBlocked = false;
        }
        for (int numb = 0; numb <= 80; numb++)
        {
            if (numbersOfFalledFaces.Contains(numb))
            {
                numbersOfFalledFaces.Remove(numb);
            }
        }
    }

    private IEnumerator PlayAnimationReset(GameObject face)
    {
        foreach (FaceScript FS in faceScripts)
        {
            FS.ResetRightLeftTop();
        }
        Animator animator = face.GetComponent<Animator>();
        animator.enabled = true;
        if (animator != null && animClipReset != null)
        {
            animator.enabled = true;
            animator.Play(animClipReset.name);
            yield return new WaitForSeconds(animClipReset.length);
        }
        foreach (FaceScript FS in faceScripts)
        {
            FS.ResetRightLeftTop();
        }
        animator.enabled = false;
        isReset = false;
    }

    private void SetPartsMaterial(Material material)
    {
        PS.rendPartTop.material = material;
        PS.rendPartMiddle.material = material;
        PS.rendPartLeft.material = material;
        PS.rendPartRight.material = material;
    }
}
