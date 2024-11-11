using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Android.Types;

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
    private GameObject faceCurrent;
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

    private bool inBlinking = false;

    private void Awake()
    {
        rendPartTop = glowingPartTop.GetComponent<MeshRenderer>();
        rendPartMiddle = glowingPartMiddle.GetComponent<MeshRenderer>();
        rendPartLeft = glowingPartLeft.GetComponent<MeshRenderer>();
        rendPartRight = glowingPartRight.GetComponent<MeshRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage();
        }
        if (Input.GetKeyDown(KeyCode.Q))
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

    private void ResetMaterials()
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
        else if (hp == 0)
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
    }

    public void TakeDamage()
    {
        if (hp > 1)
        {
            hp -= 1;
        }
        else
        {
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
        if (hp <= 4)
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

    public void Lose()
    {
        faceCurrent.GetComponent<FaceScript>().havePlayer = false;
        rendPartMiddle.material = materialTurnOff;
        imageLose.gameObject.SetActive(true);
        if (TC != null)
        {
            TC.timerIsRunning = false;
        }
        RFS.isTurnOn = false;
        SCD.isOn = false;
    }
}
