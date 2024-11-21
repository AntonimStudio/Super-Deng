using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private int hp = 4;
    [SerializeField] private GameObject glowingPartTop;
    [SerializeField] private GameObject glowingPartMiddle;
    [SerializeField] private GameObject glowingPartLeft;
    [SerializeField] private GameObject glowingPartRight;
    [Space]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animClipBlink;
    [SerializeField] private AudioClip sound;
    [SerializeField] private GameObject faceCurrent;
    private FaceScript faceCurrentFS;
    [SerializeField] private Material materialTurnOn;
    [SerializeField] private Material materialTurnOff;
    [Space]
    public MeshRenderer rendPartTop;
    public MeshRenderer rendPartMiddle;
    public MeshRenderer rendPartLeft;
    public MeshRenderer rendPartRight;


    [SerializeField] private Image imageLose;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private LightShutDownScript LSDS;
    [SerializeField] private IcoSphereDanceScript ISDS;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceGameOver;
    [SerializeField] private float fadeDuration = 2f;

    private bool inBlinking = false;
    private bool isLosing = false;
    private bool inTakingDamage = false;

    private void Awake()
    {
        
        rendPartTop = glowingPartTop.GetComponent<MeshRenderer>();
        rendPartMiddle = glowingPartMiddle.GetComponent<MeshRenderer>();
        rendPartLeft = glowingPartLeft.GetComponent<MeshRenderer>();
        rendPartRight = glowingPartRight.GetComponent<MeshRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }

    private void Start()
    {
        faceCurrentFS = faceCurrent.GetComponent<FaceScript>();
    }

    private void Update()
    {
        if (faceCurrentFS.isKilling && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
            StartCoroutine(PlayAnimationTakeDamage());
        }
        if (Input.GetKeyDown(KeyCode.Q) && !inTakingDamage)
        {
            inTakingDamage = true;
            TakeDamage();
            StartCoroutine(PlayAnimationTakeDamage());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeHP();
        }

        if (faceCurrent.GetComponent<FaceScript>().isBlinking && !inBlinking)
        {
            inBlinking = true;
            if (animator != null && animClipBlink != null)
            {
                animator.enabled = true;
                animator.Play(animClipBlink.name); // Проигрываем анимацию
            }
        }
        else if (!faceCurrent.GetComponent<FaceScript>().isBlinking && inBlinking)
        {
            inBlinking = false;
            animator.enabled = false;
            ResetMaterials();
        }
    }

    public void ResetMaterials()
    {
        if (hp == 4)
        {
            rendPartTop.material = materialTurnOn;
            rendPartMiddle.material = materialTurnOn;
            rendPartLeft.material = materialTurnOn;
            rendPartRight.material = materialTurnOn;
        }
        else if (hp == 3)
        {
            rendPartRight.material = materialTurnOn;
            rendPartTop.material = materialTurnOff;
            rendPartMiddle.material = materialTurnOn;
            rendPartLeft.material = materialTurnOn;
        }
        else if (hp == 2)
        {
            rendPartRight.material = materialTurnOff;
            rendPartTop.material = materialTurnOff;
            rendPartMiddle.material = materialTurnOn;
            rendPartLeft.material = materialTurnOn;
        }
        else if (hp == 1)
        {
            rendPartRight.material = materialTurnOff;
            rendPartTop.material = materialTurnOff;
            rendPartMiddle.material = materialTurnOn;
            rendPartLeft.material = materialTurnOff;
        }
        else
        {
            rendPartRight.material = materialTurnOff;
            rendPartTop.material = materialTurnOff;
            rendPartMiddle.material = materialTurnOff;
            rendPartLeft.material = materialTurnOff;
        }
    }
    
    public void SetCurrentFace(GameObject face)
    {
        faceCurrent = face;
        faceCurrentFS = face.GetComponent<FaceScript>();
    }
    
    public void TakeDamage()
    {
        if (hp > 1)
        {
            hp -= 1;
        }
        else if (!isLosing)
        {
            hp -= 1;
            Lose();
        }
        
        if (hp <= 3)
        {
            rendPartTop.material = materialTurnOff;
        }
        if (hp <= 2)
        {
            rendPartRight.material = materialTurnOff;
        }
        if (hp <= 1)
        {
            rendPartLeft.material = materialTurnOff;
        }

    }

    public void TakeHP()
    {
        if (hp < 4)
        {
            hp += 1;
        }
        if (hp == 2)
        {
            rendPartLeft.material = materialTurnOn;
        }
        if (hp == 3)
        {
            rendPartRight.material = materialTurnOn;
        }
        if (hp == 4)
        {
            rendPartTop.material = materialTurnOn;
        }
    }

    private IEnumerator PlayAnimationTakeDamage()
    {
        if (animator != null && animClipBlink != null)
        {
            animator.enabled = true;
            animator.Play(animClipBlink.name);
            yield return new WaitForSeconds(animClipBlink.length); // Ждем завершения анимации
        }
        ResetMaterials();
        animator.enabled = false;
        inTakingDamage = false;
    }

    public void Lose()
    {
        isLosing = true;
        //faceCurrent.GetComponent<FaceScript>().havePlayer = false;
        rendPartMiddle.material = materialTurnOff;
        RFS.isTurnOn = false;
        SCD.isOn = false;
        ISDS.isOn = false;
        if (TC != null)
        {
            TC.timerIsRunning = false;
        }
        if (LSDS != null)
        {
            LSDS.StartShutDown();
        }
        else ShowImage();
        audioSourceGameOver.clip = sound;
        audioSourceGameOver.Play();

        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {

        float startVolume = audioSourceMusic.volume;

        while (audioSourceMusic.volume > 0)
        {
            audioSourceMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSourceMusic.volume = 0;
        audioSourceMusic.Stop();
    }

    public void ShowImage()
    {
        imageLose.gameObject.SetActive(true);
    }

    public void SetPartsMaterial(Material material)
    {
        rendPartTop.material = material;
        rendPartMiddle.material = material;
        rendPartLeft.material = material;
        rendPartRight.material = material;
    }
}
